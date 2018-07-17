using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class GameManager : NetworkBehaviour {

    [Header ("Enemy Exlosion Particles")]
    [SerializeField]
    private ObjectPooler objectPooler;

    private GameObject explosionParticleSystem;

    // Spawn Particles for killed Enemies
    [ClientRpc]
    public void RpcSpawnParticleExplosion(GameObject sender)
    {
        explosionParticleSystem = objectPooler.SpawnFromPool("EnemyExplosion", sender.transform.position, sender.transform.rotation);   // spawn Particles
        StartCoroutine(RpcDeactivateParticleExplosion(explosionParticleSystem, explosionParticleSystem.GetComponent<ParticleSystem>().main.duration)); // call function to put particles back into pool
    }

    // Put Particles back into Pool after delay
    IEnumerator RpcDeactivateParticleExplosion(GameObject particleSystem, float delay)
    {
        yield return new WaitForSeconds(delay);
        particleSystem.SetActive(false);
    }



    //public TextMeshProUGUI score;

    //public void GameOver()
    //{
    //    RpcGameOver();
    //}

    //[ClientRpc]
    //private void RpcGameOver()
    //{
    //    score.SetText("GameOver");
    //}
}
