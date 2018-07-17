using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildNavMeshOnStart : MonoBehaviour {

	
	void Awake ()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();
	}
}
