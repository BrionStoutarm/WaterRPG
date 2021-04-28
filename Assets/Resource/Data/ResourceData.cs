using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ResourceData : UpdatableData {
    public string resourceName;
    public int amount;
    public float minSpawnHeight;
    public float maxSpawnHeight;
    public Material resourceMaterial;
    public Transform model;
}
