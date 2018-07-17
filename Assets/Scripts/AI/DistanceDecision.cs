using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Distance")]
public class DistanceDecision : Decision {

    public float distanceThreshold;
    public float lookRange = 5.0f;
    public float lookSphereCastRadius = 5.0f;
    
    public override bool Decide(StateController controller)
    {
        return Look(controller);
    }

    //private bool Distance(StateController controller)
    //{
    //    if (Vector3.Magnitude(controller.transform.position - controller.chaseTarget.position) <= distanceThreshold)
    //        return true;
    //    else
    //        return false;
    //}

    private bool Look(StateController controller)
    {
        RaycastHit hit;

        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * lookRange, Color.green);

        if (Physics.SphereCast(controller.eyes.position, lookSphereCastRadius, controller.eyes.forward, out hit, lookRange)
            && hit.collider.CompareTag("Player"))
        {
            controller.chaseTarget = hit.transform;
            return true;
        }
        else
        {
            return false;
        }
    }

}
