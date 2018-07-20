using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using RoboRyanTron.Unite2017.Events;

public class GameManager : NetworkBehaviour {

    [Header("Events")]
    [SerializeField] private GameEvent GameOverEvent;

    private List<GameObject> activePlayers = new List<GameObject>();

    [Header ("Enemy Exlosion Particles")]
    [SerializeField]
    private ObjectPooler objectPooler;

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

    public void PlayerConnected(GameObject player)
    {
        activePlayers.Add(player);
    }

    public void PlayerDied(GameObject player)
    {
        print("Player " + player.name + "died");
        if(activePlayers.Contains(player))
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
