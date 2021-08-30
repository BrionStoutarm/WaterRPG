using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private int currentDeck = 0;
    private GameManager gameManager;

    private static BoatManager s_instance;
    public static BoatManager Instance {
        get => s_instance;
        set {
            if(value != null && s_instance == null) {
                s_instance = value;
            }
        }
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }

        PlayerInput.OnUpArrowEvent += Instance_OnUpArrowEvent;
        PlayerInput.OnDownArrowEvent += Instance_OnDownEvent;

    }

    private void Instance_OnDownEvent(object sender, PlayerInput.OnDownArrowArgs e) {
        if (currentDeck != 2) {
            int prevDeck = currentDeck;
            currentDeck++;
            deckList[currentDeck].SetVisible(true);
            deckList[prevDeck].SetVisible(false);

            GridBuildingSystem.Instance.SetActiveGrid(deckList[currentDeck].DeckGrid());
        }
    }

    private void Instance_OnUpArrowEvent(object sender, PlayerInput.OnUpArrowArgs e) {

        if (currentDeck != 0) {
            int prevDeck = currentDeck;
            currentDeck--;
            deckList[currentDeck].SetVisible(true);
            deckList[prevDeck].SetVisible(false);

            GridBuildingSystem.Instance.SetActiveGrid(deckList[currentDeck].DeckGrid());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
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

