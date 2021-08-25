//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Move = System.Tuple<AITypes.Maneuver, BoatMovement.MovementSetting>;


//public class AIBoatControl : MonoBehaviour
//{
    

//    public Vector3 m_target;
//    public GameObject m_targetObject;
//    public float m_gotoThreshold = 2f;
//    public float m_tackTargetAngle = 0;
//    public float m_tackAngleThreshold = 5f; //acceptable angle within target
//    public float m_tackAngle = 90f;
//    public float m_tackThreshold = 1.00001f;
//    public float m_followDistance = 50f;
//    public float m_followThreshold = 2f;
//    public float m_collisionAvoidanceDistance = 30f;
//    public float m_collisionAvoidanceWeight = 10f;
//    public BoatMovement m_boat;
//    private GameManager m_gameManager;


//    public AITypes.Mode m_mode;

    
//    public AITypes.Maneuver m_maneuver;


//    // Start is called before the first frame update
//    void Start()
//    {
//        m_gameManager = GameManager.Get();
//        m_mode = AITypes.Mode.GO_TO;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (m_gameManager.Paused())
//        {
//            return; //eventually need to make turn based decisions, but do this on demand not in update
//        }
//        if (m_boat)
//        {
//            switch (m_mode)
//            {
//                case (AITypes.Mode.RANDOM):
//                    MoveRandomly();
//                    break;
//                case (AITypes.Mode.MAX_SPEED):
//                    MoveMaxSpeed();
//                    break;
//                case (AITypes.Mode.GO_TO):
//                    MoveGoTo();
//                    break;
//                case (AITypes.Mode.FOLLOW):
//                    MoveFollow();
//                    break;
//                default:
//                    FullStop();
//                    break;
//            }
//        }
//    }
//    public void SetBoat(BoatMovement boat)
//    {
//        m_boat = boat;
//    }

//    public void SetTarget(Vector3 target)
//    {
//        m_target = target;
//        m_mode = AITypes.Mode.GO_TO;
//    }

//    public void FullStop()
//    {
//        ExecuteMove(new Move(AITypes.Maneuver.NONE, BoatMovement.MovementSetting.FULL_STOP));
//    }

//    public void MoveRandomly()
//    {
//        float turnRand = UnityEngine.Random.value;
//        if (turnRand > .9)
//        {
//            if (turnRand > .96)
//            {
//                m_boat.LiveRotationSetting(BoatMovement.RotationSetting.FORWARD);
//            }
//            else if (turnRand > .93)
//            {
//                m_boat.LiveRotationSetting(BoatMovement.RotationSetting.LEFT);
//            }
//            else
//            {
//                m_boat.LiveRotationSetting(BoatMovement.RotationSetting.RIGHT);
//            }
//        }
//        float speedRand = UnityEngine.Random.value;
//        if (speedRand > .95)
//        {
//            if (speedRand > .985)
//            {
//                m_boat.DecreaseMovementSetting();
//            }
//            else
//            {
//                m_boat.IncreaseMovementSetting();
//            }
//        }
//    }

//    public void addAllManeuvers(ref List<Move> moveList, BoatMovement.MovementSetting movement)
//    {
//        for(AITypes.Maneuver i = 0; i < AITypes.Maneuver.MANUEVER_END; ++i)
//        {
//            moveList.Add(new Move(i, movement));
//        }
//    }

//    public List<Move> GenerateMoves()
//    {
//        var ret = new List<Move>(); //may be more efficient to create the exhaustive list once and legalize it
//        var curMov = m_boat.m_movementSetting;
//        addAllManeuvers(ref ret, curMov);
//        if (curMov > BoatMovement.MovementSetting.FULL_STOP) //Exclude oarsback to avoid cases where the AI won't turn
//        {
//            addAllManeuvers(ref ret, curMov - 1);
//        }
//        if (curMov < (BoatMovement.MovementSetting.MOVEMENT_COUNT - 1))
//        {
//            addAllManeuvers(ref ret, curMov + 1);
//        }
//        return ret;
//    }

//    public Vector3 PredictLocation(Move m)
//    {
//        switch (m.Item1)
//        {
//            case AITypes.Maneuver.TACK_LEFT:
//            case AITypes.Maneuver.TACK_RIGHT:
//                return m_boat.LivePredictTackPosition(ManeuverToRotationSetting(m.Item1), m.Item2, m_tackAngle);
//            case AITypes.Maneuver.TACK_HALF_LEFT:
//            case AITypes.Maneuver.TACK_HALF_RIGHT:
//                return m_boat.LivePredictTackPosition(ManeuverToRotationSetting(m.Item1), m.Item2, (m_tackAngle / 2));
//        }
//        return m_boat.LivePredictLocation(ManeuverToRotationSetting(m.Item1), m.Item2);
//    }

//    public float CollisionDistanceScore(Vector3 predictedLocation)
//    {
//        Collider[] hitColliders = Physics.OverlapSphere(predictedLocation, m_collisionAvoidanceDistance);
//        float closestCollider = m_collisionAvoidanceDistance;
//        foreach(Collider c in hitColliders)
//        {
//            if (c.transform.root != m_boat.transform) {
//                Vector3 closest = c.ClosestPoint(predictedLocation);
//                float distanceToClosest = Vector3.Distance(closest, predictedLocation);
//                if (distanceToClosest < closestCollider)
//                {
//                    closestCollider = distanceToClosest;
//                }
//            }
//        }
//        return m_collisionAvoidanceWeight * (closestCollider / m_collisionAvoidanceDistance);
//    }

//    public float MaxSpeedScore(Vector3 predictedLocation)
//    {
//        float score = 1/ (1 + (m_gameManager.Weather().WindSpeed() - Vector3.Distance(m_boat.transform.position, predictedLocation))) + CollisionDistanceScore(predictedLocation);
//        //Debug.Log(string.Format("{0} {1} {2}", BoatMovement.RotationSettingToString(m.Item1), BoatMovement.MovementSettingToString(m.Item2), score));
//        return score;
//    }
//    public void MoveMaxSpeed()
//    {
//        var moves = GenerateMoves();
//        var moveAndScore = ChooseMove(moves, MaxSpeedScore);
//        ExecuteMove(moveAndScore.Item1);
//    }

//    private float GoToScore(Vector3 predictedLocation)
//    {
//        float score = (1 / (1 + Vector3.Distance(m_target, predictedLocation))) + CollisionDistanceScore(predictedLocation);
//        //Debug.Log(string.Format("{0} {1} {2} {3}", BoatMovement.RotationSettingToString(m.Item1), BoatMovement.MovementSettingToString(m.Item2), Vector3.Distance(m_target, predictedLocation), score));
//        return score;
//    }

//    private float FollowScore(Vector3 predictedLocation)
//    {
//        float score = (1 / (1 + FollowDistanceDistance(predictedLocation))) + CollisionDistanceScore(predictedLocation);
//        return score;
//    }
//    private float FollowDistanceDistance(Vector3 toMeasure) //Fully aware this name is bad, if you can come up with a better one let me know. It's the distance from the target distance Ex actual_dist = 5, target_dist = 10, distance_dist = 5
//    {
//        return Math.Abs(m_followDistance - Vector3.Distance(m_targetObject.transform.position, toMeasure));
//    }

//    public void MoveFollow()
//    {
//        if (!m_targetObject)
//        {
//            FullStop();
//            return;
//        }
//        var moves = GenerateMoves();
//        LegalizeFollow(ref moves);
//        LegalizeManeuver(ref moves);
//        var moveAndScore = ChooseMove(moves, FollowScore);
//        var move = moveAndScore.Item1;
//        ExecuteMove(move);
//    }

//    public void MoveGoTo()
//    {
//        if(Vector3.Distance(m_target, m_boat.transform.position) < m_gotoThreshold){
//            m_mode = AITypes.Mode.WAIT;
//            FullStop();
//            return;
//        }
//        var moves = GenerateMoves();
//        LegalizeGoTo(ref moves);
//        LegalizeManeuver(ref moves);
//        var moveAndScore = ChooseMove(moves, GoToScore);
//        var move = moveAndScore.Item1;
//        ExecuteMove(move);
//    }

//    private bool isFullStop(Move m)
//    {
//        if(m.Item2 == BoatMovement.MovementSetting.FULL_STOP)
//        {
//            return true;
//        }
//        return false;
//    }

//    private bool isNotLeft(Move m)
//    {
//        if(m.Item1 != AITypes.Maneuver.LEFT)
//        {
//            return true;
//        }
//        return false;
//    }

//    private bool isNotRight(Move m)
//    {
//        if (m.Item1 != AITypes.Maneuver.RIGHT)
//        {
//            return true;
//        }
//        return false;
//    }

//    public void LegalizeGoTo(ref List<Move> moveList)
//    {
//        if (Vector3.Distance(m_boat.transform.position, m_target) > m_gotoThreshold)
//        {
//            moveList.RemoveAll(isFullStop);
//        }
//    }

//    public void LegalizeFollow(ref List<Move> moveList)
//    {
//        if (FollowDistanceDistance(m_boat.transform.position) > m_followThreshold)
//        {
//            moveList.RemoveAll(isFullStop);
//        }
//    }

//    public bool IsTack(AITypes.Maneuver m)
//    {
//        switch (m)
//        {
//            case AITypes.Maneuver.TACK_HALF_LEFT:
//            case AITypes.Maneuver.TACK_LEFT:
//            case AITypes.Maneuver.TACK_HALF_RIGHT:
//            case AITypes.Maneuver.TACK_RIGHT:
//                return true;
//            default:
//                return false;

//        }
//    }

//    public bool IsLeftTack(AITypes.Maneuver m)
//    {
//        switch (m)
//        {
//            case AITypes.Maneuver.TACK_HALF_LEFT:
//            case AITypes.Maneuver.TACK_LEFT:
//                return true;
//            default:
//                return false;

//        }
//    }

//    public bool IsRightTack(AITypes.Maneuver m)
//    {
//        switch (m)
//        {
//            case AITypes.Maneuver.TACK_HALF_RIGHT:
//            case AITypes.Maneuver.TACK_RIGHT:
//                return true;
//            default:
//                return false;

//        }
//    }

//    public void LegalizeManeuver(ref List<Move> moveList)
//    {
//        if (m_maneuver != AITypes.Maneuver.NONE)
//        {
//            if (IsTack(m_maneuver))
//            {
//                if (m_boat.NormalizeAngle(m_boat.NormalizedAngle() - m_tackTargetAngle) < m_tackAngleThreshold)
//                {
//                    m_maneuver = AITypes.Maneuver.NONE;
//                }
//            }
//            if (IsLeftTack(m_maneuver))
//            {
//                moveList.RemoveAll(isNotLeft);
//            }
//            else if (IsRightTack(m_maneuver))
//            {
//                moveList.RemoveAll(isNotRight);
//            }
//        }
//    }

//    public BoatMovement.RotationSetting ManeuverToRotationSetting(AITypes.Maneuver m)
//    {
//        switch (m)
//        {
//            case AITypes.Maneuver.TACK_LEFT:
//            case AITypes.Maneuver.TACK_HALF_LEFT:
//            case AITypes.Maneuver.LEFT:
//            {
//                return BoatMovement.RotationSetting.LEFT;
//            }
//            case AITypes.Maneuver.TACK_RIGHT:
//            case AITypes.Maneuver.TACK_HALF_RIGHT:
//            case AITypes.Maneuver.RIGHT:
//            {
//                return BoatMovement.RotationSetting.RIGHT;
//            }
//            default:
//                return BoatMovement.RotationSetting.FORWARD;
//        }
//    }
//    public void ExecuteMove(Move toMove)
//    {
//        switch (toMove.Item1)
//        {
//            case AITypes.Maneuver.TACK_LEFT:
//                m_maneuver = AITypes.Maneuver.TACK_LEFT;
//                m_tackTargetAngle = m_boat.NormalizeAngle(m_boat.NormalizedAngle() - m_tackAngle);
//                break;
//            case AITypes.Maneuver.TACK_HALF_LEFT:
//                m_maneuver = AITypes.Maneuver.TACK_LEFT;
//                m_tackTargetAngle = m_boat.NormalizeAngle(m_boat.NormalizedAngle() - (m_tackAngle / 2));
//                break;
//            case AITypes.Maneuver.TACK_RIGHT:
//                m_maneuver = AITypes.Maneuver.TACK_RIGHT;
//                m_tackTargetAngle = m_boat.NormalizeAngle(m_boat.NormalizedAngle() + m_tackAngle);
//                break;
//            case AITypes.Maneuver.TACK_HALF_RIGHT:
//                m_maneuver = AITypes.Maneuver.TACK_RIGHT;
//                m_tackTargetAngle = m_boat.NormalizeAngle(m_boat.NormalizedAngle() + (m_tackAngle / 2));
//                break;
//            default:
//                break;
//        }
//        m_boat.m_rotationSetting = ManeuverToRotationSetting(toMove.Item1);
//        m_boat.m_movementSetting = toMove.Item2;
//    }

//    public float ManeuverAdjustment(AITypes.Maneuver m, float curScore)
//    {
//        switch (m)
//        {
//            case AITypes.Maneuver.TACK_LEFT:
//            case AITypes.Maneuver.TACK_HALF_LEFT:
//            case AITypes.Maneuver.TACK_RIGHT:
//            case AITypes.Maneuver.TACK_HALF_RIGHT:
//                return curScore / m_tackThreshold;
//            default:
//                return curScore;
//        }
//    }

//    public Tuple<Move, float> ChooseMove(List<Move> moveList, Func<Vector3, float> scoreMove)
//    {
//        float bestScore = 0;
//        Move bestMove = moveList[0];
//        //string logStr = "";
//        foreach(Move m in moveList)
//        {
//            float score = ManeuverAdjustment(m.Item1, scoreMove(PredictLocation(m)));
//            //logStr += string.Format("{0} {1} {2}   ", BoatMovement.RotationSettingToString(m.Item1), BoatMovement.MovementSettingToString(m.Item2), score);
//            if (score > bestScore)
//            {
//                bestMove = m;
//                bestScore = score;
//            }
//        }
//        //Debug.Log(logStr);
//        //Debug.Log(string.Format("{0} {1} {2}", BoatMovement.RotationSettingToString(bestMove.Item1), BoatMovement.MovementSettingToString(bestMove.Item2), bestScore));
//        return new Tuple<Move, float>(bestMove, bestScore);
//    }

//    public void Follow(GameObject obj)
//    {
//        m_targetObject = obj;
//        m_mode = AITypes.Mode.FOLLOW;
//    }
//}