using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Villager : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform goal;
    public Transform homeLocation;

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

    private void DoTask() {
        
    }

    // Assign will have a task as well
    public void Assign(Transform goal/*, Task task*/) {
        SetDestination(goal);
    }

    private void SetDestination(Transform destination) {
        navMeshAgent.destination = destination.position;
    }

    public class Task {
        string name;
        float duration;
        Transform location;
        Villager worker;

        public Task(string name, float duration, Transform location, Villager worker) {
            this.name = name;
            this.duration = duration;
            this.location = location;
            this.worker = worker;
        }

        public virtual void DoTask() {
            worker.Assign(location);
        }
    }
}
