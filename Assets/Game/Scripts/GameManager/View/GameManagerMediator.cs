using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.dispatcher.eventdispatcher.api;

public class GameManagerMediator : Mediator {

    [Inject]
    public GameManagerView view {get; set;}

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
