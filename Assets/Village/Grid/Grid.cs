using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<TGridObject> {

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs {
        public int x;
        public int z;
    }

    private int width;
    private int height;
    private float cellSize;
    private float heightCellSize;
    private float widthCellSize;

    private bool debugMode = false;
    private TextMesh[,] debugTextArray;

    private Vector3 originPosition;
    private TGridObject[,] gridArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Vector3 topRightPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject, bool debugMode) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;


        heightCellSize = (topRightPosition.z - originPosition.z) / height;
        widthCellSize = (topRightPosition.x - originPosition.x) / width;

        gridArray = new TGridObject[width, height];
        for (int x = 0; x < gridArray.GetLength(0); x++) {
            for (int z = 0; z < gridArray.GetLength(1); z++) {
                gridArray[x, z] = createGridObject(this, x, z);
            }
        }

        //Debug
        if (debugMode) {
            debugTextArray = new TextMesh[width, height];
            for (int x = 0; x < gridArray.GetLength(0) ; x++) {
                for (int z = 0; z < gridArray.GetLength(1); z++) {
                    debugTextArray[x, z] = StaticFunctions.CreateWorldText(gridArray[x, z].ToString(), null, GetWorldPosition(x, z) + new Vector3(widthCellSize / 2, 0, heightCellSize / 2), 8, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) => {
                debugTextArray[eventArgs.x, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.z].ToString();
            };
        }
    }

    public int Width() {
        return width;
    }

    public int Height() {
        return height;
    }

    public float GetCellSize() {
        return cellSize;
    }

    public Vector3 GetWorldPosition(int x, int z) {
        Vector3 vec = new Vector3(x * widthCellSize, 0, z * heightCellSize) + originPosition;
        return vec;
    }

    public void GetXZ(Vector3 worldPosition, out int x, out int z) {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / widthCellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / heightCellSize);
    }

    public void SetGridObject(int x, int z, TGridObject value) {
        if (x >= 0 && z >= 0 && x < width && z < height) { 
            gridArray[x, z] = value;
            if (debugMode) {
                debugTextArray[x, z].text = gridArray[x, z].ToString();
            }
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, z = z });
        }
    }

    public void TriggerGridObjectChanged(int x, int z) {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, z = z });
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value) {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        SetGridObject(x, z, value);
    }

    public TGridObject GetGridObject(int x, int z) {
        if (x >= 0 && z >= 0 && x < width && z < height) {
            return gridArray[x, z];
        }
        else {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition) {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetGridObject(x, z);
    }
}
