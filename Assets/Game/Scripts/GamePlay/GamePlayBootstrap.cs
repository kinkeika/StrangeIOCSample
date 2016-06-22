using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;

public class GamePlayBootstrap : ContextView {

    GamePlayContext gamePlayContext = null;

    void Awake() {
        this.context = gamePlayContext = new GamePlayContext(this);
    }

    protected override void OnDestroy ()
    {
        if (this.gamePlayContext != null)
            this.gamePlayContext.ContextUnbind();
    }
}
