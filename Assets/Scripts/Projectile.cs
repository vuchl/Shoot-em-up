using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField]
    private Rigidbody projectileRigidBody;
    [SerializeField]
    private float damageAmount;
    [SerializeField]
    private float speed;

    //public void OnObjectSpawn()
    //{
    //    projectileRigidBody.velocity = transform.forward * speed;
    //}

    private void OnEnable()
    {
        projectileRigidBody.velocity = transform.forward * speed;
    }
}
