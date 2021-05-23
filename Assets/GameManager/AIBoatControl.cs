using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Move = System.Tuple<BoatMovement.RotationSetting, BoatMovement.MovementSetting>;

public class AIBoatControl : MonoBehaviour
{


    public Vector3 m_target;
    public float m_gotoThreshold = 2f;
    public float m_tackTargetAngle = 0;
    public float m_tackAngleThreshold = 5f; //acceptable angle within target
    public float m_tackThreshold = 1.00001f;
    private BoatMovement m_boat;
    private GameManager m_gameManager;

    public enum Mode
    {
        WAIT,
        RANDOM,
        MAX_SPEED,
        GO_TO
    }
    public Mode m_mode;

    public enum Maneuver
    {
        NONE,
        TACK_LEFT,
        TACK_RIGHT
    }
    public Maneuver m_manuever;


    // Start is called before the first frame update
    void Start()
    {
        m_gameManager = GameManager.Get();
        m_mode = Mode.GO_TO;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_gameManager.Paused())
        {
            return; //eventually need to make turn based decisions, but do this on demand not in update
        }
        if (m_boat)
        {
            switch (m_mode)
            {
                case (Mode.RANDOM):
                    MoveRandomly();
                    break;
                case (Mode.MAX_SPEED):
                    MoveMaxSpeed();
                    break;
                case (Mode.GO_TO):
                    MoveGoTo();
                    break;
                default:
                    FullStop();
                    break;
            }
        }
    }
    public void SetBoat(BoatMovement boat)
    {
        m_boat = boat;
    }

    public void SetTarget(Vector3 target)
    {
        m_target = target;
        m_mode = Mode.GO_TO;
    }

    public void FullStop()
    {
        ExecuteMove(new Move(BoatMovement.RotationSetting.FORWARD, BoatMovement.MovementSetting.FULL_STOP));
    }

    public void MoveRandomly()
    {
        float turnRand = UnityEngine.Random.value;
        if (turnRand > .9)
        {
            if (turnRand > .96)
            {
                m_boat.LiveRotationSetting(BoatMovement.RotationSetting.FORWARD);
            }
            else if (turnRand > .93)
            {
                m_boat.LiveRotationSetting(BoatMovement.RotationSetting.LEFT);
            }
            else
            {
                m_boat.LiveRotationSetting(BoatMovement.RotationSetting.RIGHT);
            }
        }
        float speedRand = UnityEngine.Random.value;
        if (speedRand > .95)
        {
            if (speedRand > .985)
            {
                m_boat.DecreaseMovementSetting();
            }
            else
            {
                m_boat.IncreaseMovementSetting();
            }
        }
    }
    public List<Move> GenerateMoves()
    {
        var ret = new List<Move>(); //may be more efficient to create the exhaustive list once and legalize it
        var curMov = m_boat.m_movementSetting;
        ret.Add(new Move(BoatMovement.RotationSetting.FORWARD, curMov));
        ret.Add(new Move(BoatMovement.RotationSetting.LEFT, curMov));
        ret.Add(new Move(BoatMovement.RotationSetting.RIGHT, curMov));
        if (curMov > BoatMovement.MovementSetting.FULL_STOP) //Exclude oarsback to avoid cases where the AI won't turn
        {
            ret.Add(new Move(BoatMovement.RotationSetting.FORWARD, curMov - 1));
            ret.Add(new Move(BoatMovement.RotationSetting.LEFT, curMov - 1));
            ret.Add(new Move(BoatMovement.RotationSetting.RIGHT, curMov - 1));
        }
        if (curMov < (BoatMovement.MovementSetting.MOVEMENT_COUNT - 1))
        {
            ret.Add(new Move(BoatMovement.RotationSetting.FORWARD, curMov + 1));
            ret.Add(new Move(BoatMovement.RotationSetting.LEFT, curMov + 1));
            ret.Add(new Move(BoatMovement.RotationSetting.RIGHT, curMov + 1));
        }
        return ret;
    }

    public float MaxSpeedScore(Move m)
    {
        Vector3 predictedLocation = m_boat.LivePredictLocation(m.Item1, m.Item2);
        float score = Vector3.Distance(m_boat.transform.position, predictedLocation);
        //Debug.Log(string.Format("{0} {1} {2}", BoatMovement.RotationSettingToString(m.Item1), BoatMovement.MovementSettingToString(m.Item2), score));
        return score;
    }
    public void MoveMaxSpeed()
    {
        var moves = GenerateMoves();
        var moveAndScore = ChooseMove(moves, MaxSpeedScore);
        ExecuteMove(moveAndScore.Item1);
    }

    public float GoToScore(Move m)
    {
        Vector3 predictedLocation = m_boat.LivePredictLocation(m.Item1, m.Item2);
        return GoToScore(predictedLocation);
    }

    public float GoToScore(Vector3 predictedLocation)
    {
        float score = 1 / (1 + Vector3.Distance(m_target, predictedLocation));
        //Debug.Log(string.Format("{0} {1} {2} {3}", BoatMovement.RotationSettingToString(m.Item1), BoatMovement.MovementSettingToString(m.Item2), Vector3.Distance(m_target, predictedLocation), score));
        return score;
    }

    public void MoveGoTo()
    {
        if(Vector3.Distance(m_target, m_boat.transform.position) < m_gotoThreshold){
            m_mode = Mode.WAIT;
            FullStop();
            return;
        }
        var moves = GenerateMoves();
        LegalizeGoTo(ref moves);
        if(m_manuever != Maneuver.NONE)
        {
            if(m_manuever == Maneuver.TACK_LEFT || m_manuever == Maneuver.TACK_RIGHT)
            {
                if (m_boat.NormalizeAngle(m_boat.NormalizedAngle() - m_tackTargetAngle) < m_tackAngleThreshold)
                {
                    m_manuever = Maneuver.NONE;
                }
            }
            LegalizeManeuver(ref moves);
        }
        var moveAndScore = ChooseMove(moves, GoToScore);
        var move = moveAndScore.Item1;
        if(m_manuever == Maneuver.NONE)
        {
            var leftTackLocation = m_boat.LivePredictTackPosition(BoatMovement.RotationSetting.LEFT, m_boat.m_movementSetting);
            var leftTackScore = GoToScore(leftTackLocation);
            var rightTackLocation = m_boat.LivePredictTackPosition(BoatMovement.RotationSetting.RIGHT, m_boat.m_movementSetting);
            var rightTackScore = GoToScore(rightTackLocation);
            //Debug.Log(string.Format("LeftScore: {0}", leftTackScore/ moveAndScore.Item2));
            if ((leftTackScore / moveAndScore.Item2) > m_tackThreshold)
            {
                //Debug.Log("TACKING LEFT");
                m_manuever = Maneuver.TACK_LEFT;
                m_tackTargetAngle = m_boat.NormalizeAngle(m_boat.NormalizedAngle() - 90f);
                return;
            }
            else
            {

                //Debug.Log(string.Format("RightScore: {0}", rightTackScore / moveAndScore.Item2));
                if ((rightTackScore / moveAndScore.Item2) > m_tackThreshold)
                {
                    //Debug.Log("TACKING RIGHT");
                    m_manuever = Maneuver.TACK_RIGHT;
                    m_tackTargetAngle = m_boat.NormalizeAngle(m_boat.NormalizedAngle() + 90f);
                    return;
                }
            }
        }
        ExecuteMove(move);
    }

    private bool isFullStop(Move m)
    {
        if(m.Item2 == BoatMovement.MovementSetting.FULL_STOP)
        {
            return true;
        }
        return false;
    }

    private bool isNotLeft(Move m)
    {
        if(m.Item1 != BoatMovement.RotationSetting.LEFT)
        {
            return true;
        }
        return false;
    }

    private bool isNotRight(Move m)
    {
        if (m.Item1 != BoatMovement.RotationSetting.RIGHT)
        {
            return true;
        }
        return false;
    }

    public void LegalizeGoTo(ref List<Move> moveList)
    {
        if (Vector3.Distance(m_boat.transform.position, m_target) > m_gotoThreshold);
        {
            moveList.RemoveAll(isFullStop);
        }
    }

    public void LegalizeManeuver(ref List<Move> moveList)
    {
        if(m_manuever == Maneuver.TACK_LEFT)
        {
            moveList.RemoveAll(isNotLeft);
        }
        else if(m_manuever == Maneuver.TACK_RIGHT)
        {
            moveList.RemoveAll(isNotRight);
        }
    }
    
    public void ExecuteMove(Move toMove)
    {
        m_boat.m_rotationSetting = toMove.Item1;
        m_boat.m_movementSetting = toMove.Item2;
    }

    public Tuple<Move, float> ChooseMove(List<Move> moveList, Func<Move, float> scoreMove)
    {
        float bestScore = 0;
        Move bestMove = moveList[0];
        string logStr = "";
        foreach(Move m in moveList)
        {
            float score = scoreMove(m);
            logStr += string.Format("{0} {1} {2}   ", BoatMovement.RotationSettingToString(m.Item1), BoatMovement.MovementSettingToString(m.Item2), score);
            if (score > bestScore)
            {
                bestMove = m;
                bestScore = score;
            }
        }
        //Debug.Log(logStr);
        //Debug.Log(string.Format("{0} {1} {2}", BoatMovement.RotationSettingToString(bestMove.Item1), BoatMovement.MovementSettingToString(bestMove.Item2), bestScore));
        return new Tuple<Move, float>(bestMove, bestScore);
    }
}