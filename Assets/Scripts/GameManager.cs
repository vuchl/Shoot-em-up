using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using RoboRyanTron.Unite2017.Events;
using RoboRyanTron.Unite2017.Variables;

public class GameManager : NetworkBehaviour {

    [Header("Events")]
    [SerializeField] private GameEvent GameOverEvent;

    [Header ("Enemy Exlosion Particles")]
    [SerializeField]
    private ObjectPooler objectPooler;

    [Header("Score")]
    [SerializeField] private FloatVariable highscore;

    private List<GameObject> activePlayers = new List<GameObject>();

    private GameObject explosionParticleSystem;

    // Spawn Particles for killed Enemies
    public void SpawnParticleExplosion(GameObject sender)
    {
        if(isServer)
            RpcSpawnParticleExplosion(sender.transform.position, sender.transform.rotation);
    }

    [ClientRpc]
    private void RpcSpawnParticleExplosion(Vector3 position, Quaternion rotation)
    {
        explosionParticleSystem = objectPooler.SpawnFromPool("EnemyExplosion", position, rotation);   // spawn Particles
        StartCoroutine(RpcDeactivateParticleExplosion(explosionParticleSystem, explosionParticleSystem.GetComponent<ParticleSystem>().main.duration)); // call function to put particles back into pool
    }

    // Put Particles back into Pool after delay
    IEnumerator RpcDeactivateParticleExplosion(GameObject particleSystem, float delay)
    {
        yield return new WaitForSeconds(delay);
        particleSystem.SetActive(false);
    }

    // add connected Player to List
    public void PlayerConnected(GameObject player)
    {
        activePlayers.Add(player);
    }

    public void PlayerDied(GameObject player)
    {
        RpcPlayerDied(player);
    }

    [ClientRpc]
    private void RpcPlayerDied(GameObject player)
    {
        if (activePlayers.Contains(player))
        {
            activePlayers.Remove(player);
        }

        if (activePlayers.Count == 0)
            GameOver();
    }

    private void GameOver()
    {
        GameOverEvent.Raise(gameObject);
    }
}
