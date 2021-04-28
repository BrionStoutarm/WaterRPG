using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Resource : MonoBehaviour{

    public ResourceData resourceData;

    void Start() {
        MeshRenderer meshRenderer = FindObjectOfType<MeshRenderer>();
        meshRenderer.material = resourceData.resourceMaterial;
    }

    void Update() {
        
    }

    public float MinSpawnHeight {
        get {
            return resourceData.minSpawnHeight;
        }
    }
    public float MaxSpawnHeight {
        get {
            return resourceData.maxSpawnHeight;
        }
    }
}

