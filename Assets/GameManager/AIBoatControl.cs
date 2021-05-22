using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBoatControl : MonoBehaviour
{
    public Vector3 m_target;
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
        if (m_gameManager.Paused())
        {
            return; //eventually need to make turn based decisions, but do this on demand not in update
        }
        if (m_boat)
        {
            MoveRandomly();
        }
    }
    public void SetBoat(BoatMovement boat)
    {
        m_boat = boat;
    }

    public void MoveRandomly()
    {
        float turnRand = Random.value;
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
        float speedRand = Random.value;
        if(Random.value > .95)
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