using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] private List<BuildingScriptableObject> buildingTypeList;
    private BuildingScriptableObject currentPlaceBuilding;

    private Grid<GridObject> grid;
    private BuildingScriptableObject.Dir dir = BuildingScriptableObject.Dir.Down;

    public GameManager gameManager;

    public static GridBuildingSystem Instance;
    public event EventHandler<OnSelectedChangedEventArgs> OnSelectedChanged;
    public class OnSelectedChangedEventArgs : EventArgs {
    }

    public Vector3 GetMouseWorldSnappedPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
            Vector3 hitPoint = hit.point;
            grid.GetXZ(hitPoint, out int x, out int z);

            Vector2Int rotationOffset = currentPlaceBuilding.GetRotationOffset(dir);
            Vector3 placeObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

            return placeObjectWorldPosition;
        }
        return Vector3.zero;
    }

    public BuildingScriptableObject GetPlacedObjectType() {
        return currentPlaceBuilding;
    }

    public void SetActiveGrid(Grid<GridObject> grid) {
        this.grid = grid;
    }

    public void SetDeckVisible(bool isVisible, Grid<GridObject> grid) {
        for (int i = 0; i < grid.Width(); i++) {
            for (int j = 0; j < grid.Height(); j++) {
                grid.GetGridObject(i, j).SetVisible(isVisible);
            }
        }
    }


    public Quaternion GetPlacedObjectRotation() {
        return Quaternion.Euler(0, currentPlaceBuilding.GetRotationAngle(dir), 0);
    }

    private void Awake() {
        currentPlaceBuilding = buildingTypeList[0];

        Instance = this;
        PlayerInput.OnLeftClickEvent += (object sender, PlayerInput.OnLeftClickArgs eventArgs) => {
            HandleLeftClick(eventArgs.worldPosition);
        };
    }

    private void Instance_OnLeftClickEvent(object sender, PlayerInput.OnLeftClickArgs e) {
        Instance.HandleLeftClick(e.worldPosition);
    }

    private void HandleLeftClick(Vector3 mousePosition) {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
            Debug.Log(hit.point);
            Vector3 hitPoint = hit.point;
            hitPoint.y = 0f;

            grid.GetXZ(hitPoint, out int x, out int z);

            List<Vector2Int> gridPositionList = currentPlaceBuilding.GetGridPositionList(new Vector2Int(x, z), dir);


            //Test can build 
            bool canBuild = true;
            foreach (Vector2Int gridPosition in gridPositionList) {
                if (gridPosition.x >= 0 && gridPosition.y >= 0 && gridPosition.x < grid.Width() && gridPosition.y < grid.Height()) {
                    if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                        //cannot build here
                        canBuild = false;
                        break;
                    }
                }
                else {
                    canBuild = false;
                }
            }

            GridObject gridObject = grid.GetGridObject(x, z);
            if (canBuild) {
                Vector2Int rotationOffset = currentPlaceBuilding.GetRotationOffset(dir);
                Vector3 placeObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

                PlacedObject placedObject = PlacedObject.Create(placeObjectWorldPosition, new Vector2Int(x, z), dir, currentPlaceBuilding);

                foreach (Vector2Int gridPosition in gridPositionList) {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                }
            }
            else {
                //StaticFunctions.CreateWorldTextPopup("Cannot build here!", hitPoint);
                Debug.Log("Cannot build here");
            }
        }
    }

    private void Update() {
        

        //demolish building
        if(Input.GetMouseButton(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
                Vector3 hitPoint = hit.point;
                grid.GetXZ(hitPoint, out int x, out int z);

                GridObject gridObject = grid.GetGridObject(x,z);
                PlacedObject placedObject = gridObject.GetPlacedObject();
                if(placedObject != null) {
                    placedObject.DestroySelf();

                    List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();

                    foreach (Vector2Int gridPosition in gridPositionList) {
                        grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                    }
                }
            }
        }

        //Rotate current placing building
        if (Input.GetKeyDown(KeyCode.R)) {
            dir = BuildingScriptableObject.GetNextDir(dir);
            Debug.Log(dir);
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) { 
            currentPlaceBuilding = buildingTypeList[0]; 
            if (OnSelectedChanged != null) OnSelectedChanged(this, new OnSelectedChangedEventArgs { });
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { 
            currentPlaceBuilding = buildingTypeList[1];
            if (OnSelectedChanged != null) OnSelectedChanged(this, new OnSelectedChangedEventArgs { });
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { 
            currentPlaceBuilding = buildingTypeList[2];
            if (OnSelectedChanged != null) OnSelectedChanged(this, new OnSelectedChangedEventArgs { });
        }
    }

}
public class GridObject {
    private Grid<GridObject> grid;
    private int x;
    private int z;
    private PlacedObject placedObject;

    public GridObject(Grid<GridObject> grid, int x, int z) {
        this.grid = grid;
        this.x = x;
        this.z = z;
    }

    public PlacedObject GetPlacedObject() {
        return placedObject;
    }

    public void SetPlacedObject(PlacedObject placedObject) {
        this.placedObject = placedObject;
        grid.TriggerGridObjectChanged(x, z);
    }

    public void ClearPlacedObject() {
        placedObject = null;
        grid.TriggerGridObjectChanged(x, z);
    }

    public bool CanBuild() {
        return placedObject == null;
    }

    public override string ToString() {
        return x + "," + z + "\n" + placedObject;
    }

    public void SetVisible(bool isVisible) {
        if(placedObject != null) {
            placedObject.SetVisible(isVisible);
        }
    }
}
