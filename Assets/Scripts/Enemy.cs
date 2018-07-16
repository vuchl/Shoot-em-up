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

    public void OnEnable()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        enemyRigidBody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (NetworkServer.active)
        {
            // Damage Player
            IDamageable damageable = other.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                print("Enemy attacks");
                damageable.Damage(damageAmount);
            }
        }
        
        print("enemy died");
        if(other.tag != "KillPlane")
            EnemyDied.Raise();
        // put enemy back into pool
        spawnManager.UnSpawnObject(gameObject);
        NetworkServer.UnSpawn(gameObject);
    }

}
