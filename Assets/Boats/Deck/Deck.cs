using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck {
    private Grid<GridObject> deckGrid;
    private int gridScale;

    private GameObject deckObject;
    private Vector3 originalScale;
    public Deck(Grid<GridObject> grid, GameObject obj) {
        deckGrid = grid;
        deckObject = obj;
        originalScale = obj.transform.localScale;
    }

    public Deck(GameObject deckObject, Vector2Int gridSize, int gridScale) {
        Renderer rend = deckObject.GetComponent<Renderer>();
        Vector3 origin = new Vector3(rend.bounds.min.x, deckObject.transform.position.y, rend.bounds.min.z);

        int gridWidth = gridSize.x * gridScale;
        int gridHeight = gridSize.y * gridScale;
        float cellSize = 10f / gridScale; // bad, will need to un-hardcode this

        deckGrid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, origin, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y), GameManager.Instance.OnDebug());

        this.deckObject = deckObject;
        originalScale = deckObject.transform.localScale;
        this.gridScale = gridScale;
    }

    public int DeckScale() {
        return gridScale;
    }

    public Grid<GridObject> DeckGrid() {
        return deckGrid;
    }
    public GameObject DeckObj() {
        return deckObject;
    }

    public void SetVisible(bool isVisible) {
        if (isVisible) {
            deckObject.transform.localScale = originalScale;
        }
        else {
            deckObject.transform.localScale = Vector3.zero;
        }
        GridBuildingSystem.Instance.SetDeckVisible(isVisible, deckGrid);
    }
}