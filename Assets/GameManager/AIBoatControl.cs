using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Move = System.Tuple<BoatMovement.RotationSetting, BoatMovement.MovementSetting>;

public class AIBoatControl : MonoBehaviour
{
    

    public Vector3 m_target;
    private BoatMovement m_boat;
    private GameManager m_gameManager;
    
    public enum Mode
    {
        RANDOM,
        MAX_SPEED
    }
    public Mode m_mode;

    

    // Start is called before the first frame update
    void Start()
    {
        m_gameManager = GameManager.Get();
        m_mode = Mode.MAX_SPEED;
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
                default:
                    break;
            }
        }
    }
    public void SetBoat(BoatMovement boat)
    {
        m_boat = boat;
    }

    public void MoveRandomly()
    {
        float turnRand = UnityEngine.Random.value;
        if(turnRand > .9)
        {
            if(turnRand > .96)
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
        if(speedRand > .95)
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
        if (curMov > 0)
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
        var move = ChooseMove(moves, MaxSpeedScore);
        ExecuteMove(move);
    }

    public void ExecuteMove(Move toMove)
    {
        m_boat.m_rotationSetting = toMove.Item1;
        m_boat.m_movementSetting = toMove.Item2;
    }

    public Move ChooseMove(List<Move> moveList, Func<Move, float> scoreMove)
    {
        float bestScore = 0;
        Move bestMove = moveList[0];
        //string logStr = "";
        foreach(Move m in moveList)
        {
            float score = scoreMove(m);
            //logStr += string.Format("{0} {1} {2}   ", BoatMovement.RotationSettingToString(m.Item1), BoatMovement.MovementSettingToString(m.Item2), score);
            if (score > bestScore)
            {
                bestMove = m;
                bestScore = score;
            }
        }
        //Debug.Log(logStr);
        //Debug.Log(string.Format("{0} {1} {2}", BoatMovement.RotationSettingToString(bestMove.Item1), BoatMovement.MovementSettingToString(bestMove.Item2), bestScore));
        return bestMove;
    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoatControl : MonoBehaviour
{
    public KeyCode m_speedUp = KeyCode.W;
    public KeyCode m_speedDown = KeyCode.S;
    public KeyCode m_turnLeft = KeyCode.A;
    public KeyCode m_turnRight = KeyCode.D;
    private BoatMovement m_boat;
    private GameManager m_gameManager;
    // Start is called before the first frame update
    void Start()
    {
        m_gameManager = GameManager.Get();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_boat)
        {
            if (Input.GetKeyDown(m_speedUp))
            {
                m_boat.IncreaseMovementSetting();
            }

            if (Input.GetKeyDown(m_speedDown))
            {
                m_boat.DecreaseMovementSetting();
            }

            if (checkTurnKey(m_turnLeft) == checkTurnKey(m_turnRight))
            {
                m_boat.TurnForward();
            }
            else if (checkTurnKey(m_turnRight))
            {
                m_boat.TurnRight();
            }
            else
            {
                m_boat.TurnLeft();
            }
        }
    }

    private bool checkTurnKey(KeyCode c)
    {
        if (m_gameManager.Paused())
        {
            return Input.GetKeyDown(c);
        }
        else
        {
            return Input.GetKey(c);
        }
    }

    public void SetBoat(BoatMovement boat)
    {
        m_boat = boat;
    }
}
*/