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
}
