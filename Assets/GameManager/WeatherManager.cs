using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public float m_windDirection = 0f;
    public float m_windSpeed = 20f;
    public float m_maxWindSpeed = 90f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_windDirection += 45;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_windDirection -= 45;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_windSpeed = Mathf.Min(m_maxWindSpeed, m_windSpeed + 5);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_windSpeed = Mathf.Max(0f, m_windSpeed - 5);
        }
    }

    public float WindSpeed() //this may need to take a location if we have variable weather across the map
    {
        return m_windSpeed;
    }

    public float WindAngle()
    {
        return m_windDirection;
    }

    public float NormalizedWindAngle()
    {
        float normalizedWind = -1f * (m_windDirection % 360);

        if (normalizedWind < 0)
        {
            normalizedWind += 360;
        }
        return normalizedWind;
    }

}
