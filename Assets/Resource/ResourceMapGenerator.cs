using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ResourceMapGenerator : MonoBehaviour
{
    public ResourceMapSettings mapSettings;
    public Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopulateTerrainWithResources(TerrainChunk terrainChunk) {
        HeightMap heightMap = terrainChunk.GetHeightMap();
        int size = terrainChunk.size;

        Mesh mesh = terrainChunk.mesh;
        foreach(Vector3 vert in mesh.vertices) {
            float height = vert.y;
            for (int r = 0; r < mapSettings.availableResources.Length; r++) {
                if (height >= mapSettings.availableResources[r].MinSpawnHeight && height <= mapSettings.availableResources[r].MaxSpawnHeight) {
                    if (Random.Range(0f, 10f) % mapSettings.resourceAvailability > 0) {
                        SpawnResource(vert , mapSettings.availableResources[r]);
                    }
                }
            }
        }

        //for(int i = 0; i < size; i++) {
        //    for(int j = 0; j < size; j++) {
        //        float height = heightMap.values[i, j];
        //        for(int r = 0; r < mapSettings.availableResources.Length; r++) {
        //            if(height >= mapSettings.availableResources[r].MinSpawnHeight && height <= mapSettings.availableResources[r].MaxSpawnHeight) {
        //                if(Random.Range(0f, 10f) % mapSettings.resourceAvailability > 0) {
        //                    SpawnResource(new Vector3(i, height, j) * terrainChunk.meshScale, mapSettings.availableResources[r]);
        //                }
        //            }
        //        }
        //    }
        //}
    }

    void SpawnResource(Vector3 position, Resource resource) {
        Instantiate(resource, position, Quaternion.identity, parent);
    }
}
