using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.command.impl;
using strange.extensions.dispatcher.eventdispatcher.impl;

public class GameManagerStartCommand : EventCommand {

    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject contextView{get;set;}

    [Inject(ContextKeys.CONTEXT)]
    public IContext context{get;set;}

    public override void Execute() {
        Retain();

        //If we're not the first context, we need to shut down the AudioListener
        //This is one way to do it, but there is no "right" way
        if (context != Context.firstContext)
        {

        }

        //MonoBehaviours can only be injected after they've been instantiated manually.
        //Here we create the main GameLoop, attaching it to the ContextView.

        //Attach the GameLoop MonoBehaviour to the contextView...
        contextView.AddComponent<GameLoop>();
        IGameTimer timer = contextView.GetComponent<GameLoop>();
        //...then bind it for injection
        injectionBinder.Bind<IGameTimer>().ToValue(timer);

        LoadGamePlay();

        Release();
    }

    void LoadGamePlay ()
    {
        dispatcher.Dispatch(GameManagerEvents.LOAD_GAMEPLAY_SCENE);
    }
}
