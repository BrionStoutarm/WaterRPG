using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    private GameManager m_gameManager;
    // Movement speed in units per second.
    public float m_lerpSpeed = 10.0F;


    public float m_oarSpeed = 2.0f;
    public float m_sailEfficiency = 1.0f;

    public enum MovementSetting
    {
        OARS_BACK,
        FULL_STOP,
        OARS_FORWARD,
        SAIL_QUARTER,
        SAIL_HALF,
        SAIL_FULL,
        MOVEMENT_COUNT //Leave last
    }

    public MovementSetting m_movementSetting = MovementSetting.FULL_STOP;

    // Time when the movement started.
    private float m_startTime;

    // Total distance between the markers.
    private float m_journeyLength;

    private Vector3 m_startPosition, m_endPosition;
    private bool m_advancingTurn = false;

    

    // Start is called before the first frame update
    void Start()
    {
        m_gameManager = GameManager.Get();
    }

    // Update is called once per frame
    void Update() {
        if (m_gameManager.Paused())
        {
            if (m_advancingTurn)
            {
                TurnMovement();
            }
        }
        else
        {
            LiveMovement();
        }
    }

    private void TurnMovement()
    {
        if (transform.position == m_endPosition)
        {
            m_advancingTurn = false;
            return;
        }
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - m_startTime) * m_lerpSpeed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / m_journeyLength;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(m_startPosition, m_endPosition, fractionOfJourney);
    }

    private void LiveMovement()
    {

        float speed = MovementSpeed() * Time.deltaTime;
        if(speed != 0f)
        {
            Vector3 newPos = transform.position + (transform.forward * speed);
            Debug.DrawLine(transform.position, newPos, Color.red, 100f);
            transform.position = newPos;
        }
       

    }

    public void AdvanceTurn()
    {
        if (!m_advancingTurn)
        {
            m_startTime = Time.time;
            m_startPosition = transform.position;
            float speed = MovementSpeed();

            m_endPosition = m_startPosition;
            if (speed != 0f)
            {
                m_endPosition += (transform.forward * speed);
                m_journeyLength = Vector3.Distance(transform.position, m_endPosition);
                Debug.DrawLine(transform.position, m_endPosition, Color.red, 100f);
            }


            m_advancingTurn = true;
        }
    }

    public float MovementSpeed()
    {
        float speed = m_oarSpeed * MovementSettingToOarMultiplier(m_movementSetting);
        speed += m_sailEfficiency * MovementSettingToSailMultiplier(m_movementSetting) * m_gameManager.Weather().WindSpeed() * WindEfficiency();
        return speed;
    }

    public bool TurnComplete()
    {
        return !m_advancingTurn;
    }

    public void TurnLeft() {
        transform.Rotate(0, -15f, 0);
    }

    public void TurnRight() {
        transform.Rotate(0, 15f, 0);
    }

    public float NormalizedAngle()
    {
        float normalizedAngle = transform.eulerAngles.y % 360;
        if (normalizedAngle < 0)
        {
            normalizedAngle += 360;
        }
        return normalizedAngle;

    }
    public float WindEfficiency()
    {
        float deltaAngle = Math.Abs(m_gameManager.Weather().NormalizedWindAngle() - NormalizedAngle()) % 360;
        deltaAngle = deltaAngle > 180f ? 360f - deltaAngle : deltaAngle;
        //Debug.Log(string.Format("Wind: {0}, Boat: {1}, Delta: {2}", windGauge.NormalizedAngle(), NormalizedAngle(), deltaAngle));
        float windEff = (180f - deltaAngle) / 180;
        //Debug.Log(windEff);
        return windEff;
    }

    public float MovementSettingToSailMultiplier(MovementSetting x)
    {
        switch (x)
        {
            case MovementSetting.SAIL_QUARTER:
                return .25f;
            case MovementSetting.SAIL_HALF:
                return .5f;
            case MovementSetting.SAIL_FULL:
                return 1f;
            default:
                return 0f;
        }
    }
    public float MovementSettingToOarMultiplier(MovementSetting x)
    {
        switch (x)
        {
            case MovementSetting.OARS_BACK:
                return -1f;
            case MovementSetting.OARS_FORWARD:
                return 1f;
            default:
                return 0f;
        }
    }

    public string MovementSettingToString(MovementSetting x)
    {
        switch (x)
        {
            case MovementSetting.OARS_BACK:
                return "OARS_BACK";
            case MovementSetting.FULL_STOP:
                return "FULL_STOP";
            case MovementSetting.OARS_FORWARD:
                return "OARS_FWD";
            case MovementSetting.SAIL_QUARTER:
                return "SAIL_Q";
            case MovementSetting.SAIL_HALF:
                return "SAIL_H";
            case MovementSetting.SAIL_FULL:
                return "SAIL_F";
            default:
                return "UNKNOWN";
        }
    }
    public float MovementSettingToWindMultiple(MovementSetting x)
    {
        switch (x)
        {
            case MovementSetting.SAIL_QUARTER:
                return .25f;
            case MovementSetting.SAIL_HALF:
                return .5f;
            case MovementSetting.SAIL_FULL:
                return 1f;
            default:
                return 0f;
        }
    }

    public void IncreaseMovementSetting()
    {
        if (m_movementSetting < MovementSetting.MOVEMENT_COUNT - 1)
        {
            m_movementSetting++;
        }
    }

    public void DecreaseMovementSetting()
    {
        if (m_movementSetting > 0)
        {
            m_movementSetting--;
        }
    }

    void OnDrawGizmos() {
        // Draw a yellow sphere at the transform's position
        Vector3 fwd = transform.position + (transform.forward * 20);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.localPosition, fwd);
        Gizmos.DrawSphere(fwd, 5f);
    }
}
