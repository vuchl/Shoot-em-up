using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoboRyanTron.Unite2017.Events;

public class Enemy : NetworkBehaviour {

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

    private Health health;
    
    private void Start()
    {
        health = GetComponent<Health>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KillPlane")
            Kill();

        if(other.tag == "Player")
        {
            EnemyDied.Raise(gameObject);
            Kill();
        }

        if (health.currentHealth <= 0)
        {
            EnemyDied.Raise(gameObject);
            Kill();
        }
    }

    public void Kill()
    {
        // put enemy back into pool
        spawnManager.UnSpawnObject(gameObject);
        NetworkServer.UnSpawn(gameObject);
    }
}
