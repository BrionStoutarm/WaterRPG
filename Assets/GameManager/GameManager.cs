using System;
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

    public static GameManager Instance {
        get => s_instance;
        set {
            if (value != null)
                s_instance = value; 
        }
    }

    public event EventHandler<TimeStepArgs> TimeStepEvent;
    public class TimeStepArgs : EventArgs { }

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
        InvokeRepeating("TriggerTimeStep", 0.0f, 10f);

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

    //Triggers the timestep event that other objects will listen to
    private void TriggerTimeStep() {
        if(TimeStepEvent != null) {
            TimeStepEvent(this, new TimeStepArgs { });
        }
    }

    public bool OnDebug() {
        return m_debugMode;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleBuildSystem() {
        GridBuildingSystem.Instance.ToggleActive();
    }
}
