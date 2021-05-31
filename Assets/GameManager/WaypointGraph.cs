using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaypointGraph : MonoBehaviour
{
    public class Waypoint
    {
        public GameObject m_obj;
        public List<WaypointEdge> m_edges = new List<WaypointEdge>();
        public WaypointGraph m_graph;
        public Waypoint(GameObject obj, WaypointGraph graph)
        {
            m_obj = obj;
            m_graph = graph;
        }

        public void DisconnectAll()
        {
            while (m_edges.Count > 0)
            {
                m_graph.RemoveEdge(m_edges[0]);
            }
        }

        public WaypointEdge EdgeTo(Waypoint w)
        {
            foreach(WaypointEdge e in m_edges)
            {
                if (e.Contains(w))
                {
                    return e;
                }
            }
            return null;
        }
        public bool IsConnectedTo(Waypoint w)
        {
            return EdgeTo(w) != null;
        }

        public void AddEdge(WaypointEdge e)
        {
            m_edges.Add(e);
        }

        public void RemoveEdge(WaypointEdge e)
        {
            m_edges.Remove(e);
        }

        public int NumEdges()
        {
            return m_edges.Count;
        }
    }

    public class WaypointEdge
    {
        public Waypoint m_a;
        public Waypoint m_b;

        public WaypointEdge(Waypoint a, Waypoint b)
        {
            m_a = a;
            m_b = b;
        }
        public bool Contains(Waypoint a)
        {
            if(m_a == a)
            {
                return true;
            }
            if(m_b == a)
            {
                return true;
            }
            return false;
        }
        public void Disconnect()
        {
            m_a.RemoveEdge(this);
            m_b.RemoveEdge(this);
        }
    }
    public GameObject m_waypointPrefab;
    public float m_waypointMergeDistance = 2f;
    private List<Waypoint> m_waypoints = new List<Waypoint>();
    private List<WaypointEdge> m_waypointEdges = new List<WaypointEdge>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = new Vector3(0, 2, 0);
        foreach (WaypointEdge e in m_waypointEdges)
        {
            Debug.DrawLine(e.m_a.m_obj.transform.position, e.m_b.m_obj.transform.position + offset, Color.blue, 100f);
        }
    }
    Waypoint ClosestWaypoint(Vector3 pos)
    {
        Waypoint ret = null;
        float minDistance = 0f;
        foreach(var w in m_waypoints)
        {
            float distance = Vector3.Distance(w.m_obj.transform.position, pos);
            if(ret == null || distance < minDistance)
            {
                ret = w;
                minDistance = distance;
            }
        }
        return ret;
    }

    public void AddEdge(Waypoint a, Waypoint b)
    {
        if (a != b && !a.IsConnectedTo(b))
        {
            WaypointEdge e = new WaypointEdge(a, b);
            a.AddEdge(e);
            b.AddEdge(e);
            m_waypointEdges.Add(e);
        }
    }

    void CleanNode(Waypoint w)
    {
        if (w.NumEdges() == 0)
        {
            RemoveWaypoint(w);
        }
    }

    void RemoveWaypoint(Waypoint w)
    {
        w.DisconnectAll();
        m_waypoints.Remove(w);
    }

    void RemoveEdge(WaypointEdge e)
    {
        e.Disconnect();
        m_waypointEdges.Remove(e);
        CleanNode(e.m_a);
        CleanNode(e.m_b);
    }

    public Waypoint AddWaypoint(Vector3 toAdd)
    {
        Waypoint closest = ClosestWaypoint(toAdd);
        if(closest != null)
        {
            if(Vector3.Distance(closest.m_obj.transform.position, toAdd) < m_waypointMergeDistance)
            {
                return closest;
            }
        }
        Waypoint ret = new Waypoint(Instantiate(m_waypointPrefab, toAdd, Quaternion.identity), this);
        m_waypoints.Add(ret);
        return ret;
    }
}
