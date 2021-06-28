﻿using UnityEngine;

public class TerrainChunk {

	const float colliderGenerationDistanceThreshold = 5;
	//public event System.Action<TerrainChunk, bool> onVisibilityChanged;
	public Vector2 coord;

	GameObject meshObject;
	Vector2 sampleCentre;
	Bounds bounds;

	MeshRenderer meshRenderer;
	MeshFilter meshFilter;
	MeshCollider meshCollider;

	LODInfo[] detailLevels;
	LODMesh[] lodMeshes;
	int colliderLODIndex;

	HeightMap heightMap;
	bool heightMapReceived;
	int previousLODIndex = -1;
	bool hasSetCollider;
	float maxViewDst;

	HeightMapSettings heightMapSettings;
	MeshSettings meshSettings;
	Transform viewer;

	public TerrainChunk(Vector2 coord, MapSettings mapSettings, Transform parent, Transform viewer) {
		this.coord = coord;
		this.detailLevels = mapSettings.detailLevels;
		this.heightMapSettings = mapSettings.heightMapSettings;
		this.meshSettings = mapSettings.meshSettings;
		this.viewer = viewer;

		 
		sampleCentre = coord * meshSettings.meshWorldSize / meshSettings.meshScale;
		Vector2 position = coord * meshSettings.meshWorldSize;
		bounds = new Bounds(position, Vector2.one * meshSettings.meshWorldSize);


		meshObject = new GameObject("Terrain Chunk");
		meshRenderer = meshObject.AddComponent<MeshRenderer>();
		meshFilter = meshObject.AddComponent<MeshFilter>();
		meshCollider = meshObject.AddComponent<MeshCollider>();
		meshRenderer.material = mapSettings.mapMaterial;

		meshObject.transform.position = new Vector3(position.x, 0, position.y);
		meshObject.transform.parent = parent;

		lodMeshes = new LODMesh[detailLevels.Length];
		for (int i = 0; i < detailLevels.Length; i++) {
			lodMeshes[i] = new LODMesh(detailLevels[i].lod);
			lodMeshes[i].updateCallback += UpdateTerrainChunk;
			if (i == colliderLODIndex) {
				lodMeshes[i].updateCallback += UpdateCollisionMesh;
			}
		}

		maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
	}

	public void Load() {
		//ThreadedDataRequester.RequestData(() => HeightMapGenerator.GenerateHeightMap(meshSettings.numVerticesPerLine, meshSettings.numVerticesPerLine, heightMapSettings, sampleCentre), OnHeightMapReceived);
		this.heightMap = HeightMapGenerator.GenerateHeightMap(meshSettings.numVerticesPerLine, meshSettings.numVerticesPerLine, heightMapSettings, sampleCentre);
		meshFilter.mesh = lodMeshes[0].GetMesh(heightMap, meshSettings);
		UpdateCollisionMesh();
		//OnHeightMapReceived();
		Debug.Log("Terrain Chunk loaded");
	}

	public Mesh mesh {
		get {
			return meshFilter.mesh;
		}
	}

	public int size {
		get {
			return meshSettings.numVerticesPerLine;
		}
	}
	
	public float meshScale {
		get {
			return meshSettings.meshScale;
		}
	}

	void OnHeightMapReceived(object heightMapObject) {
		this.heightMap = (HeightMap)heightMapObject;
		heightMapReceived = true;

		UpdateTerrainChunk();
	}

	Vector2 viewerPosition {
		get {
			return new Vector2(viewer.position.x, viewer.position.z);
		}
	}


	//Will need to refactor this i think
	public void UpdateTerrainChunk() {
		if (heightMapReceived) {
			Debug.Log("update terrain chunk");
			float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));

			bool visible = true;

			if (visible) {
				int lodIndex = 0;

				for (int i = 0; i < detailLevels.Length - 1; i++) {
					if (viewerDstFromNearestEdge > detailLevels[i].visibleDstThreshold) {
						lodIndex = i + 1;
					}
					else {
						break;
					}
				}

				if (lodIndex != previousLODIndex) {
					LODMesh lodMesh = lodMeshes[lodIndex];
					if (lodMesh.hasMesh) {
						previousLODIndex = lodIndex;
						meshFilter.mesh = lodMesh.mesh;
					}
					else if (!lodMesh.hasRequestedMesh) {
						lodMesh.RequestMesh(heightMap, meshSettings);
					}
				}


			}

			//if (wasVisible != visible) {

			//	SetVisible(visible);
			//	//if (onVisibilityChanged != null) {
			//	//	onVisibilityChanged(this, visible);
			//	//}
			//}
		}
	}

	public void UpdateCollisionMesh() {
		if (!hasSetCollider) {
			meshCollider.sharedMesh = meshFilter.mesh;
			hasSetCollider = true;
			//float sqrDstFromViewerToEdge = bounds.SqrDistance(viewerPosition);

			//if (sqrDstFromViewerToEdge < detailLevels[colliderLODIndex].sqrVisibleDstThreshold) {
			//	if (!lodMeshes[colliderLODIndex].hasRequestedMesh) {
			//		lodMeshes[colliderLODIndex].RequestMesh(heightMap, meshSettings);
			//	}
			//}

			//if (sqrDstFromViewerToEdge < colliderGenerationDistanceThreshold * colliderGenerationDistanceThreshold) {
			//	if (lodMeshes[colliderLODIndex].hasMesh) {
			//		meshCollider.sharedMesh = lodMeshes[colliderLODIndex].mesh;
			//		hasSetCollider = true;
			//	}
			//}
		}
	}

	public void SetVisible(bool visible) {
		meshObject.SetActive(visible);
	}

	public bool IsVisible() {
		return meshObject.activeSelf;
	}

	public HeightMap GetHeightMap() {
		return heightMap;
	}

}

class LODMesh {

	public Mesh mesh;
	public bool hasRequestedMesh;
	public bool hasMesh;
	int lod;
	public event System.Action updateCallback;

	public LODMesh(int lod) {
		this.lod = lod;
	}

	void OnMeshDataReceived(object meshDataObject) {
		mesh = ((MeshData)meshDataObject).CreateMesh();
		hasMesh = true;

		updateCallback();
	}

	public void RequestMesh(HeightMap heightMap, MeshSettings meshSettings) {
		hasRequestedMesh = true;
		ThreadedDataRequester.RequestData(() => MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, lod), OnMeshDataReceived);
	}

	public Mesh GetMesh(HeightMap heightMap, MeshSettings meshSettings) {
		Mesh newMesh = MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, lod).CreateMesh();
		return newMesh;
	}

}

[System.Serializable]
public struct MapSettings {
	public HeightMapSettings heightMapSettings;
	public MeshSettings meshSettings;
	public Material mapMaterial;
	public LODInfo[] detailLevels;

	public MapSettings(HeightMapSettings heightMapSettings, MeshSettings meshSettings, Material mapMaterial, LODInfo[] detailLevels) {
		this.heightMapSettings = heightMapSettings;
		this.meshSettings = meshSettings;
		this.mapMaterial = mapMaterial;
		this.detailLevels = detailLevels;
	}
}