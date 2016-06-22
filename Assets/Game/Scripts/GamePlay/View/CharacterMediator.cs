using UnityEngine;
using System.Collections;

using strange.extensions.mediation.impl;
using strange.extensions.dispatcher.eventdispatcher.api;

public class CharacterMediator : EventMediator {

    //This is how your Mediator knows about your View.
    [Inject]
    public CharacterView view {get; set;}

    [Inject]
    public IGamePlayDataModel gameplayDataModel { get; set; }

    public override void OnRegister() {
        UpdateListeners(true);

        view.Init();
    }

    public override void OnRemove()
    {
        base.OnRemove();
        UpdateListeners(false);
    }

    private void UpdateListeners(bool value)
    {
        dispatcher.UpdateListener(value, GameManagerEvents.GAME_UPDATE, OnGameUpdate);

        if (value)
        {
            //Listen to the global event bus for events
            dispatcher.AddListener(GamePlayEvents.APPLY_NEW_TARGET_POINT, OnChangeTargetPoint);
        }
        else
        {
            //Clean up listeners when the view is about to be destroyed
            dispatcher.RemoveListener(GamePlayEvents.APPLY_NEW_TARGET_POINT, OnChangeTargetPoint);
        }
    }

    private void OnGameUpdate ()
    {
        view.GameUpdate();
    }

    private void OnChangeTargetPoint ()
    {
        view.UpdateTargetPoint(gameplayDataModel.targetPoint);
    }



}
