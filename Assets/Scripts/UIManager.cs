using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class UIManager : NetworkBehaviour {

    [SerializeField] private GameObject gameOverTxt;
    [SerializeField] private TextMeshProUGUI scoreTxt;

    [SyncVar (hook = "OnScoreChange")]
    private int score;

    public void UpdateScore()
    {
        score++;
    }

    public void UpdateScoreUI(int score)
    {
        scoreTxt.text = score.ToString();
    }
	
    public void OnScoreChange(int score)
    {
        UpdateScoreUI(score);
    }

    public void DisplayGameOverTxt()
    {
        gameOverTxt.gameObject.SetActive(true);
    }
}
