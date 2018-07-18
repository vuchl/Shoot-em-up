using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {
    
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public SpawnManager spawnManager;
    

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }
    
    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                CmdSpawn(spawnPosition);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
        }
    }

    [Command]
    void CmdSpawn(Vector3 spawnPosition)
    {
        // Set up enemy on server
        var enemy = spawnManager.GetFromPool(spawnPosition);
        enemy.transform.eulerAngles = new Vector3(0, 180, 0);

        // spawn enemy on client, custom spawn handler is called
        NetworkServer.Spawn(enemy, spawnManager.assetId);
        
    }
}
