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

    public Transform playerStartLoc;
    public Transform viewer;

    void Start() {
        terrainGenerator = FindObjectOfType<TerrainGenerator>();
        GenerateLevel(playerStartLoc, transform, viewer);
    }

   //will take more params
    public Level GenerateLevel(Transform playerStart, Transform parent, Transform viewer) {
        TerrainChunk terrainChunk = terrainGenerator.GenerateTerrainChunk(); //pass in biome and other area info to affect the terrain generated
        Level newLevel = new Level(playerStart, terrainChunk);
        return newLevel;
    }
}
