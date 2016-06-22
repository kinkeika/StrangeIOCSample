using UnityEngine;
using System.Collections;

using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;

public class CharacterView : View {

    public float rotationDamping = 20f;
    public float speed = 1f;
    public int gravity = 0;
    public Animator animator;
    float verticalVel;  // Used for continuing momentum while in air    
    CharacterController characterController;

    Vector3 targetPoint = Vector3.zero;
    Vector3 moveDirection = Vector3.zero;

    internal void Init()
    {
        characterController = this.transform.GetComponent<CharacterController>();
    }

    public void InitializeView ()
    {
    }

    internal void GameUpdate ()
    {
        UpdateMovement();

        if ( characterController.isGrounded )
            verticalVel = 0f;// Remove any persistent velocity after landing
    }

    float UpdateMovement()
    {

        Vector3 inputVec = moveDirection;
        inputVec *= speed;

        characterController.Move((inputVec + Vector3.up * -gravity + new Vector3(0, verticalVel, 0)) * Time.deltaTime);

        // Rotation
        if (inputVec != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(inputVec), 
                Time.deltaTime * rotationDamping);

        return inputVec.magnitude;
    }

    internal void UpdateTargetPoint (Vector3 _targetPoint)
    {
        targetPoint = _targetPoint;

        var heading = targetPoint - this.transform.position;
        var distance = heading.magnitude;
        moveDirection = heading / distance * speed; // This is now the normalized direction.
    }



}
