using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level 
{
    TerrainChunk terrainChunk;
    //public ResourceMap resourceMap;

    public Level (TerrainChunk terrainChunk) {
        this.terrainChunk = terrainChunk;
        terrainChunk.Load();
     
    }
}
