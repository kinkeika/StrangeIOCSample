using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;

public class GameLoop : MonoBehaviour, IGameTimer {

    private bool sendUpdates = false;

    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher{get;set;}

    public GameLoop ()
    {
    }

    public void Start()
    {
        sendUpdates = true;
    }

    public void Stop()
    {
        sendUpdates = false;
    }

    void Update()
    {
        if (sendUpdates && dispatcher != null)
            dispatcher.Dispatch(GameManagerEvents.GAME_UPDATE);
    }
}
