using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class VillagerAI : MonoBehaviour
{
    public NavMeshAgent m_navAgent;
    public Vector3 m_target;
    public List<WaypointGraph.Waypoint> m_waypoints = new List<WaypointGraph.Waypoint>();
    public GameObject m_targetObject;
    public AITypes.Mode m_mode;

    public float m_waypointThreshold = 1f;

    private GameManager m_gameManager;
    // Start is called before the first frame update
    void Start()
    {
        m_gameManager = GameManager.Get();
        m_mode = AITypes.Mode.WAIT;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_navAgent)
        {
            switch (m_mode)
            {
                case AITypes.Mode.GO_TO:
                    MoveGoTo();
                    break;
                case AITypes.Mode.RANDOM:
                    MoveRandomly();
                    break;
                case AITypes.Mode.FOLLOW:
                    MoveFollow();
                    break;
                default:
                    Stop();
                    break;
            }
        }
        
    }

    void Stop()
    {
        m_navAgent.destination = m_navAgent.transform.position;
    }

    void MoveFollow()
    {
        m_target = m_targetObject.transform.position;
        m_waypoints = m_gameManager.WaypointGraph().RequestWaypoints(m_navAgent.transform.position, m_target);
        MoveGoTo();
    }
    void MoveGoTo()
    {
        if (m_waypoints.Count > 0)
        {
            Vector3 normalizedWaypoint = m_waypoints[0].Position();
            normalizedWaypoint.y = m_navAgent.transform.position.y;
            if (Vector3.Distance(normalizedWaypoint, m_navAgent.transform.position) < m_waypointThreshold)
            {
                m_waypoints.RemoveAt(0);
            }
        }
        if (m_waypoints.Count > 0)
        {
            m_navAgent.destination = m_waypoints[0].Position();
        }
        else
        {
            m_navAgent.destination = m_target;
        }
    }

    void MoveRandomly()
    {

    }

    public void GoTo(Vector3 v)
    {
        m_mode = AITypes.Mode.GO_TO;
        m_target = v;
        m_waypoints = m_gameManager.WaypointGraph().RequestWaypoints(m_navAgent.transform.position, m_target);
    }
}
