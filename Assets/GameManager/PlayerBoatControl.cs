using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoatControl : MonoBehaviour
{
    public KeyCode m_speedUp = KeyCode.W;
    public KeyCode m_speedDown = KeyCode.S;
    public KeyCode m_turnLeft = KeyCode.A;
    public KeyCode m_turnRight = KeyCode.D;
    public BoatMovement m_boat;
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
