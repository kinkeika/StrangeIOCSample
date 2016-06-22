using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;

public class GameManagerBootstrap : ContextView {

    GameManagerContext gameManagerContext = null;

    void Awake() {
        this.context = gameManagerContext = new GameManagerContext(this);
    }

    protected override void OnDestroy ()
    {
        if (this.gameManagerContext != null)
            this.gameManagerContext.ContextUnbind();
    }
}
