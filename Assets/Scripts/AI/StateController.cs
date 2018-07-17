using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class StateController : NetworkBehaviour {

    public State defaultState;
    public State currentState;
    public State remainState;
    public Transform eyes;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    public Transform chaseTarget;

    private bool aiActive = true;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        currentState = defaultState;
    }

    public void SetupAI(bool aiActivationFromEnemyManager)
    {
        aiActive = aiActivationFromEnemyManager;
        if (aiActive)
        {
            navMeshAgent.enabled = true;
        }
        else
        {
            navMeshAgent.enabled = false;
        }
    }

    void Update()
    {
        if (!aiActive)
            return;
        currentState.UpdateState(this);
    }

    void OnDrawGizmos()
    {
        if (currentState != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(transform.position, 2);
        }
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }

    private void OnExitState()
    {

    }

    
}
