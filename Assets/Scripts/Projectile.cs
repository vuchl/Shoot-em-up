using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoboRyanTron.Unite2017.Events;

public class Projectile : NetworkBehaviour
{
    [SerializeField]
    private float speed;
    
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
        gameObject.SetActive(false);
    }
}
