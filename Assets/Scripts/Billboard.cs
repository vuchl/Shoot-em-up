using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
    void Update()
    {
        //transform.LookAt(Camera.main.transform);

        Vector3 v = Camera.main.transform.position - transform.position;

        v.x = v.z = 0.0f;

        transform.LookAt(Camera.main.transform.position - v);

        transform.rotation = (Camera.main.transform.rotation); // Take care about camera rotation
    }
}
