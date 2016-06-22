using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.command.impl;

public class TargetPointController : EventCommand {

    public override void Execute() {
        Retain();

        UpdateTargetPoint();

        Release();
    }

    void UpdateTargetPoint ()
    {
        dispatcher.Dispatch(GamePlayEvents.APPLY_NEW_TARGET_POINT);  
    }


}
