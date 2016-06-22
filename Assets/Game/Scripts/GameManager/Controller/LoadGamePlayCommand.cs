using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.command.impl;
using UnityEngine.SceneManagement;

public class LoadGamePlayCommand : EventCommand {

    public override void Execute() {
        Retain();

        SceneManager.LoadScene("GamePlay", LoadSceneMode.Additive);

        Release();
    }
}
