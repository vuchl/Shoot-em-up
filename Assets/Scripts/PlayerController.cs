using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : NetworkBehaviour
{
    [Header("Movement")]
    public float speed;
    public float tilt;
    public Boundary boundary;

    [Header("Shooting")]
    public Projectile projectilePrefab;
    public Transform shotSpawnPos;
    public float fireRate;

    private ObjectPooler objectPooler;
    private float nextFire;


    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            CheckForProjectile();

            
        }
    }

    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            Move();
        }
    }

    private void CheckForProjectile()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            //This runs on the local authority client
            CmdRequestProjectile("Projectile", shotSpawnPos.position, Quaternion.identity);
        }
    }


    [Command]
    private void CmdRequestProjectile(string tag, Vector3 position, Quaternion rotation)
    {
        //This runs on the server
        RpcSpawnProjectile(tag, position, rotation);
    }

    [ClientRpc]
    private void RpcSpawnProjectile(string tag, Vector3 position, Quaternion rotation)
    {
        objectPooler.SpawnFromPool(tag, position, rotation);
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
}
