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
    public Transform shotSpawn;
    public float fireRate;

    private float nextFire;

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
            CmdRequestProjectile(shotSpawn.position, gameObject);
        }
    }


    [Command]
    private void CmdRequestProjectile(Vector3 position, GameObject obj)
    {
        //This runs on the server
        RpcSpawnProjectile(position);
    }

    [ClientRpc]
    private void RpcSpawnProjectile(Vector3 position)
    {
        Projectile newProjectile = Instantiate(projectilePrefab);
        newProjectile.InitProjectile(position, this);
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
