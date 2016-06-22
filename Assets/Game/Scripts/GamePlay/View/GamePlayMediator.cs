using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.dispatcher.eventdispatcher.api;

public class GamePlayMediator : Mediator {

    [Inject]
    public GamePlayView view {get; set;}

    public override void OnRegister() {
        UpdateListeners(true);
    }

    public override void OnRemove()
    {
        base.OnRemove();
        UpdateListeners(false);
    }

    private void UpdateListeners(bool value)
    {
    }
}
