using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    //public TextureSettings textureSettings;
    //public HeightMapSettings heightMapSettings;
    //public MeshSettings meshSettings;
    public MapSettings mapSettings;
    public Transform playerStart; // need to refactor this later to be an area
    public Transform viewer;

    Level currentLevel;

    void Start() {
    }
}
