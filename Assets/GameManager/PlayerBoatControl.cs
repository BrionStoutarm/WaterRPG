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
    // Start is called before the first frame update
    void Start()
    {
        
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

            if (Input.GetKeyDown(m_turnLeft))
            {
                m_boat.TurnLeft();
            }

            if (Input.GetKeyDown(m_turnRight))
            {
                m_boat.TurnRight();
            }
        }
    }

    public void SetBoat(BoatMovement boat)
    {
        m_boat = boat;
    }
}
