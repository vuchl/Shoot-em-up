using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoboRyanTron.Unite2017.Events;

public class Enemy : NetworkBehaviour, IKillable{

    [SerializeField]
    private Rigidbody enemyRigidBody;
    [SerializeField]
    private float speed;
    [SerializeField]
    private SpawnManager spawnManager;
    [SerializeField]
    private float damageAmount;
    [SerializeField]
    private GameEvent EnemyDied;

    private void OnTriggerEnter(Collider other)
    {
        // collision with KillPlane
        if (other.tag == "KillPlane")
            Kill();

        //// collision with Player
        //if(other.tag == "Player")
        //{
        //    EnemyDied.Raise(gameObject);
        //    Kill();
        //}
    }

    
    public void Kill()
    {
        // put enemy back into pool
        spawnManager.UnSpawnObject(gameObject);
        NetworkServer.UnSpawn(gameObject);
    }

    // destroy Enemy called by Health Script
    public void OnKilled()
    {
        EnemyDied.Raise(gameObject);
        Kill();
    }
}
