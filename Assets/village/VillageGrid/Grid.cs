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
    private Vector3[] gridPoints;
    private List<Vector3> gridPointsList;
    LineRenderer lineRenderer;

    public Grid(int width, int height, float cellSize, Vector3 originPosition, Vector3 topRightPosition, Transform parent) {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        lineRenderer = parent.GetComponent<LineRenderer>();
        lineRenderer.positionCount = width * height * 4;
        gridPoints = new Vector3[width * height];
        gridPointsList = new List<Vector3>();
        heightCellSize = (topRightPosition.z - originPosition.z) / height;
        widthCellSize = (topRightPosition.x - originPosition.x) / width;

        gridArray = new int[width, height];
        debugTextArray = new TextMesh[width, height];
        

        for(float i = originPosition.x; i <= topRightPosition.x; i += 1 * widthCellSize) {
            Vector3 pos = new Vector3(i, parent.position.y, originPosition.z);
            Vector3 oppPos = new Vector3(i, parent.position.y, topRightPosition.z);
            gridPointsList.Add(pos);
            gridPointsList.Add(oppPos);
        }

        for(float i = originPosition.z; i <= topRightPosition.z; i += 1 * heightCellSize) {
            Vector3 pos = new Vector3(originPosition.x, parent.position.y, i);
            Vector3 oppPos = new Vector3(topRightPosition.x, parent.position.y, i);
            gridPointsList.Add(pos);
            gridPointsList.Add(oppPos);
        }

        gridPoints = gridPointsList.ToArray();

        //for (int x = 0; x < gridArray.GetLength(0) - 1;  x++) {
        //    for (int y = 0; y < gridArray.GetLength(1) - 1; y++) {
        //        //debugTextArray[x, y] = CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 3, TextAnchor.MiddleCenter);
        //        gridPoints[x + y] = GetWorldPosition(x, y);
        //        gridPoints[x + y + 1] = GetWorldPosition(x + 1, y);
        //        gridPoints[x + y + 2] = GetWorldPosition(x, y + 1);
        //        gridPoints[x + y + 3] = GetWorldPosition(x + y, y + 1);
        //        //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
        //        //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);

        //    }
        //}
        //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
        //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

        lineRenderer.SetPositions(gridPoints);
    }

    public Vector3[] GridPoints() {
        return gridPoints;
    }

    private Vector3 GetWorldPosition(int x, int y) {
        Vector3 vec = new Vector3(x * widthCellSize, 0, y * heightCellSize) + originPosition;
        return vec;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetValue(int x, int y, int value) {
        if (x >= 0 && y >= 0 && x <= width && y <= height) { 
            gridArray[x, y] = value;
            debugTextArray[x, y].text = gridArray[x, y].ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, int value) {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
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
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }

    //CodeMonkey.utils
    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = 5000) {
        Color color = Color.white;
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
    public static Vector3 GetMouseWorldPosition() {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.y = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ() {
        return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera) {
        return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
        Vector3 worldPosition = worldCamera.ScreenToViewportPoint(screenPosition);
        return worldPosition;
    }
}
