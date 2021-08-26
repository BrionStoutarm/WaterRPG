using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class BoatManager : MonoBehaviour
{
    public int gridWidth, gridHeight;

    Grid<GridObject> topDeckGrid;
    Grid<GridObject> middleDeckGrid;
    Grid<GridObject> bottomDeckGrid;

    Deck[] deckList;
    public GameObject topDeckObject;
    public GameObject middleDeckObject;
    public GameObject bottomDeckObject;

    public GameManager gameManager;

    private int currentDeck = 0;

    private void Awake() {

    }

    // Start is called before the first frame update
    void Start()
    {
        deckList = new Deck[3];


        Renderer rend = topDeckObject.GetComponent<Renderer>();
        Vector3 origin = new Vector3(rend.bounds.min.x, topDeckObject.transform.position.y, rend.bounds.min.z);
        topDeckGrid = new Grid<GridObject>(gridWidth, gridHeight, 10f, origin, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y), gameManager.OnDebug());
        Deck topDeck = new Deck(topDeckGrid, topDeckObject);
        deckList[0] = topDeck;

        Renderer midRend = middleDeckObject.GetComponent<Renderer>();
        origin = new Vector3(rend.bounds.min.x, middleDeckObject.transform.position.y, rend.bounds.min.z);
        middleDeckGrid = new Grid<GridObject>(gridWidth, gridHeight, 10f, origin, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y), gameManager.OnDebug());
        Deck midDeck = new Deck(middleDeckGrid, middleDeckObject);
        deckList[1] = midDeck;
        midDeck.DeckObj().SetActive(false);

        Renderer botRend = bottomDeckObject.GetComponent<Renderer>();
        origin = new Vector3(botRend.bounds.min.x, bottomDeckObject.transform.position.y, botRend.bounds.min.z);
        bottomDeckGrid = new Grid<GridObject>(gridWidth, gridHeight, 10f, origin, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y), gameManager.OnDebug());
        Deck botDeck = new Deck(bottomDeckGrid, bottomDeckObject);
        deckList[2] = botDeck;
        botDeck.DeckObj().SetActive(false);

        GridBuildingSystem.Instance.SetActiveGrid(deckList[currentDeck].DeckGrid());
    }

    //private void OnDrawGizmos() {
    //    foreach (Vector3 point in topDeckGrid.GridPoints()) {
    //        Gizmos.DrawSphere(point, 0.025f);
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (currentDeck != 2) {
                int prevDeck = currentDeck;
                currentDeck++;
                deckList[currentDeck].DeckObj().SetActive(true);
                deckList[prevDeck].DeckObj().SetActive(false);

                GridBuildingSystem.Instance.SetActiveGrid(deckList[currentDeck].DeckGrid());
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (currentDeck != 0) {
                int prevDeck = currentDeck;
                currentDeck--;
                deckList[currentDeck].DeckObj().SetActive(true);
                deckList[prevDeck].DeckObj().SetActive(false);

                GridBuildingSystem.Instance.SetActiveGrid(deckList[currentDeck].DeckGrid());
            }
        }
    }

}

public class Deck {
    private Grid<GridObject> deckGrid;
    private GameObject deckObject;

    public Deck(Grid<GridObject> grid, GameObject obj) {
        deckGrid = grid;
        deckObject = obj;
    }

    public Grid<GridObject> DeckGrid() {
        return deckGrid;
    }

    public GameObject DeckObj() {
        return deckObject;
    }
}
