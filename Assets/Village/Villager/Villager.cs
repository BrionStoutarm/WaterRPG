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

        GridBuildingSystem.Instance.OnPlacedBuilding += HandlePlacedBuilding;
    }

    // Update is called once per frame
    void Update()
    {
        if(goal != null) {
            if (goal.position != navMeshAgent.destination)
                SetDestination(goal);   
        }
    }

    private void HandlePlacedBuilding(object sender, GridBuildingSystem.OnPlacedBuildingArgs args) {
        goal = args.placedObject.transform;
    }

    private void SetDestination(Transform destination) {
        navMeshAgent.destination = destination.position;
    }
}
