using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject m_playerPrefab;
    public GameObject m_sailIndicatorPrefab;
    public GameObject m_windGaugePrefab;

    private FollowCamera m_mainCamera;
    private GameObject m_playerBoat;
    private SailIndicator m_sailIndicator;
    private WindGauge m_windGauge;
    
    public WeatherManager m_weatherManager;
    public PlayerBoatControl m_playerBoatControl;
    public AIBoatControl m_aiBoatControl;

    public float m_turnLength = 1.0f;

    private bool m_paused = true;
    private bool m_advancingTurn = false;

    private static GameManager s_instance;

    void Awake()
    {
        if (s_instance != null)
        {
            Debug.LogError("Multiple Game Managers");
            return;
        }

        s_instance = this;
    }

    void Start()
    {
        m_playerBoat = Instantiate(m_playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        m_mainCamera = GameObject.FindObjectOfType<FollowCamera>();
        m_mainCamera.Follow(m_playerBoat);
        //m_playerBoatControl.SetBoat(m_playerBoat.GetComponent<BoatMovement>());
        m_aiBoatControl.SetBoat(m_playerBoat.GetComponent<BoatMovement>());
        ConstructSailIndicator(m_playerBoat.GetComponent<BoatMovement>());
        ConstructWindGauge();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_advancingTurn)
        {
            if (m_playerBoat.GetComponent<BoatMovement>().TurnComplete())
            {
                m_advancingTurn = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                m_paused ^= true;
            }
            if (m_paused)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_advancingTurn = true;
                    m_playerBoat.GetComponent<BoatMovement>().AdvanceTurn(); //Need refactoring here to create a Turn sensitive base class
                }
            }

        }
    }

    public static GameManager Get()
    {
        return s_instance;
    }
    public WeatherManager Weather()
    {
        return m_weatherManager;
    }

    public bool Paused()
    {
        return m_paused;
    }

    void ConstructSailIndicator(BoatMovement boat)
    {
        m_sailIndicator = Instantiate(m_sailIndicatorPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<SailIndicator>();
        m_sailIndicator.SetShip(boat);
        m_sailIndicator.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
        m_sailIndicator.transform.localPosition = new Vector3(-308, -376, 0);
    }

    void ConstructWindGauge()
    {
        m_windGauge = Instantiate(m_windGaugePrefab, new Vector3(), Quaternion.identity).GetComponent<WindGauge>();
        m_windGauge.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
        m_windGauge.transform.localPosition = new Vector3(-468, -371, 0);
    }
}
