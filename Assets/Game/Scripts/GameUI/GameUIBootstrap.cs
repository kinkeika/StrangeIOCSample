using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;

public class GameUIBootstrap : ContextView {

    GameUIContext gameUIContext = null;


    void Awake() {
        this.context = gameUIContext = new GameUIContext(this);
    }

    protected override void OnDestroy ()
    {
        if (this.gameUIContext != null)
            this.gameUIContext.ContextUnbind();
    }
}
