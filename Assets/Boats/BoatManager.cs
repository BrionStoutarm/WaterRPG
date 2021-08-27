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
        midDeck.SetVisible(false);

        Renderer botRend = bottomDeckObject.GetComponent<Renderer>();
        origin = new Vector3(botRend.bounds.min.x, bottomDeckObject.transform.position.y, botRend.bounds.min.z);
        bottomDeckGrid = new Grid<GridObject>(gridWidth, gridHeight, 10f, origin, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y), gameManager.OnDebug());
        Deck botDeck = new Deck(bottomDeckGrid, bottomDeckObject);
        deckList[2] = botDeck;
        botDeck.SetVisible(false);

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
                deckList[currentDeck].SetVisible(true);
                deckList[prevDeck].SetVisible(false);
           
                //GridBuildingSystem.Instance.SetDeckVisible(true, deckList[currentDeck].DeckGrid());
                //GridBuildingSystem.Instance.SetDeckVisible(false, deckList[prevDeck].DeckGrid());
                GridBuildingSystem.Instance.SetActiveGrid(deckList[currentDeck].DeckGrid());
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (currentDeck != 0) {
                int prevDeck = currentDeck;
                currentDeck--;
                deckList[currentDeck].SetVisible(true);
                deckList[prevDeck].SetVisible(false);

                //GridBuildingSystem.Instance.SetDeckVisible(true, deckList[currentDeck].DeckGrid());
                //GridBuildingSystem.Instance.SetDeckVisible(false, deckList[prevDeck].DeckGrid());
                GridBuildingSystem.Instance.SetActiveGrid(deckList[currentDeck].DeckGrid());
            }
        }
    }
    public class Deck {
        private Grid<GridObject> deckGrid;
        private GameObject deckObject;
        private Vector3 originalDeckScale;
        public Deck(Grid<GridObject> grid, GameObject obj) {
            deckGrid = grid;
            deckObject = obj;
            originalDeckScale = obj.transform.localScale;
        }

        public Grid<GridObject> DeckGrid() {
            return deckGrid;
        }

        public GameObject DeckObj() {
            return deckObject;
        }

        public void SetVisible(bool isVisible) {
            if(isVisible) {
                deckObject.transform.localScale = originalDeckScale;
            }
            else {
                deckObject.transform.localScale = Vector3.zero;
            }
            GridBuildingSystem.Instance.SetDeckVisible(isVisible, deckGrid);
        }
    }
}

