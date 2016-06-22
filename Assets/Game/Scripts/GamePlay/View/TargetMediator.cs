using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.dispatcher.eventdispatcher.api;

public class TargetMediator : EventMediator {

    //This is how your Mediator knows about your View.
    [Inject]
    public TargetView view { get; set; }

    [Inject]
    public IGamePlayDataModel gameplayDataModel { get; set; }
    [Inject]
    public IGameResultModel gameResultModel { get; set; }

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
            //Listen to the view for an event
            view.dispatcher.AddListener(GamePlayEvents.TARGET_POINT_MOVED, OnMovedTargetPoint);
            view.dispatcher.AddListener(GamePlayEvents.IMPROVE_SCORE_TO_UI, OnImproveScore);
        }
        else
        {
            //Clean up listeners when the view is about to be destroyed
            view.dispatcher.RemoveListener(GamePlayEvents.TARGET_POINT_MOVED, OnMovedTargetPoint);
            view.dispatcher.RemoveListener(GamePlayEvents.IMPROVE_SCORE_TO_UI, OnImproveScore);
        }
    }

    private void OnGameUpdate ()
    {
        view.GameUpdate();
    }

    private void OnMovedTargetPoint (IEvent evt)
    {
        Vector3 point = (Vector3)evt.data;
        gameplayDataModel.targetPoint = point;

        dispatcher.Dispatch(GamePlayEvents.UPDATE_TARGET_POINT);
    }

    private void OnImproveScore (IEvent evt)
    {
        long point = (long)evt.data;
        gameResultModel.score += point;
        dispatcher.Dispatch(GameUIEvents.IMPROVE_SCORE);
    }

}
