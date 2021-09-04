using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, PlaceableScriptableObject.Dir dir, PlaceableScriptableObject placeableType, int cellScale) {
        Transform placedObjectTransform = Instantiate(placeableType.prefab, worldPosition, Quaternion.Euler(0, placeableType.GetRotationAngle(dir), 0));

        PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();

        placedObject.placeableType = placeableType;
        placedObject.origin = origin;
        placedObject.dir = dir;
        placedObject.originalScale = placeableType.prefab.localScale;
        placedObject.cellScale = cellScale;
        placedObject.BindActions();

        return placedObject;
    }

    private PlaceableScriptableObject placeableType;
    private Vector2Int origin;
    private PlaceableScriptableObject.Dir dir;
    private Vector3 originalScale;
    private int cellScale; //grid sizes are gonna be 8x10, 16x20, etc.  cellScale will just be 1, 2, etc to correspond with grid scale
    public List<PlaceableAction> ActionList { get; private set; }

    public List<Vector2Int> GetGridPositionList() {
        return placeableType.GetGridPositionList(origin, dir, cellScale);
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

    public void SetVisible(bool isVisible) {
        if(isVisible) {
            gameObject.transform.localScale = originalScale;
        }
        else {
            gameObject.transform.localScale = Vector3.zero;
        }
    }

    public string GetObjectName() {
        return placeableType.nameString;
    }

    private void BindActions() {
        //very silly but i wanted to get something working
        switch (placeableType.nameString) {
            case "BasicBuilding":
                BindBasicBuildingActions();
                break;
        }

    }

    private void BindBasicBuildingActions() {
        ActionList = new List<PlaceableAction>();

        BasicBuildingAction firstAction = new BasicBuildingAction(this, "First Action");
        ActionList.Add(firstAction);

        BasicBuildingAction secondAction = new BasicBuildingAction(this, "Second Action");
        ActionList.Add(secondAction);
    }
}
