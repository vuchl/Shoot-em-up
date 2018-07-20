using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoboRyanTron.Unite2017.Events;

public class EnemyShooting : NetworkBehaviour {

    [SerializeField] private Transform shotSpawnPos;
    [SerializeField] private float fireRate;

    public GameEvent EnemyProjectileFired;

    private ObjectPooler objectPooler;
    private float nextFire;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    private void Update()
    {
        if (isLocalPlayer)
            CheckForProjectile();
    }

    public void CheckForProjectile()
    {
        if (Time.time > nextFire)
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
        objectPooler.SpawnFromPool("EnemyProjectile", position, gameObject.transform.rotation);
        EnemyProjectileFired.Raise(gameObject);
    }

}
