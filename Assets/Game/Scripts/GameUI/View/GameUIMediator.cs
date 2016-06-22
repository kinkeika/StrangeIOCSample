using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.dispatcher.eventdispatcher.api;

public class GameUIMediator : EventMediator {

    [Inject]
    public GameUIView view {get; set;}

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
        if (value)
        {
            //Listen to the global event bus for events
            dispatcher.AddListener(GameUIEvents.UPDATE_SCORE, OnUpdateScore);
        }
        else
        {
            //Listen to the global event bus for events
            dispatcher.RemoveListener(GameUIEvents.UPDATE_SCORE, OnUpdateScore);
        }
    }

    private void OnUpdateScore ()
    {
        view.UpdateScore(gameResultModel.score);
    }
}
