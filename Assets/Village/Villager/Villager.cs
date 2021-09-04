using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Villager : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform goal;

    // Start is called before the first frame update
    void Start()
    {
        goal = null;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(goal != null) {
            if (goal.position != navMeshAgent.destination)
                SetDestination(goal);   
        }
    }

    public void Assign(Transform goal) {
        SetDestination(goal);
    }

    private void SetDestination(Transform destination) {
        navMeshAgent.destination = destination.position;
    }
}
