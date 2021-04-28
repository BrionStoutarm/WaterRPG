using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* where we generate the data for the level
 * - Enemies 
 *      - Locations
 *      - Types
 * - Resources
 *      - Locations
 *      - Types
 *      - Amounts
 * 
 * 
*/
public class LevelGenerator : MonoBehaviour
{
    TerrainGenerator terrainGenerator;
    ResourceMapGenerator resourceMapGenerator;

    public Transform playerStartLoc;
    public Transform viewer;

    TerrainChunk terrainChunk;

    void Start() {
        terrainGenerator = FindObjectOfType<TerrainGenerator>();
        resourceMapGenerator = FindObjectOfType<ResourceMapGenerator>();
        GenerateLevel(playerStartLoc, transform, viewer);
    }

    void Update() {
    }

    //will take more params
    public Level GenerateLevel(Transform playerStart, Transform parent, Transform viewer) {
        terrainChunk = terrainGenerator.GenerateTerrainChunk(); //pass in biome and other area info to affect the terrain generated
        Debug.Log("Terrain Chunk done!");
        resourceMapGenerator.PopulateTerrainWithResources(terrainChunk);//^^
        Debug.Log("Populating Resources done!");
        Level newLevel = new Level(playerStart, terrainChunk);
        return newLevel;
    }
}
