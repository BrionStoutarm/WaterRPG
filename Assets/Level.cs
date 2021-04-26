using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level 
{
    TerrainChunk terrainChunk;
    //public ResourceMap resourceMap;
    Transform playerStartPosition;

    public Level (Transform playerStartPosition, TerrainChunk terrainChunk) {
        this.playerStartPosition = playerStartPosition;
        this.terrainChunk = terrainChunk;
        terrainChunk.Load();
     
    }
}
