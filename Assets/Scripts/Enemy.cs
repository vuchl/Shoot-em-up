using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Enemy : NetworkBehaviour {

    [SerializeField]
    private Rigidbody enemyRigidBody;
    [SerializeField]
    private float speed;
    [SerializeField]
    private SpawnManager spawnManager;
    [SerializeField]
    private float damageAmount;


    public void OnEnable()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        enemyRigidBody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (NetworkServer.active)
        {
            IDamageable damageable = other.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                print("Enemy attacks");
                damageable.Damage(damageAmount);
            }
        }

        // put enemy back into pool
        spawnManager.UnSpawnObject(gameObject);
        NetworkServer.UnSpawn(gameObject);
    }

}
