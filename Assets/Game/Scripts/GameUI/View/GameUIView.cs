using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;

public class GameUIView : EventView {

    Text playerCount = null;
    Text score = null;

    public virtual void Init()
    {
        if (score == null)
            score = this.transform.root.FindChild("UICanvas/Offset/Top/Score/Text").GetComponent<Text>();

        // Init score
        UpdateScore(0);
    }

    internal void UpdateScore (long _score = 0)
    {
        score.text = new StringBuilder("SCORE : ").Append(_score).ToString();
    }

}
