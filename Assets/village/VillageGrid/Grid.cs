using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid {

    private int width;
    private int height;
    private float cellSize;
    private float heightCellSize;
    private float widthCellSize;

    private Vector3 originPosition;
    private int[,] gridArray;
    private TextMesh[,] debugTextArray;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Vector3 topRightPosition) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;


        heightCellSize = (topRightPosition.z - originPosition.z) / height;
        widthCellSize = (topRightPosition.x - originPosition.x) / width;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];
        

        //for(float i = originPosition.x; i <= topRightPosition.x; i += 1 * widthCellSize) {
        //    Vector3 pos = new Vector3(i, parent.position.y, originPosition.z);
        //    Vector3 oppPos = new Vector3(i, parent.position.y, topRightPosition.z);
        //    gridPointsList.Add(pos);
        //    //gridPointsList.Add(oppPos);
        //}

        //for(float i = originPosition.z; i <= topRightPosition.z; i += 1 * heightCellSize) {
        //    Vector3 pos = new Vector3(originPosition.x, parent.position.y, i);
        //    Vector3 oppPos = new Vector3(topRightPosition.x, parent.position.y, i);
        //    gridPointsList.Add(pos);
        //    //gridPointsList.Add(oppPos);
        //}

        for (int x = 0; x < gridArray.GetLength(0) ; x++) {
            for (int y = 0; y < gridArray.GetLength(1); y++) {
                debugTextArray[x, y] = StaticFunctions.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(widthCellSize, heightCellSize), 1, TextAnchor.MiddleCenter);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

    private Vector3 GetWorldPosition(int x, int y) {
        Vector3 vec = new Vector3(x * widthCellSize, 0, y * heightCellSize) + originPosition;
        return vec;
    }

    private void GetXZ(Vector3 worldPosition, out int x, out int z) {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / widthCellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / heightCellSize);
    }

    public void SetValue(int x, int y, int value) {
        if (x >= 0 && y >= 0 && x < width && y < height) { 
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, int value) {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        SetValue(x, z, value);
    }

    public int GetValue(int x, int y) {
        if (x >= 0 && y >= 0 && x <= width && y <= height) {
            return gridArray[x, y];
        }
        else {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition, int value) {
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetValue(x, z);
    }
}
