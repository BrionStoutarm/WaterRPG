using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap : MonoBehaviour
{
    public int size_x, size_y; //number of terrain chunks
    public TerrainGenerator terrainGenerator;
    public MeshSettings meshSettings;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < size_x; i++) {
            for(int j = 0; j < size_y; j++) {
                terrainGenerator.GenerateTerrainChunk(new Vector2(i , j));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateWorld() {
        for (int i = 0; i < size_x; i++) {
            for (int j = 0; j < size_y; j++) {
                terrainGenerator.GenerateTerrainChunk(new Vector2(i, j));
            }
        }
    }
}
