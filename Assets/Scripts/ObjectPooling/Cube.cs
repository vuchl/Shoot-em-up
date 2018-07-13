using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour, IPooledObject
{

  public float upForce = 1.0f;
  public float sideForce = .1f;

  public void OnObjectSpawn () {
	  float xforce = Random.Range(-sideForce, sideForce);
	  float yforce = Random.Range(upForce/2f, upForce);
	  float zforce = Random.Range(-sideForce, sideForce);
	  
	  Vector3 force = new Vector3(xforce, yforce, zforce);

	  GetComponent<Rigidbody>().velocity = force;

	}

}
