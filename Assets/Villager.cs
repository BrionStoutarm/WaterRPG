using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour
{
    public Rigidbody m_rigidbody;
    public float m_rotationSpeed = 90f;
    public float m_walkSpeed = 10f;

    public enum MovementSetting
    {
        STOP,
        WALK,
        MOVEMENT_COUNT //Leave last
    }

    public enum RotationSetting
    {
        FORWARD,
        LEFT,
        RIGHT,
        ROTATION_COUNT
    }

    public RotationSetting m_rotationSetting = RotationSetting.FORWARD;
    public MovementSetting m_movementSetting = MovementSetting.STOP;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponentInChildren<Rigidbody>();
        if (!m_rigidbody)
        {
            Debug.Log("NO RIGIDBODY");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LiveRotation();
        LiveMovement();
    }

    void LiveMovement()
    {
        Vector3 newPos = LiveMovementPosition(m_rotationSetting, m_movementSetting);
        if (newPos != transform.position)
        {
            Debug.DrawLine(transform.position, newPos, Color.red, 100f);
            m_rigidbody.MovePosition(newPos);
        }
    }
    private void LiveRotation()
    {
        switch (m_rotationSetting)
        {
            case (RotationSetting.LEFT):
                Quaternion leftRotation = Quaternion.Euler(new Vector3(0, -m_rotationSpeed * Time.fixedDeltaTime, 0));
                m_rigidbody.MoveRotation(m_rigidbody.rotation * leftRotation);
                break;
            case (RotationSetting.RIGHT):
                Quaternion rightRotation = Quaternion.Euler(new Vector3(0, m_rotationSpeed * Time.fixedDeltaTime, 0));
                m_rigidbody.MoveRotation(m_rigidbody.rotation * rightRotation);
                break;
            default:
                return;
        }
    }

    public Vector3 LiveMovementPosition(RotationSetting rot, MovementSetting mov)
    {
        Vector3 dirVector = transform.forward;
        switch (rot)
        {
            case (RotationSetting.LEFT):
                dirVector = Quaternion.Euler(0, -m_rotationSpeed * Time.deltaTime, 0) * dirVector;
                break;
            case (RotationSetting.RIGHT):
                dirVector = Quaternion.Euler(0, m_rotationSpeed * Time.deltaTime, 0) * dirVector;
                break;
            default:
                break;
        }
        return LiveMovementPosition(dirVector, mov);
    }

    public float MovementSettingSpeed(MovementSetting mov)
    {
        switch (mov)
        {
            case MovementSetting.WALK:
                return m_walkSpeed;
            default:
                return 0f;
        }
    }

    public Vector3 LiveMovementPosition(Vector3 dirVector, MovementSetting mov)
    {
        float speed = MovementSettingSpeed(mov) * Time.deltaTime;
        Vector3 newPos = transform.position + (dirVector * speed);
        return newPos;
    }
}
