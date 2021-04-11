using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TileGeneration : MonoBehaviour
{
    [SerializeField]
    NoiseMapGenerator m_noiseGen;

    [SerializeField]
    private MeshRenderer m_tileRenderer;

    [SerializeField]
    private MeshFilter m_meshFilter;

    [SerializeField]
    private MeshCollider m_meshCollider;

    [SerializeField]
    private float mapScale;

    [SerializeField]
    private AnimationCurve heightCurve;

    [SerializeField]
    private Wave[] waves;

    [SerializeField]
    private float heightMultiplier;

    public TerrainType[] m_terrainTypes;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTile();
    }
    public void GenerateTile() {
        // calculate tile depth and width based on the mesh vertices
        Vector3[] meshVertices = m_meshFilter.mesh.vertices;
        int tileDepth = (int)Mathf.Sqrt(meshVertices.Length);
        int tileWidth = tileDepth;

        // calculate the offsets based on the tile position
        float offsetX = -gameObject.transform.position.x;
        float offsetZ = -gameObject.transform.position.z;

        // generate a heightMap using noise
        float[,] heightMap = m_noiseGen.GenerateNoiseMap(tileDepth, tileWidth, mapScale, offsetX, offsetZ, waves);

        // build a Texture2D from the height map
        Texture2D tileTexture = BuildTexture(heightMap);
        m_tileRenderer.material.mainTexture = tileTexture;

        // update the tile mesh vertices according to the height map
        UpdateMeshVertices(heightMap);
    }

    private Texture2D BuildTexture(float[,] heightMap) {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);

        Color[] colorMap = new Color[tileDepth * tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++) {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++) {
                // transform the 2D map index is an Array index
                int colorIndex = zIndex * tileWidth + xIndex;
                float height = heightMap[zIndex, xIndex];
                // choose a terrain type according to the height value
                TerrainType terrainType = ChooseTerrainType(height);
                // assign the color according to the terrain type
                colorMap[colorIndex] = terrainType.color;
            }
        }

        // create a new texture and set its pixel colors
        Texture2D tileTexture = new Texture2D(tileWidth, tileDepth);
        tileTexture.wrapMode = TextureWrapMode.Clamp;
        tileTexture.SetPixels(colorMap);
        tileTexture.Apply();

        return tileTexture;
    }

    TerrainType ChooseTerrainType(float height) {
        // for each terrain type, check if the height is lower than the one for the terrain type
        foreach (TerrainType terrainType in m_terrainTypes) {
            // return the first terrain type whose height is higher than the generated one
            if (height < terrainType.height) {
                return terrainType;
            }
        }
        return m_terrainTypes[m_terrainTypes.Length - 1];
    }

    private void UpdateMeshVertices(float[,] heightMap) {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);

        Vector3[] meshVertices = m_meshFilter.mesh.vertices;

        // iterate through all the heightMap coordinates, updating the vertex index
        int vertexIndex = 0;
        for (int zIndex = 0; zIndex < tileDepth; zIndex++) {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++) {
                float height = heightMap[zIndex, xIndex];

                Vector3 vertex = meshVertices[vertexIndex];

                // change the vertex Y coordinate, proportional to the height value. The height value is evaluated by the heightCurve function, in order to correct it.
                meshVertices[vertexIndex] = new Vector3(vertex.x, heightCurve.Evaluate(height) * heightMultiplier, vertex.z);

                vertexIndex++;
            }
        }

        // update the vertices in the mesh and update its properties
        m_meshFilter.mesh.vertices = meshVertices;
        m_meshFilter.mesh.RecalculateBounds();
        m_meshFilter.mesh.RecalculateNormals();
        // update the mesh collider
        m_meshCollider.sharedMesh = m_meshFilter.mesh;
    }
}

