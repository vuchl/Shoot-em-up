using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class GameManager : NetworkBehaviour {

    public TextMeshProUGUI score;

    public void GameOver()
    {
        RpcGameOver();
    }

    [ClientRpc]
    private void RpcGameOver()
    {
        score.SetText("GameOver");
    }
}
