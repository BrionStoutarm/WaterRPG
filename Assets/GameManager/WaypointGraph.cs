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
        public int m_index = -1;
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

        public Vector3 Position()
        {
            return m_obj.transform.position;
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

        class DjikstraQueue
        {
            LinkedList<Tuple<float, Waypoint>> m_queue = new LinkedList<Tuple<float, Waypoint>>();
            public void Push(float f, Waypoint w) //this can be more efficient if neccessary
            {
                var listNode = m_queue.First;
                while (listNode != null)
                {
                    if(f < listNode.Value.Item1)
                    {
                        m_queue.AddBefore(listNode, new LinkedListNode<Tuple<float, Waypoint>>(new Tuple<float, Waypoint>(f, w)));
                        return;
                    }
                    listNode = listNode.Next;
                }
                m_queue.AddLast(new LinkedListNode<Tuple<float, Waypoint>>(new Tuple<float, Waypoint>(f, w)));
            }
            public Waypoint Pop()
            {
                Waypoint ret = null;
                if(m_queue.First != null)
                {
                    ret = m_queue.First.Value.Item2;
                    m_queue.RemoveFirst();
                }
                return ret;
            }

            public bool Empty()
            {
                return m_queue.First == null;
            }
        }

        public bool FindPath(Waypoint w, ref List<Waypoint> path)
        {
            List<bool> visited = new List<bool>(m_graph.Size());
            List<float> distance = new List<float>(m_graph.Size());
            for(int i = 0; i < m_graph.Size(); ++i)
            {
                visited.Add(false);
                distance.Add(-1);
                if(m_graph.Waypoints()[i] == this)
                {
                    distance[i] = 0;
                }
            }
            DjikstraQueue queue = new DjikstraQueue();
            queue.Push(0, this);
            while (!queue.Empty())
            {
                Waypoint curPoint = queue.Pop();
                if (visited[curPoint.m_index])
                {
                    continue;
                }
                float curDistance = distance[curPoint.m_index];
                if(curPoint == w)
                {
                    Waypoint traceback = w;
                    while (traceback != null)
                    {
                        path.Add(traceback);
                        if(traceback == this)
                        {
                            break;
                        }
                        float minDistance = 0;
                        Waypoint nextNode = null;
                        foreach(var edge in traceback.m_edges)
                        {
                            Waypoint n = edge.Other(traceback);
                            if (!visited[n.m_index])
                            {
                                continue;
                            }
                            if(nextNode == null || (distance[n.m_index] < minDistance))
                            {
                                minDistance = distance[n.m_index];
                                nextNode = n;
                            }
                        }
                        traceback = nextNode;
                    }
                    path.Reverse();
                    return true;
                }
                foreach(var edge in curPoint.m_edges)
                {
                    Waypoint next = edge.Other(curPoint);
                    float nextDistance = curDistance + Vector3.Distance(curPoint.Position(), next.Position());
                    if(distance[next.m_index] == - 1 || nextDistance < distance[next.m_index])
                    {
                        distance[next.m_index] = nextDistance;
                        queue.Push(nextDistance, next);
                    }
                }
                visited[curPoint.m_index] = true;
            }

            return false;
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

        public Waypoint Other(Waypoint a)
        {
            if(m_a == a)
            {
                return m_b;
            }
            return m_a;
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
        m_waypoints[w.m_index] = m_waypoints[m_waypoints.Count - 1];
        m_waypoints[w.m_index].m_index = w.m_index;
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
        ret.m_index = m_waypoints.Count;
        m_waypoints.Add(ret);
        return ret;
    }

    public List<Waypoint> RequestWaypoints(Vector3 begin, Vector3 end)
    {
        List<Waypoint> ret = new List<Waypoint>();
        if(m_waypoints.Count == 0)
        {
            return ret;
        }
        float baseDistance = Vector3.Distance(begin, end);
        Waypoint closestToBegin = ClosestWaypoint(begin);
        Waypoint closestToEnd = ClosestWaypoint(end);
        if (closestToBegin.FindPath(closestToEnd, ref ret))
        {
            return ret;
        }
        return ret;
    }

    public int Size()
    {
        return m_waypoints.Count;
    }

    public List<Waypoint> Waypoints()
    {
        return m_waypoints;
    }
}
