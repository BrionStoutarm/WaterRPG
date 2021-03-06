using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BoatManager : MonoBehaviour {
    public BoatObject boatObject;
    public float rotationFactor;
    public float maxRotation;

    private int p_currentDeck = 0;
    public int currentDeck {
        get => p_currentDeck;
        private set {
            if (value >= 0 && value < boatObject.GetNumDecks()) {
                p_currentDeck = value;
            }
        }
    }

    private static BoatManager s_instance;
    public static BoatManager Instance {
        get => s_instance;
        set {
            if(value != null && s_instance == null) {
                s_instance = value;
            }
        }
    }


    public class ChangedDeckEventArgs : EventArgs { }
    public event EventHandler<ChangedDeckEventArgs> ChangedDeckEvent;


    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }

        PlayerInput.OnUpArrowEvent += Instance_OnUpArrowEvent;
        PlayerInput.OnDownArrowEvent += Instance_OnDownEvent; 
    }

    private void Instance_OnDownEvent(object sender, PlayerInput.OnDownArrowArgs e) {
        Deck nextDeck = SetNextBelowDeck();
        if(nextDeck != null) {
            GridBuildingSystem.Instance.SetActiveGrid(nextDeck);
            if(ChangedDeckEvent != null) { ChangedDeckEvent(this, new ChangedDeckEventArgs { }); }
        }
    }

    private void Instance_OnUpArrowEvent(object sender, PlayerInput.OnUpArrowArgs e) {
        Deck nextDeck = SetNextAboveDeck();
        if (nextDeck != null) {
            GridBuildingSystem.Instance.SetActiveGrid(nextDeck);
            if (ChangedDeckEvent != null) { ChangedDeckEvent(this, new ChangedDeckEventArgs { }); }
        }
    }

    private void Instance_OnBuildingPlacedEvent(object sender, GridBuildingSystem.OnPlacedBuildingArgs args) {
        boatObject.RotateBoat(args.placedObject.weight, args.gridPosition);
    }

    public Deck GetNextAboveDeck() {
        if (currentDeck != 0) {
            return boatObject.GetDeck(currentDeck - 1);
        }
        return null;
    }

    public Deck SetNextAboveDeck() {
        if (currentDeck != 0) {
            int prevDeck = currentDeck;
            currentDeck--;
            boatObject.GetDeck(currentDeck).SetVisible(true);
            boatObject.GetDeck(prevDeck).SetVisible(false);

            return boatObject.GetDeck(currentDeck);
        }
        return null;
    }

    public Deck GetNextBelowDeck() {
        if (currentDeck != boatObject.GetNumDecks() - 1) {
            return boatObject.GetDeck(currentDeck + 1);
        }
        return null;
    }

    public Deck SetNextBelowDeck() {
        if (currentDeck != boatObject.GetNumDecks() - 1) {
            int prevDeck = currentDeck;
            currentDeck++;
            boatObject.GetDeck(currentDeck).SetVisible(true);
            boatObject.GetDeck(prevDeck).SetVisible(false);

            return boatObject.GetDeck(currentDeck);
        }
        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        boatObject.InitializeBoat();
        GridBuildingSystem.Instance.SetActiveGrid(boatObject.GetDeck(0));

        GridBuildingSystem.Instance.OnPlacedBuilding += Instance_OnBuildingPlacedEvent;//load order needs to be fixed
    }

    // Update is called once per frame
    void Update()
    {

    }
}

