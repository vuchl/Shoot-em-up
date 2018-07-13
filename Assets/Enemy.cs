using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPooledObject {

    [SerializeField]
    private Rigidbody enemyRigidBody;
    [SerializeField]
    private float speed;

    public void OnObjectSpawn()
    {
        enemyRigidBody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
    }

}
