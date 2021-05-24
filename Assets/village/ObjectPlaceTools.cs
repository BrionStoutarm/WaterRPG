using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ObjectPlaceTools : MonoBehaviour
{
    static bool m_destroy = false;
    static bool showTerrainMenu = false;
    public Placeable m_basicGroundSeg;
    public Placeable m_basicRamp;
    public Placeable m_basicPath;
    public Placeable m_basicFarm;
    public Placeable m_basicIndustry;

    public ObjectPlacer m_placer;
    
    public void PlaceBasicFarm()
    {
        m_placer.ClearPlacer();
        m_placer.SetObjectToPlace(m_basicFarm);
        m_destroy = false;
        Debug.Log("placing farm");
    }

    public void PlaceBasicIndustry()
    {
        m_placer.ClearPlacer();
        m_placer.SetObjectToPlace(m_basicIndustry);
        m_destroy = false;
        Debug.Log("placing industry");
    }    

    public void TogglePathwayPlace()
    {
        m_placer.ClearPlacer();
        m_placer.SetObjectToPlace(m_basicPath);
        m_destroy = false;
        Debug.Log("Pathway Placement");
    }

    public void ToggleDestroy ()
    {
        m_destroy = !m_destroy;
        if (m_destroy)
        {
            m_placer.ClearPlacer();
            m_placer.SetDestroy(true);
        }
        else
            m_placer.SetDestroy(false);
    }

    public static bool isDestroy()
    {
        return m_destroy;
    }

    public void ToggleTerrainPlace()
    {
        m_placer.SetObjectToPlace(m_basicGroundSeg);
        m_destroy = false;
    }

    public void ToggleRampPlace()
    { 
        m_placer.SetObjectToPlace(m_basicRamp);
        m_destroy = false;
    }

    public void ToggleTerrainMenu()
    {
        showTerrainMenu = !showTerrainMenu;
    }

    public static bool showTerrainManipMenu()
    {
        return showTerrainMenu;
    }
}
