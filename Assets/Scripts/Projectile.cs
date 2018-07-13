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

    private PlayerController projectileInstigator;
    private Collider instigatorCollider;

    void Start()
    {
    }

    public void InitProjectile(Vector3 position, PlayerController instigator)
    {
        transform.position = position;
        projectileRigidBody.velocity = transform.forward * speed;
        projectileInstigator = instigator;

        instigatorCollider = instigator.GetComponentInChildren<Collider>();
    }

    public void OnObjectSpawn()
    {
        projectileRigidBody.velocity = transform.forward * speed;
    }
}
