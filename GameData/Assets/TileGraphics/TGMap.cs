using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TGMap : MonoBehaviour
{
    public int size_x = 100;
    public int size_z = 50;
    public float tileSize = 1.0f;

    public Texture2D terrainTiles;
    public int tileResolution;

    // Start is called before the first frame update
    void Start() {
        //BuildMesh();
    }


    //public void BuildTexture() {
    //    int numTilesPerRow = terrainTiles.width / tileResolution;
    //    int numRows = terrainTiles.height / tileResolution;

    //    int texWidth = size_x * tileResolution;
    //    int texHeight = size_z * tileResolution;
    //    Texture2D texture = new Texture2D(texWidth, texHeight);

    //    for (int y = 0; y < size_z; y++) {
    //        for (int x = 0; x < size_x; x++) {
    //            int terrainTileOffeset = Random.Range(0, 4) * tileResolution;
    //            Color[] p = terrainTiles.GetPixels(terrainTileOffeset, 0, tileResolution, tileResolution);
    //            texture.SetPixels(x * tileResolution, y * tileResolution, tileResolution, tileResolution, p);
    //        }
    //    }
    //    texture.filterMode = FilterMode.Point;
    //    texture.Apply();

    //    MeshRenderer mr = GetComponent<MeshRenderer>();
    //    mr.sharedMaterials[0].mainTexture = texture;

    //    Debug.Log("Texture Done!");
    //}

    public void BuildMesh() {
        int numTiles = size_x * size_z;
        int numTris = numTiles * 2;
        int vsize_x = size_x + 1;
        int vsize_z = size_z + 1;
        int numVerts = vsize_x * vsize_z;

        //Generate mesh data
        Vector3[] vertices = new Vector3[numVerts];
        Vector3[] normals = new Vector3[numVerts];
        Vector2[] uv = new Vector2[numVerts];

        int[] triangles = new int[numTris * 3];
        int x, z;
        for (z = 0; z < vsize_z; z++) {
            for (x = 0; x < vsize_x; x++) {
                vertices[z * vsize_x + x] = new Vector3(x * tileSize, 0, z * tileSize);
                normals[z * vsize_x + x] = Vector3.up;
                uv[z * vsize_x + x] = new Vector2((float)x / vsize_x, (float)z / vsize_z);
            }
        }

        for (z = 0; z < size_z; z++) {
            for (x = 0; x < size_x; x++) {
                int squareIndex = z * size_x + x;
                int triOffset = squareIndex * 6;

                triangles[triOffset + 0] = z * vsize_x + x + 0;
                triangles[triOffset + 2] = z * vsize_x + x + vsize_x + 1;
                triangles[triOffset + 1] = z * vsize_x + x + vsize_x + 0;

                triangles[triOffset + 3] = z * vsize_x + x + 0;
                triangles[triOffset + 5] = z * vsize_x + x + 1;
                triangles[triOffset + 4] = z * vsize_x + x + vsize_x + 1;
            }
        }

        //Create a new mesh and populate with data
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        //Assign mesh to objects
        MeshFilter mf = GetComponent<MeshFilter>();
        MeshRenderer mr = GetComponent<MeshRenderer>();
        MeshCollider mc = GetComponent<MeshCollider>();

        mc.sharedMesh = mesh;
        mf.mesh = mesh;

        Debug.Log("Mesh Done!");
        BuildTexture();
    }
}
