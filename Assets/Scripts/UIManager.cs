using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class UIManager : NetworkBehaviour {

    public TextMeshProUGUI scoreTxt;
    [SyncVar (hook = "OnScoreChange")]
    private int score;

    public void UpdateScore()
    {
        score++;
        print(score);
    }

    public void UpdateScoreUI(int score)
    {
        scoreTxt.text = score.ToString();
    }
	
    public void OnScoreChange(int score)
    {
        UpdateScoreUI(score);
    }

    public void PrintEventTest(GameObject test)
    {
        print("Sender + " + test);
    }
}
