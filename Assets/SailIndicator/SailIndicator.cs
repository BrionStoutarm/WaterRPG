using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailIndicator : MonoBehaviour
{
    public BoatMovement m_ship;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_ship)
        {
            GetComponent<UnityEngine.UI.Text>().text = BoatMovement.MovementSettingToString(m_ship.m_movementSetting);
        }
    }

    public void SetShip(BoatMovement ship)
    {
        m_ship = ship;
    }
}
