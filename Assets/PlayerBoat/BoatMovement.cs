using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    private WindGauge windGauge; //TEMP: hack for now, access through game manager
    // Movement speed in units per second.
    public float lerpSpeed = 10.0F;


    public float oarSpeed = 2.0f;
    public float sailEfficiency = 1.0f;

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

    public MovementSetting movementSetting = MovementSetting.FULL_STOP;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    private Vector3 startPosition, endPosition;
    private bool advancingTurn = false;

    

    // Start is called before the first frame update
    void Start()
    {
        windGauge = FindObjectOfType<WindGauge>(); //TEMP
    }

    // Update is called once per frame
    void Update() {
        //will remove later when we flesh out the control scheme
        if (!advancingTurn) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                AdvanceTurn(); //This should be called from a gameManager class
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                IncreaseMovementSetting();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                DecreaseMovementSetting();
            }

            if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                TurnLeft();
            }

            if(Input.GetKeyDown(KeyCode.RightArrow)) {
                TurnRight();
            }
        }

        if (advancingTurn) {
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - startTime) * lerpSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);

            if (transform.position == endPosition) {
                advancingTurn = false;
            }
        }

    }

    public void AdvanceTurn()
    {
        if (!advancingTurn)
        {
            startTime = Time.time;
            startPosition = transform.position;
            float moveDistanceModifier = oarSpeed * MovementSettingToOarMultiplier(movementSetting);
            if (windGauge) {
                moveDistanceModifier += sailEfficiency * MovementSettingToSailMultiplier(movementSetting) * windGauge.m_windSpeed * WindEfficiency();
            }
            else
            {
                Debug.Log("No Wind Gauge");
            }
            endPosition = startPosition + (transform.forward * moveDistanceModifier);
            journeyLength = Vector3.Distance(transform.position, endPosition);
            Debug.DrawLine(transform.position, endPosition, Color.red, 100f);


            advancingTurn = true;
        }
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
        float deltaAngle = Math.Abs(windGauge.NormalizedAngle() - NormalizedAngle()) % 360;
        deltaAngle = deltaAngle > 180f ? 360f - deltaAngle : deltaAngle;
        Debug.Log(string.Format("Wind: {0}, Boat: {1}, Delta: {2}", windGauge.NormalizedAngle(), NormalizedAngle(), deltaAngle));
        float windEff = (180f - deltaAngle) / 180;
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
        if (movementSetting < MovementSetting.MOVEMENT_COUNT - 1)
        {
            movementSetting++;
        }
    }

    public void DecreaseMovementSetting()
    {
        if (movementSetting > 0)
        {
            movementSetting--;
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
