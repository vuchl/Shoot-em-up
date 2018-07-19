using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShooting : NetworkBehaviour
{

    [SerializeField] private Transform shotSpawnPos;
    [SerializeField] private float fireRate;

    private ObjectPooler objectPooler;
    private float nextFire;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    private void Update()
    {
        if(isLocalPlayer)
            CheckForProjectile();
    }

    private void CheckForProjectile()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            //This runs on the local authority client
            CmdRequestProjectile(shotSpawnPos.position);
        }
    }


    [Command]
    private void CmdRequestProjectile(Vector3 position)
    {
        //This runs on the server
        RpcSpawnProjectile(position);
    }

    [ClientRpc]
    private void RpcSpawnProjectile(Vector3 position)
    {
        objectPooler.SpawnFromPool("Projectile", position, Quaternion.identity);
    }

    //private void CheckForProjectile()
    //{
    //    if (!isLocalPlayer)
    //        return;
    //    if (Input.GetButton("Fire1") && Time.time > nextFire)
    //    {
    //        nextFire = Time.time + fireRate;
    //        //This runs on the local authority client
    //        CmdRequestProjectile(shotSpawnPos.position);
    //    }
    //}

    //[Command]
    //private void CmdRequestProjectile(Vector3 spawnPosition)
    //{
    //    var projectile = projectileSpawnManager.GetFromPool(spawnPosition);

    //    // spawn enemy on client, custom spawn handler is called
    //    NetworkServer.Spawn(projectile, projectileSpawnManager.assetId);
    //}

    //[Command]
    //public void CmdUnspawnProjectile(GameObject projectile)
    //{
    //    // put enemy back into pool
    //    projectileSpawnManager.UnSpawnObject(projectile);
    //    NetworkServer.UnSpawn(projectile);
    //}

}
