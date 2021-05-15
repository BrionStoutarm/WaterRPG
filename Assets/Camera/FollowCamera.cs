using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject m_toFollow;
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
            Vector3 target = m_toFollow.transform.position;
            target += m_offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, target , m_speed);
            transform.position = smoothedPosition;
        }
        
    }
}
