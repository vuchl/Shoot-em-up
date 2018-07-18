using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoboRyanTron.Unite2017.Events;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : NetworkBehaviour, IDamageable
{
    [Header("Movement")]
    public float speed;
    public float tilt;
    public Boundary boundary;

    [Header("Shooting")]
    public Projectile projectilePrefab;
    public Transform shotSpawnPos;
    public float fireRate;
    public SpawnManager bulletSpawnManager;

    [Header("Stats")]
    [SerializeField]
    private float maxHealth;

    private ObjectPooler objectPooler;
    private float nextFire;

    public GameEvent PlayerDied;

    [SyncVar]
    private float currenthealth;

    private void Start()
    {
        objectPooler = ObjectPooler.Instance;

        currenthealth = maxHealth;
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            Move();
        }
    }

 

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = movement * speed;

        GetComponent<Rigidbody>().position = new Vector3
        (
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
        );

        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }

    // called by enemies
    public void Damage(float damageAmount)
    {
        CmdDamage(damageAmount);
    }

    // damage command send to server
    [Command]
    private void CmdDamage(float damageAmount)
    {
        RpcDamage(damageAmount);
    }

    // damage command send back to all clients
    [ClientRpc]
    private void RpcDamage(float damageAmount)
    {
        print("player taking damage");
        print("damage amount: " + damageAmount);
        print("currrent health: " + currenthealth);
        currenthealth -= damageAmount;

        if (currenthealth <= 0.0f)
        {
            PlayerDied.Raise(gameObject);
            print("Player died");
            enabled = false;
            gameObject.SetActive(false);
        }
    }

}
