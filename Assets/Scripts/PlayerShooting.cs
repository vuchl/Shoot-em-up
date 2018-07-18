using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShooting : NetworkBehaviour
{

    [SerializeField] private Transform shotSpawnPos;
    [SerializeField] private float fireRate;
    [SerializeField] private SpawnManager projectileSpawnManager;

    private float nextFire;

    private void Update()
    {
        CheckForProjectile();
    }

    private void CheckForProjectile()
    {
        if (!isLocalPlayer)
            return;
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            //This runs on the local authority client
            CmdRequestProjectile(shotSpawnPos.position);
        }
    }

    [Command]
    private void CmdRequestProjectile(Vector3 spawnPosition)
    {
        var projectile = projectileSpawnManager.GetFromPool(spawnPosition);

        // spawn enemy on client, custom spawn handler is called
        NetworkServer.Spawn(projectile, projectileSpawnManager.assetId);
    }

    [Command]
    public void CmdUnspawnProjectile(GameObject projectile)
    {
        // put enemy back into pool
        projectileSpawnManager.UnSpawnObject(projectile);
        NetworkServer.UnSpawn(projectile);
    }

}
