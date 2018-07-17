using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.Unite2017.Variables;

public class TakeDamage : MonoBehaviour {

    public FloatReference damageAmount;

    private void OnTriggerEnter(Collider collision)
    {
        var hit = collision.gameObject;
        var health = hit.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damageAmount.Value);
        }
    }
}
