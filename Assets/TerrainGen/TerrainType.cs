using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
