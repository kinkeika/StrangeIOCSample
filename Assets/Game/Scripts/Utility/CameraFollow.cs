using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    #region カメラ追従関連パラメータ
    /** 追従するオブジェクト */
    public Transform target;

    /** カメラの視感調整範囲 */
    public float minDragHeight = 3.0f;
    public float maxDragHeight = 15.0f;

    /** Y方向の高さ */
    private float height = 5.0f;
    /** Z方向の距離 */
    private float distance = 10.0f;
    float defaultDistance = 0;
    float farDistance = 0;
    [SerializeField] float farDistanceFactor = 1.5f;

    /** 上下高さのスムーズ移動速度 */
    private float heightDamping = 3.0f;
    /** 左右回転のスムーズ移動速度 */
    private float rotationDamping = 2.0f;
    /** 距離のスムーズ移動速度 */
    private float distanceDamping = 1.0f;

    /** カメラアングル相対値 */
    private float angle;

    /** 回転のドラッグ操作の ON/OFF */
    public bool angleDragOperation = false;

    /** ドラッグ操作での回転速度 */
    private float angleDragSpeed = 10f;

    /** マウス移動始点 */
    private Vector3 startPos;

    /** 高さのドラッグ操作での ON/OFF */
    public bool heightDragOperation = false;

    /** ドラッグ操作での高さ移動速度 */
    private float heightDragSpeed = 2.0f;

    /** 変化先距離 */
    private float wantedDistance;

    [SerializeField] bool uguiTouchOperation = false;
    bool uguiTouchFlag = false;


    /** カメラ自動回転 ON/OFF */
    public bool autoAngleOperation = false;
    // カメラ自動回転、手動回転用
    GameObject _subTarget = null;
    Transform subTarget = null;
    bool cameraSmothDumpFlag = false;
    // カメラ距離係数
    [SerializeField] float cameraDistanceFactor = 5.0f;

    private float _refFixX = 0f;
    private float _refFixY = 0f;
    private float _refFixZ = 0f;
    private float _refXDamping = 0.1f;
    private float _refYDamping = 0.1f;
    private float _refZDamping = 0.1f;

    [SerializeField] bool cameraFollowFlag = false;
    private float _refMoveXDamping = 1.0f;
    private float _refMoveYDamping = 1.0f;
    private float _refMoveZDamping = 1.0f;

    [SerializeField] float cameraMovingDelayDistance = 0;
    #endregion

    Vector3 cameraPositionFactor = new Vector3 (0.6f, 6.5f, -0.6f);

    void Awake ()
    {
        // init param
        this.InitParameter ();
        this.InitTarget(target);

        cameraFollowFlag = false;
    }

    // Use this for initialization
    public void Initialize(Transform _target = null)
    {
        if (_target != null)
            this.transform.position = _target.position + this.cameraPositionFactor;

        // init param
        this.InitParameter ();
        this.InitTarget(_target);

        cameraFollowFlag = false;
    }

    void InitParameter ()
    {
        angle = this.transform.eulerAngles.y;
        height = this.transform.position.y;
        this._subTarget = new GameObject();
        this._subTarget.name = "CameraTargetPoint";
        this.subTarget = this._subTarget.transform;
    }

    public void InitTarget (Transform _target)
    {

        this.target = _target;
        this.subTarget.transform.position = this.target.position;
        this.cameraSmothDumpFlag = false;
        // 距離
        wantedDistance = distance = Vector3.Distance(this.transform.position, this.target.position) + cameraDistanceFactor;
        defaultDistance = wantedDistance;
        farDistance = wantedDistance * farDistanceFactor;
    }

    public void ChangeTarget (Transform _target)
    {
        this.target = _target;
        this.subTarget.transform.position = this.target.position;
        this.cameraSmothDumpFlag = true;
    }
        
    void Update () 
    {

        if (target == null)
            return;
        
        if (this.uguiTouchOperation)
        {

            if (this.uguiTouchFlag)
            {
                if (Input.GetMouseButtonUp(0))
                    this.uguiTouchFlag = false;
                return;
            }
        }
            
        if (angleDragOperation || heightDragOperation) 
        {
            Vector3 movePos = Vector3.zero;
            if (Input.GetMouseButtonDown(0)) {
                startPos = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0)) {
                movePos = Input.mousePosition - startPos;
                startPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0)) {
                startPos = Vector3.zero;
            }
            if (movePos != Vector3.zero) {
                if (angleDragOperation) {
                    angle += movePos.x * angleDragSpeed * Time.deltaTime;
                    if (angle < 0f) {
                        angle += 360f;
                    } else if (angle >= 360f) {
                        angle -= 360f;
                    }
                }
                if (heightDragOperation) 
                {
                    this.height = Mathf.Clamp(this.height - movePos.y * heightDragSpeed * Time.deltaTime, this.minDragHeight, this.maxDragHeight);
                }
            }
        }


    }

    void LateUpdate () 
    {
        if (target == null)
            return;
        
        if (this.uguiTouchFlag)
            return;

//        subTarget.transform.position = target.position;
        this.SetSubTargetPosition();
        this.SmoothDragCamera(isFarCamera);

        //追従先位置
        float wantedRotationAngle = 0;
        if (autoAngleOperation)
            wantedRotationAngle = target.eulerAngles.y + angle;
        else
            wantedRotationAngle = subTarget.eulerAngles.y + angle;

        float wantedHeight = target.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, 
            rotationDamping * Time.deltaTime);


        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
        distance = Mathf.Lerp(distance, wantedDistance, distanceDamping * Time.deltaTime);

        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
        Vector3 pos = Vector3.zero;
        if (autoAngleOperation)
            pos = target.position - currentRotation * Vector3.forward * distance;
        else
            pos = subTarget.position - currentRotation * Vector3.forward * distance;
        pos.y = currentHeight;

        if (cameraSmothDumpFlag && Vector3.Distance (pos, this.transform.position) > 0.5f)
        {
            Vector3 _currentPosition = this.transform.position;
            _currentPosition.x = Mathf.SmoothDamp(_currentPosition.x, pos.x, ref this._refFixX, this._refXDamping);
            _currentPosition.y = Mathf.SmoothDamp(_currentPosition.y, pos.y, ref this._refFixY, this._refYDamping);
            _currentPosition.z = Mathf.SmoothDamp(_currentPosition.z, pos.z, ref this._refFixZ, this._refZDamping);
            this.transform.position = _currentPosition;
        }
        else
        {
            this.cameraSmothDumpFlag = false;
            this.transform.position = pos;
        }



        if (autoAngleOperation)
            this.transform.LookAt(this.target);
        else
            this.transform.LookAt(this.subTarget);

    }



    void SetSubTargetPosition ()
    {
        if (!cameraFollowFlag && Vector3.Distance(subTarget.transform.position, target.position) > cameraMovingDelayDistance)
        {
            cameraFollowFlag = true;
        }

        if (cameraFollowFlag)
        {
            Vector3 _currentPosition = subTarget.transform.position;
            _currentPosition.x = Mathf.SmoothDamp(_currentPosition.x, target.position.x, ref this._refFixX, this._refMoveXDamping);
            _currentPosition.y = Mathf.SmoothDamp(_currentPosition.y, target.position.y, ref this._refFixY, this._refMoveYDamping);
            _currentPosition.z = Mathf.SmoothDamp(_currentPosition.z, target.position.z, ref this._refFixZ, this._refMoveZDamping);
            subTarget.transform.position = _currentPosition;

            if (Vector3.Distance(subTarget.transform.position, target.position) < 0.15f)
            {
                cameraFollowFlag = false;
                subTarget.transform.position = target.position;
            }
                
        }

    }

    private float _refDragFix = 0.0f;
    private float _refDragDamping = 0.3f;
    private bool isFarCamera = true;

    void SmoothDragCamera (bool _isFar = false)
    {
        float _distance = wantedDistance;
        if (_isFar)
        {
            _distance = Mathf.SmoothDamp(_distance, farDistance, ref this._refDragFix, this._refDragDamping);
            wantedDistance = _distance;
        }
        else
        {
            _distance = Mathf.SmoothDamp(_distance, defaultDistance, ref this._refDragFix, this._refDragDamping);
            wantedDistance = _distance;
        }
    }

    public void CameraDragToScreenTouch (bool _isFar = false)
    {
        isFarCamera = _isFar;
    }
}
