using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject m_toFollow;
    public Vector3 m_centerPosition = new Vector3(0, 0, 0);
    public float m_lerpSpeed = .50f;
    public float m_moveSpeed = 1f;
    public Vector3 m_offset;

    private float m_startTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        m_offset = transform.position; //hacky
    }

    // Update is called once per frame
    void Update()
    {
        var delta = Time.time - m_startTime;
        if (m_toFollow)
        {
            m_centerPosition = m_toFollow.transform.position;
            delta = 1f;
        }
        if (transform.position != m_centerPosition)
        {
            Vector3 target = m_centerPosition;
            target += m_offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, target, m_lerpSpeed / (delta));
            transform.position = smoothedPosition;
        }

    }

    public void Follow(GameObject obj)
    {
        m_toFollow = obj;
    }

    public void Center(Vector3 loc)
    {
        m_startTime = Time.time;
        m_toFollow = null;
        m_centerPosition = loc;
    }

    public void Left()
    {
        m_startTime = Time.time;
        m_toFollow = null;
        m_centerPosition.x -= m_moveSpeed;
    }
    public void Right()
    {
        m_startTime = Time.time;
        m_toFollow = null;
        m_centerPosition.x += m_moveSpeed;
    }
    public void Up()
    {
        m_startTime = Time.time;
        m_toFollow = null;
        m_centerPosition.z += m_moveSpeed;
    }
    public void Down()
    {
        m_startTime = Time.time;
        m_toFollow = null;
        m_centerPosition.z -= m_moveSpeed;
    }
}
