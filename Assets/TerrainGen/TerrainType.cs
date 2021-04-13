using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Code from: https://gamedevacademy.org/complete-guide-to-procedural-level-generation-in-unity-part-1/

[System.Serializable]
public class TerrainType {
	public string name;
	public float height;
	public Color color;
}

public class TerrainGeneration : MonoBehaviour {

	[SerializeField]
	private TerrainType[] terrainTypes;

}
