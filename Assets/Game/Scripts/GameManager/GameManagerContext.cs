using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;

public class GameManagerContext : MVCSContext {

    /**
     * Constructor
     */
    public GameManagerContext(MonoBehaviour contextView) : base(contextView) {
    }

    public GameManagerContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
    {
    }

    protected override void mapBindings() {
        base.mapBindings();

        this.ContextBind();

        // bind command
        commandBinder.Bind(ContextEvent.START).To<GameManagerStartCommand>().Once();
        commandBinder.Bind(GameManagerEvents.LOAD_GAMEPLAY_SCENE)
            .To<LoadGamePlayCommand>()
            .To<LoadGameUICommand>()
            .InSequence();

        commandBinder.Bind(GameManagerEvents.GAME_STATE.READY).To<GameStateCommand>();

    }

    void ContextBind ()
    {

        //Map a mock model and a mock service, both as Singletons
        injectionBinder.CrossContextBinder.Bind<IGameResultModel>().To<GameResultModel>().ToSingleton();

        // bind our view to its mediator
        mediationBinder.Bind<GameManagerView>().To<GameManagerMediator>();

        // bind controller
        commandBinder.Bind(GamePlayEvents.UPDATE_TARGET_POINT).To<TargetPointController>();

        //  global context bridge binding
        if(crossContextBridge.GetBinding(GameManagerEvents.GAME_UPDATE) == null) 
            crossContextBridge.Bind(GameManagerEvents.GAME_UPDATE);
        if(crossContextBridge.GetBinding(GameManagerEvents.GAME_STATE.READY) == null) 
            crossContextBridge.Bind(GameManagerEvents.GAME_STATE.READY);
    }

    public void ContextUnbind ()
    {
        mediationBinder.Unbind<GameManagerView>();
        commandBinder.Unbind(GamePlayEvents.UPDATE_TARGET_POINT);

        if (injectionBinder.CrossContextBinder.Bind<IGameResultModel>() != null)
            injectionBinder.CrossContextBinder.Unbind<IGameResultModel>();
        if(crossContextBridge.GetBinding(GameManagerEvents.GAME_UPDATE) != null) 
            crossContextBridge.Unbind(GameManagerEvents.GAME_UPDATE);
        if(crossContextBridge.GetBinding(GameManagerEvents.GAME_STATE.READY) != null) 
            crossContextBridge.Unbind(GameManagerEvents.GAME_STATE.READY);
    }


}
