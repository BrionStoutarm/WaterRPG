using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPlacedObject : PlacedObject
{
    public static BuildingPlacedObject CreateBuilding(Vector3 worldPosition, Vector2Int origin, PlaceableScriptableObject.Dir dir, BuildingPlaceableScriptableObject placeableType, int cellScale) {
        Transform placedObjectTransform = Instantiate(placeableType.prefab, worldPosition, Quaternion.Euler(0, placeableType.GetRotationAngle(dir), 0));

        BuildingPlacedObject placedObject = placedObjectTransform.GetComponent<BuildingPlacedObject>();

        placedObject.placeableType = placeableType;
        placedObject.origin = origin;
        placedObject.dir = dir;
        placedObject.originalScale = placeableType.prefab.localScale;
        placedObject.cellScale = cellScale;
        placedObject.BindActions();

        return placedObject;
    }

    public BuildingPlaceableScriptableObject GetBuildingType() {
        return placeableType as BuildingPlaceableScriptableObject;
    }
}
