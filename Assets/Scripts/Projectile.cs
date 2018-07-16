using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class Projectile : NetworkBehaviour
{
    
    [SerializeField]
    private float damageAmount;
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
        Debug.LogError("Collision");
        gameObject.SetActive(false);
    }
}
