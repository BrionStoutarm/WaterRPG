using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindGauge : MonoBehaviour
{
    public Vector3 m_windDirection;
    public float m_windSpeed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        m_windDirection = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        //TEMP: should be handled in weather manager in future
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_windDirection.z += 45;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            m_windDirection.z -= 45;
        }

        Vector3 currentAngle = transform.eulerAngles;
        currentAngle = new Vector3(
             Mathf.LerpAngle(currentAngle.x, m_windDirection.x, Time.deltaTime),
             Mathf.LerpAngle(currentAngle.y, m_windDirection.y, Time.deltaTime),
             Mathf.LerpAngle(currentAngle.z, m_windDirection.z, Time.deltaTime));

        transform.eulerAngles = currentAngle;
    }

    public float NormalizedAngle()
    {
        float normalizedWind = -1f * (m_windDirection.z % 360);
        
        if (normalizedWind < 0) {
            normalizedWind += 360;
        }
        return normalizedWind; 
    }
}
