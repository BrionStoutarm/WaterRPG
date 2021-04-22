using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    const float viewerMoveThresholdForChunkUpdate = 25f;
    const float sqrViewerMoveThresholdForChunkUpdate = viewerMoveThresholdForChunkUpdate * viewerMoveThresholdForChunkUpdate;
    const float colliderGenerationDistanceThreshold = 5f;

    public int colliderLODIndex;
    public LODInfo[] detailLevels;
    public static float maxViewDistance;

    public Transform viewer;
    public Material mapMaterial;

    public static Vector2 viewPosition;
    Vector2 viewerPositionOld;
    static MapGenerator mapGenerator;
    int chunkSize;
    int chunksVisibleInViewDistance;

    Dictionary<Vector2, TerrainChunk> terrainChunkDict = new Dictionary<Vector2, TerrainChunk>();
    static List<TerrainChunk> visibleTerrainChunks = new List<TerrainChunk>();

    // Start is called before the first frame update
    void Start() {
        mapGenerator = FindObjectOfType<MapGenerator>();

        maxViewDistance = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
        chunkSize = mapGenerator.mapChunkSize - 1;
        chunksVisibleInViewDistance = Mathf.RoundToInt(maxViewDistance / chunkSize);

        UpdateVisibleChunks();
    }

    void Update() {
        viewPosition = new Vector2(viewer.position.x, viewer.position.z) / mapGenerator.terrainData.uniformScale;

        if(viewPosition != viewerPositionOld) {
            foreach(TerrainChunk chunk in visibleTerrainChunks) {
                chunk.UpdateCollisionMesh();
            }
        }

        if((viewerPositionOld - viewPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate) {
            viewerPositionOld = viewPosition;
            UpdateVisibleChunks();
        }
    }

    void UpdateVisibleChunks() {
        HashSet<Vector2> alreadyUpdatedChunkCoords = new HashSet<Vector2>();
        for(int i = visibleTerrainChunks.Count - 1; i >= 0; i--) {
            visibleTerrainChunks[i].UpdateTerrainChunk();
            alreadyUpdatedChunkCoords.Add(visibleTerrainChunks[i].coord);
        }

        int currentChunkCoordX = Mathf.RoundToInt(viewPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewPosition.y / chunkSize);

        for(int yOffset = - chunksVisibleInViewDistance; yOffset <= chunksVisibleInViewDistance; yOffset++) {
            for(int xOffset = -chunksVisibleInViewDistance; xOffset <= chunksVisibleInViewDistance; xOffset++) {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (!alreadyUpdatedChunkCoords.Contains(viewedChunkCoord)) {
                    if(terrainChunkDict.ContainsKey(viewedChunkCoord)) {
                        terrainChunkDict[viewedChunkCoord].UpdateTerrainChunk();
                    }
                    else {
                        terrainChunkDict.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord, chunkSize, detailLevels, colliderLODIndex, transform, mapMaterial));
                    }
                }
            }
        }

    }

    public class TerrainChunk {

        public Vector2 coord;

        GameObject meshObject;
        Vector2 position;
        Bounds bounds;

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        MeshCollider meshCollider;

        LODInfo[] detailLevels;
        LODMesh[] lodMeshes;
        int colliderLODIndex;

        MapData mapData;
        bool mapDataRecieved;
        int previousLODIndex = -1;
        bool hasSetCollider;

        public TerrainChunk(Vector2 coord, int size, LODInfo[] detailLevels, int colliderLODIndex, Transform parent, Material material) {
            this.coord = coord;
            this.detailLevels = detailLevels;
            this.colliderLODIndex = colliderLODIndex;
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshCollider = meshObject.AddComponent<MeshCollider>();
            meshRenderer.material = material;

            meshObject.transform.position = positionV3 * mapGenerator.terrainData.uniformScale;
            meshObject.transform.parent = parent;
            meshObject.transform.localScale = Vector3.one * mapGenerator.terrainData.uniformScale;
            SetVisible(false);

            lodMeshes = new LODMesh[detailLevels.Length];
            for(int i = 0; i < detailLevels.Length; i++) {
                lodMeshes[i] = new LODMesh(detailLevels[i].lod);
                lodMeshes[i].updateCallback += UpdateTerrainChunk;
                if(i == colliderLODIndex) {
                    lodMeshes[i].updateCallback += UpdateCollisionMesh;
                }
            }

            mapGenerator.RequestMapData(position, OnMapDataRecieved);
        }

        void OnMapDataRecieved(MapData mapData) {
            this.mapData = mapData;
            mapDataRecieved = true;

            UpdateTerrainChunk();
        }

        public void UpdateTerrainChunk() {
            if(mapDataRecieved) {
                float viewDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewPosition));

                bool wasVisible = IsVisible();
                bool visible = viewDstFromNearestEdge <= maxViewDistance;

                if(visible) {
                    int lodIndex = 0;
                    for(int i = 0; i < detailLevels.Length - 1; i++) {
                        if(viewDstFromNearestEdge > detailLevels[i].visibleDstThreshold) {
                            lodIndex = i + 1;
                        } 
                        else {
                            break;
                        }
                    }

                    if(lodIndex != previousLODIndex) {
                        LODMesh lodMesh = lodMeshes[lodIndex];
                        if(lodMesh.hasMesh) {
                            previousLODIndex = lodIndex;
                            meshFilter.mesh = lodMesh.mesh;
                        }
                        else if (!lodMesh.hasRequestedMesh) {
                            lodMesh.RequestMesh(mapData);
                        }
                    }

                    if(wasVisible != visible) {
                        if(visible) {
                            visibleTerrainChunks.Add(this);
                        }
                        else {
                            visibleTerrainChunks.Remove(this);
                        }
                        SetVisible(visible);
                    }

                    visibleTerrainChunks.Add(this);
                }

                SetVisible(visible);
            }
        }

        public void UpdateCollisionMesh() {
            if(!hasSetCollider) {
                float sqrDistFromViewerToEdge = bounds.SqrDistance(viewPosition);

                if(sqrDistFromViewerToEdge < detailLevels[colliderLODIndex].sqrVisibleDstThreshold) {
                    if (!lodMeshes[colliderLODIndex].hasRequestedMesh) {
                        lodMeshes[colliderLODIndex].RequestMesh(mapData);
                    }
                }

                if(sqrDistFromViewerToEdge < colliderGenerationDistanceThreshold * colliderGenerationDistanceThreshold) {
                    if(lodMeshes[colliderLODIndex].hasMesh) { 
                        meshCollider.sharedMesh = lodMeshes[colliderLODIndex].mesh;
                        hasSetCollider = true;
                    }
                }
            }
        }

        public void SetVisible(bool visible) {
            meshObject.SetActive(visible);
        }

        public bool IsVisible() {
            return meshObject.activeSelf;
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

        void OnMeshDataRecieved(MeshData meshData) {
            mesh = meshData.CreateMesh();
            hasMesh = true;

            updateCallback();
        }

        public void RequestMesh(MapData mapData) {
            hasRequestedMesh = true;
            mapGenerator.RequestMeshData(mapData, lod, OnMeshDataRecieved);
        }
    }

    [System.Serializable]
    public struct LODInfo {
        [Range(0, MeshGenerator.numSupportedLODs - 1)]
        public int lod;
        public float visibleDstThreshold;

        public float sqrVisibleDstThreshold {
            get {
                return visibleDstThreshold * visibleDstThreshold;
            }
        }
    }
}
