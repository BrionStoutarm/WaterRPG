using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VillageCenter : MonoBehaviour
{
    public GameObject m_villagerPrefab;
    public List<GameObject> m_villagers;
    public uint m_maxVillagers = 10;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }


    public bool OkToSpawnVillager()
    {
        if(m_villagers.Count < m_maxVillagers)
        {
            return true;
        }
        return false;
    }

    public void SpawnVillager()
    {
        if (OkToSpawnVillager())
        {
            Vector3 spawnLocation = transform.position - new Vector3(-5, 0, 0);
            m_villagers.Add(Instantiate(m_villagerPrefab, spawnLocation, Quaternion.identity));
        }
    }
}
