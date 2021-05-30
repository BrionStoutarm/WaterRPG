using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject m_toFollow;
    public Vector3 m_centerPosition = new Vector3(0, 0, 0);
    public float m_speed = 1.00f;
    public Vector3 m_offset;
    // Start is called before the first frame update
    void Start()
    {
        m_offset = transform.position; //hacky
    }

    // Update is called once per frame
    void Update()
    {
        if (m_toFollow)
        {
            m_centerPosition = m_toFollow.transform.position;
        }
        Vector3 target = m_centerPosition;
        target += m_offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, target, m_speed);
        transform.position = smoothedPosition;

    }

    public void Follow(GameObject obj)
    {
        m_toFollow = obj;
    }

    public void Center(Vector3 loc)
    {
        m_toFollow = null;
        m_centerPosition = loc;
    }
}
