using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.command.impl;
using strange.extensions.dispatcher.eventdispatcher.impl;

public class GameStateCommand : EventCommand {

    [Inject]
    public IGameTimer gameTimer { get; set; }

    public override void Execute() {
        Retain();

        gameTimer.Start();

        Release();

    }
}
