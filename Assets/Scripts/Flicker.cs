using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour {

    [SerializeField] private float frequency;
    [SerializeField] private float strength;

    private void Update()
    {
        float colorValue = (Mathf.Sin(Time.time * frequency))*strength+0.5f;
        
        GetComponent<MeshRenderer>().material.SetColor("_TintColor", new Color(colorValue, colorValue, colorValue));
    }

}
