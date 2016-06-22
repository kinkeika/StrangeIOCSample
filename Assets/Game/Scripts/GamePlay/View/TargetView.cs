using UnityEngine;
using System.Collections;

using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;

public class TargetView : View {

    [Inject]
    public IEventDispatcher dispatcher{get;set;}

    float time = 0;
    const float intervalTime = 3.0f;

    long score = 10;

    internal void Init()
    {
        
    }

    internal void GameUpdate ()
    {
        time += Time.deltaTime;
        if (time > intervalTime)
        {
            this.transform.position = new Vector3(
                UnityEngine.Random.Range(-10, 10),
                0,
                UnityEngine.Random.Range(-10, 10));
            dispatcher.Dispatch(GamePlayEvents.TARGET_POINT_MOVED, this.transform.position);
            time = 0;

            AddScore();
        }
    }

    void AddScore ()
    {
        dispatcher.Dispatch(GamePlayEvents.IMPROVE_SCORE_TO_UI, score);
    }
}
