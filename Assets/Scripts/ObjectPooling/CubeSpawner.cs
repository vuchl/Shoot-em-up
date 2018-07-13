using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{

  //public GameObject cubePrefab;

  private bool isActive;
  private ObjectPooler objectPooler;

  private void Start() {
    objectPooler = ObjectPooler.Instance;
  }

  void FixedUpdate () {
    if(isActive)
      objectPooler.SpawnFromPool("Cube", transform.position, Quaternion.identity);
	  //Instantiate(cubePrefab, transform.position, Quaternion.identity);
	}

  private void Update() {
    if (Input.GetMouseButtonUp(0))
      isActive = !isActive;
  }
}
