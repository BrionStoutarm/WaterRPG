using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, BuildingScriptableObject.Dir dir, BuildingScriptableObject buildingType) {
        Transform placedObjectTransform = Instantiate(buildingType.prefab, worldPosition, Quaternion.Euler(0, buildingType.GetRotationAngle(dir), 0));

        PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();

        placedObject.buildingType = buildingType;
        placedObject.origin = origin;
        placedObject.dir = dir;

        return placedObject;
    }

    private BuildingScriptableObject buildingType;
    private Vector2Int origin;
    private BuildingScriptableObject.Dir dir;

    public List<Vector2Int> GetGridPositionList() {
        return buildingType.GetGridPositionList(origin, dir);
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }
}
