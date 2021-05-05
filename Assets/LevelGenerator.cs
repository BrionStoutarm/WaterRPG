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
    Vector3 playerStartLoc;

    public GameObject playerBoat;
    public Transform viewer;

    TerrainChunk terrainChunk;

    void Start() {
        terrainGenerator = FindObjectOfType<TerrainGenerator>();
        resourceMapGenerator = FindObjectOfType<ResourceMapGenerator>();
        GenerateLevel(transform, viewer);
        SpawnPlayer();
    }

    void Update() {
    }

    //Refactor later on to have a start area where player could choose how to deploy available ships/units
    //For now just spawn the placeholder boat at the startLoc
    void SpawnPlayer() {
        playerStartLoc = new Vector3(terrainChunk.size / 2, 0, terrainChunk.size / 2);
        Instantiate(playerBoat, playerStartLoc, Quaternion.identity);
    }

    //will take more params
    public Level GenerateLevel(Transform parent, Transform viewer) {
        terrainChunk = terrainGenerator.GenerateTerrainChunk(); //pass in biome and other area info to affect the terrain generated
        Debug.Log("Terrain Chunk done!");
        resourceMapGenerator.PopulateTerrainWithResources(terrainChunk);//^^
        Debug.Log("Populating Resources done!");
        Level newLevel = new Level(terrainChunk);
        return newLevel;
    }
}
