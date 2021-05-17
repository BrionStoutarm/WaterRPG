using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(Mesh))]
public class Waves : MonoBehaviour
{
    public int dimension = 10;
    public Octave[] octaves;
    public float UVScale = 1f;

    protected MeshFilter meshFilter;
    protected Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        //Mesh Setup
        mesh = new Mesh();
        mesh.name = gameObject.name;

        mesh.vertices = GenerateVertices();
        mesh.triangles = GenerateTriangles();
        mesh.uv = GenerateUVs();
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        
    }

    public float GetHeightAtPosition(Vector3 position) {
        //scale factor and position in local space
        Vector3 scale = new Vector3(1 / transform.lossyScale.x, 0, 1 / transform.lossyScale.z);
        Vector3 localPos = Vector3.Scale((position - transform.position), scale);

        //get edge points
        Vector3 p1 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Floor(localPos.z));
        Vector3 p2 = new Vector3(Mathf.Floor(localPos.x), 0, Mathf.Ceil(localPos.z));
        Vector3 p3 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Floor(localPos.z));
        Vector3 p4 = new Vector3(Mathf.Ceil(localPos.x), 0, Mathf.Ceil(localPos.z));

        //clamp if the position is outside the plane
        p1.x = Mathf.Clamp(p1.x, 0, dimension);
        p1.z = Mathf.Clamp(p1.z, 0, dimension);
        p2.x = Mathf.Clamp(p2.x, 0, dimension);
        p2.z = Mathf.Clamp(p2.z, 0, dimension);
        p3.x = Mathf.Clamp(p3.x, 0, dimension);
        p3.z = Mathf.Clamp(p3.z, 0, dimension);
        p4.x = Mathf.Clamp(p4.x, 0, dimension);
        p4.z = Mathf.Clamp(p4.z, 0, dimension);

        //get the max distance to one of the edges and take that to compue max - dist
        float max = Mathf.Max(Vector3.Distance(p1, localPos), Vector3.Distance(p2, localPos), Vector3.Distance(p3, localPos), Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        float dist = (max - Vector3.Distance(p1, localPos))
                   + (max - Vector3.Distance(p2, localPos))
                   + (max - Vector3.Distance(p3, localPos))
                   + (max - Vector3.Distance(p4, localPos) + Mathf.Epsilon);
        //weighted sum
        float height = mesh.vertices[index((int)p1.x, (int)p1.z)].y * (max - Vector3.Distance(p1, localPos))
                     + mesh.vertices[index((int)p2.x, (int)p2.z)].y * (max - Vector3.Distance(p2, localPos))
                     + mesh.vertices[index((int)p3.x, (int)p3.z)].y * (max - Vector3.Distance(p3, localPos))
                     + mesh.vertices[index((int)p4.x, (int)p4.z)].y * (max - Vector3.Distance(p4, localPos));
        //scale
        return height * transform.lossyScale.y / dist;
    }

    private Vector3[] GenerateVertices() {
        Vector3[] verts = new Vector3[(dimension + 1) * (dimension + 1)];

        for(int x = 0; x <= dimension; x++) {
            for(int z = 0; z <= dimension; z++) {
                verts[index(x, z)] = new Vector3(x, 0, z);
            }
        }

        return verts;
    }

    private int[] GenerateTriangles() {
        int[] triangles = new int[mesh.vertices.Length * 6];

        for(int x = 0; x < dimension; x++) {
            for(int z = 0; z < dimension; z++) {
                triangles[index(x, z) * 6 + 0] = index(x, z);
                triangles[index(x, z) * 6 + 1] = index(x + 1, z + 1);
                triangles[index(x, z) * 6 + 2] = index(x + 1, z);
                triangles[index(x, z) * 6 + 3] = index(x, z);
                triangles[index(x, z) * 6 + 4] = index(x, z + 1);
                triangles[index(x, z) * 6 + 5] = index(x + 1, z + 1);
            }
        }

        return triangles;
    }

    private Vector2[] GenerateUVs() {
        Vector2[] uvs = new Vector2[mesh.vertices.Length];

        for(int x = 0; x <= dimension; x++) {
            for(int z = 0; z <= dimension; z++) {
                Vector2 vec = new Vector2((x / UVScale) % 2, (z / UVScale) % 2);
                uvs[index(x, z)] = new Vector2(vec.x <= 1 ? vec.x : 2 - vec.x, vec.y <= 1 ? vec.y : 2 - vec.y);
            }
        }

        return uvs;
    }


    private int index(int x, int z) {
        return x * (dimension + 1) + z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] vertices = mesh.vertices;
        for(int x = 0; x <= dimension; x++) {
            for(int z = 0; z <= dimension; z++) {
                float height = 0f;

                for(int o = 0; o < octaves.Length; o++) {
                    if(octaves[o].alternate) {
                        float perl = Mathf.PerlinNoise((x * octaves[o].scale.x) / dimension, (z * octaves[o].scale.y) / dimension) * Mathf.PI * 2f;
                        height += Mathf.Cos(perl + octaves[o].speed.magnitude * Time.time) * octaves[o].height;
                    }
                    else {
                        float perl = Mathf.PerlinNoise((x * octaves[o].scale.x + Time.time * octaves[o].speed.x) / dimension, (z * octaves[o].scale.y + Time.time * octaves[o].speed.y) / dimension) - 0.5f;
                        height += perl * octaves[o].height;
                    }
                }

                vertices[index(x, z)] = new Vector3(x, height, z);
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }

    [Serializable]
    public struct Octave {
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }
}
