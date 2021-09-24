using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] private List<BuildingPlaceableScriptableObject> buildingTypeList;
    private BuildingPlaceableScriptableObject currentPlaceBuilding;

    private Deck activeDeck;
    private int activeGridScale = 1;
    private PlaceableScriptableObject.Dir dir = PlaceableScriptableObject.Dir.Down;

    private static GridBuildingSystem s_instance;

    public event EventHandler<OnSelectedBuildingChangedArgs> OnSelectedBuildingChanged;
    public class OnSelectedBuildingChangedArgs : EventArgs { }

    public event EventHandler<OnBuildingSysActiveArgs> OnBuildingSysActive;
    public class OnBuildingSysActiveArgs : EventArgs {
        public bool active;
    }

    //temporary i think, for villager testing purposes
    public event EventHandler<OnPlacedBuildingArgs> OnPlacedBuilding;
    public class OnPlacedBuildingArgs : EventArgs {
        public BuildingPlaceableScriptableObject placedObject; //should subclass this to be a building, as there will be more placeable objects
        public Vector2Int gridPosition;
    }

    private bool m_isActive = false;

    public static GridBuildingSystem Instance {
        get => s_instance;
        set {
            if (value != null && s_instance == null)
                s_instance = value;
        }
    }

    public Vector3 GetMouseWorldSnappedPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
            Vector3 hitPoint = hit.point;
            activeDeck.DeckGrid().GetXZ(hitPoint, out int x, out int z);

            Vector2Int rotationOffset = currentPlaceBuilding.GetRotationOffset(dir, activeDeck.DeckScale());
            Vector3 placeObjectWorldPosition = activeDeck.DeckGrid().GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * activeDeck.DeckGrid().GetCellSize();

            return placeObjectWorldPosition;
        }
        return Vector3.zero;
    }

    public PlaceableScriptableObject GetPlacedObjectType() {
        return currentPlaceBuilding;
    }

    public PlacedObject GetPlacedObjectAtWorldPosition(Vector3 worldPosition) {
        GridObject gridObject = activeDeck.DeckGrid().GetGridObject(worldPosition);
        PlacedObject placedObject = gridObject.GetPlacedObject();
        return placedObject;
    }

    public PlacedObject GetObjectAtGridPosition(int x, int z) {
        GridObject gridObject = activeDeck.DeckGrid().GetGridObject(x, z);
        PlacedObject placedObject = gridObject.GetPlacedObject();
        return placedObject;
    }

    public void SetActiveGrid(Deck activeDeck) {
        this.activeDeck = activeDeck;
        activeGridScale = activeDeck.DeckScale();
    }

    public void SetDeckVisible(bool isVisible, Grid<GridObject> grid) {
        for (int i = 0; i < grid.Width(); i++) {
            for (int j = 0; j < grid.Height(); j++) {
                grid.GetGridObject(i, j).SetVisible(isVisible);
            }
        }
    }

    public bool IsActive() {
        return m_isActive;
    }

    public void ToggleActive() {
        m_isActive = !m_isActive;
        Instance.enabled = m_isActive;

        if(OnBuildingSysActive != null) {
            OnBuildingSysActive(this, new OnBuildingSysActiveArgs { active = m_isActive });
        }

        Debug.Log("GridBuildingSystem is now: " + m_isActive);
    }

    public Quaternion GetPlacedObjectRotation() {
        return Quaternion.Euler(0, currentPlaceBuilding.GetRotationAngle(dir), 0);
    }

    private void Awake() {
        if(Instance != null) {
            Debug.LogError("Multiple Building Systems");
            return;
        }
        Instance = this;
        
        currentPlaceBuilding = buildingTypeList[0];


        //Register Events to listen to
        PlayerInput.OnLeftClickEvent += Instance_OnLeftClickEvent;
        PlayerInput.OnRightClickEvent += Instance_OnRightClickEvent;
        activeGridScale = 1;
        enabled = m_isActive;
    }

    private void Instance_OnLeftClickEvent(object sender, PlayerInput.OnLeftClickArgs e) {
        Instance.HandleLeftClick(e.worldPosition);
    }

    private void HandleLeftClick(Vector3 mousePosition) {
        if(enabled) {
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out hit)) {
                Debug.Log(hit.point);
                Vector3 hitPoint = hit.point;
                hitPoint.y = 0f;

                activeDeck.DeckGrid().GetXZ(hitPoint, out int x, out int z);

                List<Vector2Int> gridPositionList = currentPlaceBuilding.GetGridPositionList(new Vector2Int(x, z), dir, activeGridScale);


                //Test can build 
                bool canBuild = true;
                foreach (Vector2Int gridPosition in gridPositionList) {
                    if (gridPosition.x >= 0 && gridPosition.y >= 0 && gridPosition.x < activeDeck.DeckGrid().Width() && gridPosition.y < activeDeck.DeckGrid().Height()) {
                        if (!activeDeck.DeckGrid().GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                            //cannot build here
                            canBuild = false;
                            break;
                        }
                    }
                    else {
                        canBuild = false;
                    }
                }

                GridObject gridObject = activeDeck.DeckGrid().GetGridObject(x, z);
                if (canBuild) {
                    Vector2Int rotationOffset = currentPlaceBuilding.GetRotationOffset(dir, activeDeck.DeckScale());
                    Vector3 placeObjectWorldPosition = activeDeck.DeckGrid().GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * activeDeck.DeckGrid().GetCellSize();

                    if (currentPlaceBuilding.isMultiDeck) {
                        BuildingPlacedObject placedObject = BuildingPlacedObject.CreateBuilding(placeObjectWorldPosition, new Vector2Int(x, z), dir, currentPlaceBuilding, activeGridScale);
                        activeDeck.AddObject(placedObject.gameObject);

                        foreach (Vector2Int gridPosition in gridPositionList) {
                            activeDeck.DeckGrid().GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                        }

                        //will have option to switch direction of the stairs, for now only going down
                        Deck belowDeck = BoatManager.Instance.GetNextBelowDeck();
                        if (belowDeck != null) {
                            Vector3 objectBelowDeckPosition = belowDeck.DeckGrid().GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * activeDeck.DeckGrid().GetCellSize();
                            BuildingPlacedObject belowObject = BuildingPlacedObject.CreateBuilding(objectBelowDeckPosition, new Vector2Int(x, z), dir, currentPlaceBuilding, activeGridScale);
                            belowDeck.AddObject(belowObject.gameObject);
                            belowObject.SetVisible(belowDeck.IsVisible());

                            foreach (Vector2Int gridPosition in gridPositionList) {
                                belowDeck.DeckGrid().GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(belowObject);
                            }
                        }
                    }
                    else {
                        BuildingPlacedObject placedObject = BuildingPlacedObject.CreateBuilding(placeObjectWorldPosition, new Vector2Int(x, z), dir, currentPlaceBuilding, activeGridScale);

                        foreach (Vector2Int gridPosition in gridPositionList) {
                            activeDeck.DeckGrid().GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                        }
                    }

                    if (OnPlacedBuilding != null) { OnPlacedBuilding(this, new OnPlacedBuildingArgs { placedObject = currentPlaceBuilding, gridPosition =  new Vector2Int(x, z)}); }
                }
                else {
                    //StaticFunctions.CreateWorldTextPopup("Cannot build here!", hitPoint);
                    Debug.Log("Cannot build here");
                }
            }
        }
    }

    private void Instance_OnRightClickEvent(object sender, PlayerInput.OnRightClickArgs e) {
        Instance.HandleRightClick(e.worldPosition);
    }
    private void HandleRightClick(Vector3 mousePosition) {
        if(enabled) {
            ToggleActive();
            //Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            //RaycastHit hit;

            //if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out hit)) {
            //    Vector3 hitPoint = hit.point;
            //    activeDeck.DeckGrid().GetXZ(hitPoint, out int x, out int z);

            //    GridObject gridObject = activeDeck.DeckGrid().GetGridObject(x, z);
            //    PlacedObject placedObject = gridObject.GetPlacedObject();
            //    if (placedObject != null) {
            //        placedObject.DestroySelf();

            //        List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();

            //        foreach (Vector2Int gridPosition in gridPositionList) {
            //            activeDeck.DeckGrid().GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
            //        }
            //    }
            //}
        }
    }

    private void Update() {
        //Rotate current placing building
        if (Input.GetKeyDown(KeyCode.R)) {
            dir = PlaceableScriptableObject.GetNextDir(dir);
            Debug.Log(dir);
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) { 
            currentPlaceBuilding = buildingTypeList[0]; 
            if (OnSelectedBuildingChanged != null) OnSelectedBuildingChanged(this, new OnSelectedBuildingChangedArgs { });
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { 
            currentPlaceBuilding = buildingTypeList[1];
            if (OnSelectedBuildingChanged != null) OnSelectedBuildingChanged(this, new OnSelectedBuildingChangedArgs { });
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { 
            currentPlaceBuilding = buildingTypeList[2];
            if (OnSelectedBuildingChanged != null) OnSelectedBuildingChanged(this, new OnSelectedBuildingChangedArgs { });
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            currentPlaceBuilding = buildingTypeList[3];
            if (OnSelectedBuildingChanged != null) OnSelectedBuildingChanged(this, new OnSelectedBuildingChangedArgs { });
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
