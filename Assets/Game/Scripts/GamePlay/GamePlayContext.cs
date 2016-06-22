using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;

public class GamePlayContext : MVCSContext {

    /**
     * Constructor
     */
    public GamePlayContext(MonoBehaviour contextView) : base(contextView) {
    }

    public GamePlayContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
    {
    }

    protected override void mapBindings() {
        base.mapBindings();

        this.ContextBind();

        // bind command
        commandBinder.Bind(ContextEvent.START).To<GamePlayStartCommand>().Once();



        // bind our interface to a concrete implementation
//        injectionBinder.Bind<ICharacterController>().To<CharacterController>().ToSingleton();

    }

    void ContextBind ()
    {

        //Map a mock model and a mock service, both as Singletons
        injectionBinder.Bind<IGamePlayDataModel>().To<GamePlayDataModel>().ToSingleton();

        // bind our view to its mediator
        mediationBinder.Bind<GamePlayView>().To<GamePlayMediator>();
        mediationBinder.Bind<CharacterView>().To<CharacterMediator>();
        mediationBinder.Bind<TargetView>().To<TargetMediator>();

        commandBinder.Bind(GamePlayEvents.UPDATE_TARGET_POINT).To<TargetPointController>();
    }

    public void ContextUnbind ()
    {
        mediationBinder.Unbind<GamePlayView>();
        mediationBinder.Unbind<CharacterView>();
    }
}
