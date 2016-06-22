using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;

public class GameUIContext : MVCSContext {

    /**
     * Constructor
     */
    public GameUIContext(MonoBehaviour contextView) : base(contextView) {
    }

    public GameUIContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
    {
    }

    protected override void mapBindings() {
        base.mapBindings();

        this.ContextBind();


    }

    void ContextBind ()
    {

        // bind our view to its mediator
        mediationBinder.Bind<GameUIView>().To<GameUIMediator>();

        commandBinder.Bind(GameUIEvents.IMPROVE_SCORE).To<ImproveScoreCommand>();
        //  global context bridge binding
        if(crossContextBridge.GetBinding(GameUIEvents.IMPROVE_SCORE) == null) 
            crossContextBridge.Bind(GameUIEvents.IMPROVE_SCORE);

    }

    public void ContextUnbind ()
    {
        mediationBinder.Unbind<GameUIView>();

        if(crossContextBridge.GetBinding(GameUIEvents.IMPROVE_SCORE) != null) 
            crossContextBridge.Unbind(GameUIEvents.IMPROVE_SCORE);
        
    }
}
