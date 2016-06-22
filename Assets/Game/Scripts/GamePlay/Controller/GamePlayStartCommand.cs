using UnityEngine;
using System;
using strange.extensions.context.api;
using strange.extensions.command.impl;

public class GamePlayStartCommand : EventCommand {



    public override void Execute() {
        
        Retain();

        dispatcher.Dispatch(GameManagerEvents.GAME_STATE.READY);

        Release();
    }

}
