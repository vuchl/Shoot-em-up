using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{

    [SerializeField]
    private Rigidbody projectileRigidBody;
    [SerializeField]
    private float damageAmount;
    [SerializeField]
    private float speed;

    public void OnObjectSpawn()
    {
        projectileRigidBody.velocity = transform.forward * speed;
    }
}
