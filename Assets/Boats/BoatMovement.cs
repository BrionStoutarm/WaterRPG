//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Net.Sockets;
//using UnityEngine;

//public class BoatMovement : MonoBehaviour
//{
//    public float m_oarSpeed = 2.0f;
//    public float m_sailEfficiency = 1.0f;
//    public float m_rotationSpeed = 90f; //degrees/sec

//    public Rigidbody m_rigidbody;
//    private GameManager m_gameManager;


//    public enum MovementSetting
//    {
//        OARS_BACK,
//        FULL_STOP,
//        OARS_FORWARD,
//        SAIL_QUARTER,
//        SAIL_HALF,
//        SAIL_FULL,
//        MOVEMENT_COUNT //Leave last
//    }

//    public enum RotationSetting
//    {
//        FORWARD,
//        LEFT,
//        RIGHT
//    }

//    public RotationSetting m_rotationSetting = RotationSetting.FORWARD;
//    public MovementSetting m_movementSetting = MovementSetting.FULL_STOP;

//    // Time when the movement started.
//    private float m_startTime;

//    // Total distance between the markers.
//    private float m_journeyLength;

//    private Vector3 m_startPosition, m_endPosition;
//    private bool m_advancingTurn = false;

    

//    // Start is called before the first frame update
//    void Start()
//    {
//        m_gameManager = GameManager.Get();
//        m_rigidbody = GetComponent<Rigidbody>();
//    }

//    // Update is called once per frame
//    void FixedUpdate() {
//        if (m_gameManager.Paused())
//        {
//            if (m_advancingTurn)
//            {
//                TurnMovement();
//            }
//        }
//        else
//        {
//            LiveRotation();
//            LiveMovement();
//        }
//    }

//    void OnCollisionEnter(Collision collision)
//    {
//        foreach (ContactPoint contact in collision.contacts)
//        {
//            Debug.DrawLine(contact.point, transform.position, Color.yellow);
//        }
//    }

//    private void TurnMovement()
//    {
//        if (transform.position == m_endPosition)
//        {
//            m_advancingTurn = false;
//            return;
//        }

//        // Fraction of journey completed equals current distance divided by total distance.
//        float fractionOfJourney = (Time.time - m_startTime) / m_gameManager.m_turnLength;

//        // Set our position as a fraction of the distance between the markers.
//        m_rigidbody.MovePosition(Vector3.Lerp(m_startPosition, m_endPosition, fractionOfJourney));
//    }

//    private void LiveMovement()
//    {
//        Vector3 newPos = LiveMovementPosition(m_rotationSetting, m_movementSetting);
//        if (newPos != transform.position)
//        {
//            Debug.DrawLine(transform.position, newPos, Color.red, 100f);
//            m_rigidbody.MovePosition(newPos);
//            //transform.position = newPos;
//        }
//    }

//    private Vector3 LiveMovementPosition(RotationSetting rot, MovementSetting mov)
//    {
//        Vector3 dirVector = transform.forward;
//        switch (rot)
//        {
//            case (RotationSetting.LEFT):
//                dirVector = Quaternion.Euler(0, -m_rotationSpeed * Time.deltaTime, 0) * dirVector;
//                break;
//            case (RotationSetting.RIGHT):
//                dirVector = Quaternion.Euler(0, m_rotationSpeed * Time.deltaTime, 0) * dirVector;
//                break;
//            default:
//                break;
//        }
//        return LiveMovementPosition(dirVector, mov);
//    }

//    public Vector3 LivePredictTackPosition(RotationSetting rot, MovementSetting mov, float angle)
//    {
//        Vector3 dirVector = transform.forward;
//        switch (rot)
//        {
//            case (RotationSetting.LEFT):
//                dirVector = Quaternion.Euler(0, -angle, 0) * dirVector;
//                break;
//            case (RotationSetting.RIGHT):
//                dirVector = Quaternion.Euler(0, angle, 0) * dirVector;
//                break;
//            default:
//                break;
//        }
//        return LiveMovementPosition(dirVector, mov);
//    }

//    private Vector3 LiveMovementPosition(Vector3 dirVector, MovementSetting mov)
//    {
//        float speed = MovementSpeed(mov, Quaternion.LookRotation(dirVector).eulerAngles.y) * Time.deltaTime;
//        Vector3 newPos = transform.position + (dirVector * speed);
//        return newPos;
//    }

//    private void LiveRotation()
//    {
//        //Debug.Log(m_rotationSpeed);
//        switch (m_rotationSetting)
//        {
//            case (RotationSetting.LEFT):
//                //Debug.Log(-m_rotationSpeed * Time.deltaTime);
//                Quaternion leftRotation = Quaternion.Euler(new Vector3(0, -m_rotationSpeed * Time.fixedDeltaTime, 0));
//                m_rigidbody.MoveRotation(m_rigidbody.rotation * leftRotation);
//                //transform.Rotate(0, -m_rotationSpeed * Time.deltaTime, 0);
//                break;
//            case (RotationSetting.RIGHT):
//                //Debug.Log(m_rotationSpeed * Time.deltaTime);
//                Quaternion rightRotation = Quaternion.Euler(new Vector3(0, m_rotationSpeed * Time.fixedDeltaTime, 0));
//                m_rigidbody.MoveRotation(m_rigidbody.rotation * rightRotation);
//                //transform.Rotate(0, m_rotationSpeed * Time.deltaTime, 0);
//                break;
//            default:
//                return;
//        }
//    }

//    public void AdvanceTurn()
//    {
//        if (!m_advancingTurn)
//        {
//            m_startTime = Time.time;
//            m_startPosition = transform.position;
//            float speed = MovementSpeed(m_movementSetting, ForwardAngle());

//            m_endPosition = m_startPosition;
//            if (speed != 0f)
//            {
//                m_endPosition += (transform.forward * speed * m_gameManager.m_turnLength);
//                m_journeyLength = Vector3.Distance(transform.position, m_endPosition);
//                Debug.DrawLine(transform.position, m_endPosition, Color.red, 100f);
//            }


//            m_advancingTurn = true;
//        }
//    }

//    public float MovementSpeed(MovementSetting mov, float angle)
//    {
//        float speed = m_oarSpeed * MovementSettingToOarMultiplier(mov);
//        speed += m_sailEfficiency * MovementSettingToSailMultiplier(mov) * m_gameManager.Weather().WindSpeed() * WindEfficiency(angle);
//        return speed;
//    }

//    public bool TurnComplete()
//    {
//        return !m_advancingTurn;
//    }

//    public void TurnForward()
//    {
//        m_rotationSetting = RotationSetting.FORWARD;
//    }

//    public void TurnLeft() {
//        if (m_gameManager.Paused())
//        {
//            transform.Rotate(0, -15f, 0);
//        }
//        m_rotationSetting = RotationSetting.LEFT;
//    }

//    public void TurnRight() {
//        if (m_gameManager.Paused())
//        {
//            transform.Rotate(0, 15f, 0);
//        }
//        m_rotationSetting = RotationSetting.RIGHT;
//    }

//    public Vector3 LivePredictLocation(BoatMovement.RotationSetting rot, MovementSetting mov)
//    {
//        return LiveMovementPosition(rot, mov);
//    }

//    public void LiveRotationSetting(BoatMovement.RotationSetting rot)
//    {
//        m_rotationSetting = rot;
//    }

//    public float ForwardAngle()
//    {
//        return transform.eulerAngles.y;
//    }
//    public float NormalizeAngle(float angle)
//    {
//        float normalizedAngle = angle % 360;
//        if (normalizedAngle < 0)
//        {
//            normalizedAngle += 360;
//        }
//        return normalizedAngle;
//    }
//    public float NormalizedAngle()
//    {
//        return NormalizeAngle(ForwardAngle());
//    }

//    public float WindEfficiency(float angle)
//    {
//        float deltaAngle = Math.Abs(m_gameManager.Weather().NormalizedWindAngle() - NormalizeAngle(angle)) % 360;
//        deltaAngle = deltaAngle > 180f ? 360f - deltaAngle : deltaAngle;
//        float windEff = (180f - deltaAngle) / 180;
//        //Debug.Log(string.Format("Wind: {0}, Boat: {1}, Delta: {2}, Eff: {3}", m_gameManager.Weather().NormalizedWindAngle(), NormalizeAngle(angle), deltaAngle, windEff));
//        return windEff;
//    }

//    public float MovementSettingToSailMultiplier(MovementSetting x)
//    {
//        switch (x)
//        {
//            case MovementSetting.SAIL_QUARTER:
//                return .25f;
//            case MovementSetting.SAIL_HALF:
//                return .5f;
//            case MovementSetting.SAIL_FULL:
//                return 1f;
//            default:
//                return 0f;
//        }
//    }
//    public float MovementSettingToOarMultiplier(MovementSetting x)
//    {
//        switch (x)
//        {
//            case MovementSetting.OARS_BACK:
//                return -.5f;
//            case MovementSetting.OARS_FORWARD:
//                return 1f;
//            default:
//                return 0f;
//        }
//    }

//    public static string MovementSettingToString(MovementSetting x)
//    {
//        switch (x)
//        {
//            case MovementSetting.OARS_BACK:
//                return "OARS_BACK";
//            case MovementSetting.FULL_STOP:
//                return "FULL_STOP";
//            case MovementSetting.OARS_FORWARD:
//                return "OARS_FWD";
//            case MovementSetting.SAIL_QUARTER:
//                return "SAIL_Q";
//            case MovementSetting.SAIL_HALF:
//                return "SAIL_H";
//            case MovementSetting.SAIL_FULL:
//                return "SAIL_F";
//            default:
//                return "UNKNOWN";
//        }
//    }

//    public static string RotationSettingToString(RotationSetting x)
//    {
//        switch (x)
//        {
//            case RotationSetting.FORWARD:
//                return "FORWARD";
//            case RotationSetting.LEFT:
//                return "LEFT";
//            case RotationSetting.RIGHT:
//                return "RIGHT";
//            default:
//                return "DEFAULT";
//        }
//    }
//    public float MovementSettingToWindMultiple(MovementSetting x)
//    {
//        switch (x)
//        {
//            case MovementSetting.SAIL_QUARTER:
//                return .25f;
//            case MovementSetting.SAIL_HALF:
//                return .5f;
//            case MovementSetting.SAIL_FULL:
//                return 1f;
//            default:
//                return 0f;
//        }
//    }

//    public void IncreaseMovementSetting()
//    {
//        if (m_movementSetting < MovementSetting.MOVEMENT_COUNT - 1)
//        {
//            m_movementSetting++;
//        }
//    }

//    public void DecreaseMovementSetting()
//    {
//        if (m_movementSetting > 0)
//        {
//            m_movementSetting--;
//        }
//    }

//    void OnDrawGizmos() {
//        // Draw a yellow sphere at the transform's position
//        Vector3 fwd = transform.position + (transform.forward * 20);

//        Gizmos.color = Color.red;
//        Gizmos.DrawLine(transform.localPosition, fwd);
//        Gizmos.DrawSphere(fwd, 5f);
//    }
//}
