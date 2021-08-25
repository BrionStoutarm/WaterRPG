using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool m_debugMode;
    //public GameObject m_playerPrefab;
   // public GameObject m_sailIndicatorPrefab;
    //public GameObject m_windGaugePrefab;
    //public GameObject m_aiBoat;

    //public GameObject m_playerBoat;
    //private SailIndicator m_sailIndicator;
    //private WindGauge m_windGauge;
    
    //public WeatherManager m_weatherManager;
    //public PlayerBoatControl m_playerBoatControl;
    //public AIBoatControl m_aiBoatControl;
    //public WaypointGraph m_waypoints;

    //public float m_turnLength = 1.0f;
    public bool m_showWind = true;

    //private bool m_paused = false;
    //private bool m_advancingTurn = false;

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
        //m_mainCamera = GameObject.FindObjectOfType<FollowCamera>();
        //if (m_playerBoat)
        //{
        //    m_mainCamera.Follow(m_playerBoat);
        //    ConstructSailIndicator(m_playerBoat.GetComponent<BoatMovement>());
        //}
        //if (m_showWind)
        //{
        //    ConstructWindGauge();
        //}
    }
    
    public bool OnDebug() {
        return m_debugMode;
    }

    // Update is called once per frame
    void Update()
    {
        //if (m_advancingTurn)
        //{
        //    if (m_playerBoat && m_playerBoat.GetComponent<BoatMovement>().TurnComplete())
        //    {
        //        m_advancingTurn = false;
        //    }
        //}
        //else
        //{
        //    if (m_paused)
        //    {
        //        if (Input.GetKeyDown(KeyCode.Space))
        //        {
        //            m_advancingTurn = true;
        //            if (m_playerBoat)
        //            {
        //                m_playerBoat.GetComponent<BoatMovement>().AdvanceTurn(); //Need refactoring here to create a Turn sensitive base class
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (Input.GetKeyDown(KeyCode.M))
        //        {
        //            m_aiBoatControl.m_mode++;
        //            if (m_aiBoatControl.m_mode >= AITypes.Mode.MODE_END)
        //            {
        //                m_aiBoatControl.m_mode = AITypes.Mode.WAIT;
        //            }
        //        }
        //    }
        //}
    }

    public static GameManager Get()
    {
        return s_instance;
    }
    //public WeatherManager Weather()
    //{
    //    return m_weatherManager;
    //}

    //public bool Paused()
    //{
    //    return m_paused;
    //}

    //public void TogglePause()
    //{
    //    m_paused ^= true;
    //}

    //void ConstructSailIndicator(BoatMovement boat)
    //{
    //    m_sailIndicator = Instantiate(m_sailIndicatorPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<SailIndicator>();
    //    m_sailIndicator.SetShip(boat);
    //    m_sailIndicator.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
    //    m_sailIndicator.transform.localPosition = new Vector3(-308, -376, 0);
    //}

    //void ConstructWindGauge()
    //{
    //    m_windGauge = Instantiate(m_windGaugePrefab, new Vector3(), Quaternion.identity).GetComponent<WindGauge>();
    //    m_windGauge.transform.SetParent(GameObject.FindObjectOfType<Canvas>().transform);
    //    m_windGauge.transform.localPosition = new Vector3(-468, -371, 0);
    //}

    //public WaypointGraph WaypointGraph()
    //{
    //    return m_waypoints;
    //}
}
