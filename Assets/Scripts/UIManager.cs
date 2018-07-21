using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using RoboRyanTron.Unite2017.Variables;

public class UIManager : NetworkBehaviour {

    [SerializeField] private GameObject gameOverTxt;
    [SerializeField] private TextMeshProUGUI scoreTxt;
    [SerializeField] private TextMeshProUGUI highscoreTxt;
    [SerializeField] private FloatVariable highScore;

    [SyncVar (hook = "OnScoreChange")]
    private int score;

    private void Start()
    {
        highscoreTxt.text = highScore.Value.ToString();
    }

    public void UpdateScore()
    {
        score++;
        if (score > highScore.Value)
            highScore.SetValue((float)score);
    }

    public void UpdateScoreUI(int score)
    {
        scoreTxt.text = score.ToString();
        if (score > highScore.Value)
            highscoreTxt.text = score.ToString();
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
