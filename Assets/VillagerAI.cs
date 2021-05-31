using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Move = System.Tuple<AITypes.LandManeuver, Villager.MovementSetting>;
public class VillagerAI : MonoBehaviour
{
    public Villager m_villager;
    public Vector3 m_target;
    public List<WaypointGraph.Waypoint> m_waypoints = new List<WaypointGraph.Waypoint>();
    public GameObject m_targetObject;

    public float m_gotoThreshold = 5.1f;
    public float m_waypointThreshold = 1f;
    public float m_collisionAvoidanceDistance = 5f;
    public float m_collisionAvoidanceWeight = 10f;

    public AITypes.Mode m_mode;
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
        if (m_villager)
        {
            switch (m_mode)
            {
                case (AITypes.Mode.RANDOM):
                    MoveRandomly();
                    break;
                case (AITypes.Mode.GO_TO):
                    MoveGoTo();
                    break;
                default:
                    Wait();
                    break;
            }
        }
    }
    public List<Move> GenerateMoves()
    {
        var ret = new List<Move>(); //may be more efficient to create the exhaustive list once and legalize it
        var curMov = m_villager.m_movementSetting;
        addAllManeuvers(ref ret, curMov);
        if (curMov > Villager.MovementSetting.STOP)
        {
            addAllManeuvers(ref ret, curMov - 1);
        }
        if (curMov < Villager.MovementSetting.WALK)
        {
            addAllManeuvers(ref ret, curMov + 1);
        }
        return ret;
    }
    public void addAllManeuvers(ref List<Move> moveList, Villager.MovementSetting movement)
    {
        for (AITypes.LandManeuver i = 0; i < AITypes.LandManeuver.MANUEVER_END; ++i)
        {
            moveList.Add(new Move(i, movement));
        }
    }

    public Villager.RotationSetting ManeuverToRotationSetting(AITypes.LandManeuver m)
    {
        switch (m)
        {
            case AITypes.LandManeuver.LEFT:
                {
                    return Villager.RotationSetting.LEFT;
                }
            case AITypes.LandManeuver.RIGHT:
                {
                    return Villager.RotationSetting.RIGHT;
                }
            default:
                return Villager.RotationSetting.FORWARD;
        }
    }
    public void ExecuteMove(Move toMove)
    {
        m_villager.m_rotationSetting = ManeuverToRotationSetting(toMove.Item1);
        m_villager.m_movementSetting = toMove.Item2;
    }

    private float GoToScore(Vector3 predictedLocation)
    {
        float score = (1 / (1 + Vector3.Distance(m_target, predictedLocation))) + CollisionDistanceScore(predictedLocation);
        return score;
    }

    private float WaypointScore(Vector3 predictedLocation)
    {
        float score = (1 / (1 + Vector3.Distance(m_waypoints[0].Position(), predictedLocation))) + CollisionDistanceScore(predictedLocation);
        return score;
    }

    public void MoveGoTo()
    {
        if (m_waypoints.Count > 0)
        {
            if (MoveWaypoint())
            {
                return;
            }
        }
        if (Vector3.Distance(m_target, m_villager.transform.position) < m_gotoThreshold)
        {
            m_mode = AITypes.Mode.WAIT;
            Wait();
            return;
        }
        var moves = GenerateMoves();
        LegalizeGoTo(ref moves);
        var moveAndScore = ChooseMove(moves, GoToScore);
        var move = moveAndScore.Item1;
        ExecuteMove(move);
    }

    public bool MoveWaypoint()
    {
        if (Vector3.Distance(m_waypoints[0].Position(), m_villager.transform.position) < m_waypointThreshold)
        {
            m_waypoints.RemoveAt(0);
        }
        if(m_waypoints.Count == 0)
        {
            return false;
        }
        var moves = GenerateMoves();
        LegalizeWaypoint(ref moves);
        var moveAndScore = ChooseMove(moves, WaypointScore);
        var move = moveAndScore.Item1;
        ExecuteMove(move);
        return true;
    }

    private bool isFullStop(Move m)
    {
        if (m.Item2 == Villager.MovementSetting.STOP)
        {
            return true;
        }
        return false;
    }

    public void LegalizeGoTo(ref List<Move> moveList)
    {
        if (Vector3.Distance(m_villager.transform.position, m_target) > m_gotoThreshold)
        {
            moveList.RemoveAll(isFullStop);
        }
    }

    public void LegalizeWaypoint(ref List<Move> moveList)
    {
        if (Vector3.Distance(m_villager.transform.position, m_waypoints[0].Position()) > m_waypointThreshold)
        {
            moveList.RemoveAll(isFullStop);
        }
    }

    public static void Shuffle<T>(ref List<T> a)
    {
        for (int i = 0; i < a.Count; i++)
        {
            var temp = a[i];
            int randomIndex = UnityEngine.Random.Range(i, a.Count);
            a[i] = a[randomIndex];
            a[randomIndex] = temp;
        }
    }

    public void MoveRandomly()
    {
        var moves = GenerateMoves();
        Shuffle(ref moves);
        var moveAndScore = ChooseMove(moves, CollisionDistanceScore);
        ExecuteMove(moveAndScore.Item1);
    }

    public void Wait()
    {
        ExecuteMove(new Move(AITypes.LandManeuver.NONE, Villager.MovementSetting.STOP));
    }

    public void GoTo(Vector3 v)
    {
        m_mode = AITypes.Mode.GO_TO;
        m_target = v;
        m_waypoints = m_gameManager.WaypointGraph().RequestWaypoints(m_villager.transform.position, m_target);
    }

    public float CollisionDistanceScore(Vector3 predictedLocation)
    {
        Collider[] hitColliders = Physics.OverlapSphere(predictedLocation, m_collisionAvoidanceDistance);
        float closestCollider = m_collisionAvoidanceDistance;
        foreach (Collider c in hitColliders)
        {
            if (c.transform.root != m_villager.transform && !c.CompareTag("Ground"))
            {
                Vector3 closest = c.ClosestPoint(predictedLocation);
                float distanceToClosest = Vector3.Distance(closest, predictedLocation);
                if (distanceToClosest < closestCollider)
                {
                    closestCollider = distanceToClosest;
                }
            }
        }
        return m_collisionAvoidanceWeight * (closestCollider / m_collisionAvoidanceDistance);
    }

    public Tuple<Move, float> ChooseMove(List<Move> moveList, Func<Vector3, float> scoreMove)
    {
        float bestScore = 0;
        Move bestMove = moveList[0];
        foreach (Move m in moveList)
        {
            float score = scoreMove(PredictLocation(m));
            if (score > bestScore)
            {
                bestMove = m;
                bestScore = score;
            }
        }
        return new Tuple<Move, float>(bestMove, bestScore);
    }

    public Vector3 PredictLocation(Move m)
    {
        return m_villager.LiveMovementPosition(ManeuverToRotationSetting(m.Item1), m.Item2);
    }
}
