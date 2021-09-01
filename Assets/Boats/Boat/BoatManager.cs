using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BoatManager : MonoBehaviour
{
    public BoatObject boatObject;
    //Grid<GridObject> topDeckGrid;
    //Grid<GridObject> middleDeckGrid;
    //Grid<GridObject> bottomDeckGrid;

    //public GameObject topDeckObject;
    //public GameObject middleDeckObject;
    //public GameObject bottomDeckObject;


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
        Deck nextDeck = boatObject.GetNextBelowDeck();
        if(nextDeck != null) {
            GridBuildingSystem.Instance.SetActiveGrid(nextDeck);
        }
    }

    private void Instance_OnUpArrowEvent(object sender, PlayerInput.OnUpArrowArgs e) {
        Deck nextDeck = boatObject.GetNextAboveDeck();
        if (nextDeck != null) {
            GridBuildingSystem.Instance.SetActiveGrid(nextDeck);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Renderer rend = topDeckObject.GetComponent<Renderer>();
        //Vector3 origin = new Vector3(rend.bounds.min.x, topDeckObject.transform.position.y, rend.bounds.min.z);
        //topDeckGrid = new Grid<GridObject>(gridWidth, gridHeight, 10f, origin, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y), GameManager.Instance.OnDebug());
        //DeckScriptableObject topDeck = new DeckScriptableObject(topDeckGrid, topDeckObject);
        //deckList[0] = topDeck;

        //Renderer midRend = middleDeckObject.GetComponent<Renderer>();
        //origin = new Vector3(rend.bounds.min.x, middleDeckObject.transform.position.y, rend.bounds.min.z);
        //middleDeckGrid = new Grid<GridObject>(gridWidth, gridHeight, 10f, origin, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y), GameManager.Instance.OnDebug());
        //DeckScriptableObject midDeck = new DeckScriptableObject(middleDeckGrid, middleDeckObject);
        //deckList[1] = midDeck;
        //midDeck.SetVisible(false);

        //Renderer botRend = bottomDeckObject.GetComponent<Renderer>();
        //origin = new Vector3(botRend.bounds.min.x, bottomDeckObject.transform.position.y, botRend.bounds.min.z);
        //bottomDeckGrid = new Grid<GridObject>(gridWidth, gridHeight, 10f, origin, (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y), GameManager.Instance.OnDebug());
        //DeckScriptableObject botDeck = new DeckScriptableObject(bottomDeckGrid, bottomDeckObject);
        //deckList[2] = botDeck;
        //botDeck.SetVisible(false);
        boatObject.InitializeBoat();
        GridBuildingSystem.Instance.SetActiveGrid(boatObject.GetDeck(0));
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
}

