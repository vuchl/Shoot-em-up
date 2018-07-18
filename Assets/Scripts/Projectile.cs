using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoboRyanTron.Unite2017.Events;

public class Projectile : NetworkBehaviour
{
    
    [SerializeField]
    private float damageAmount;
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameEvent ProjectileExploded;

    private Rigidbody projectileRigidBody;

    private void Awake()
    {
        projectileRigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        
        projectileRigidBody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        ProjectileExploded.Raise(gameObject);
    }
}
