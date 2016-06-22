using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.command.impl;

public class ImproveScoreCommand : EventCommand {

    public override void Execute() {
        Retain();

        ImproveScore();

        Release();
    }

    void ImproveScore ()
    {
        dispatcher.Dispatch(GameUIEvents.UPDATE_SCORE);  
    }
}
