using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatObject : MonoBehaviour {
    [System.Serializable]
    public class GridSize {
        public int width;
        public int height;
        public int gridScale;
    }

    public GameObject[] deckObjects;
    public GridSize[] gridSizes;

    private List<Deck> decks;
    private int currentDeck = 0;

    private void Start() {

    }

    public void InitializeBoat() {
        decks = new List<Deck>();

        for (int i = 0; i < deckObjects.Length; i++) {
            GameObject deckObject = deckObjects[i];
            Vector2Int gridSize = new Vector2Int(gridSizes[i].width, gridSizes[i].height);
            int gridScale = gridSizes[i].gridScale;

            Deck newDeck = new Deck(deckObject, gridSize, gridScale);
            decks.Add(newDeck);
            newDeck.SetVisible(false);
        }
    }

    public Deck GetDeck(int deckNumber) {
        return decks[deckNumber];
    }

    public Deck GetNextAboveDeck() {
        if (currentDeck != 0) {

            return decks[currentDeck--];
        }
        return null;
    }

    public Deck SetNextAboveDeck() {
        if (currentDeck != 0) {
            int prevDeck = currentDeck;
            currentDeck--;
            decks[currentDeck].SetVisible(true);
            decks[prevDeck].SetVisible(false);

            return decks[currentDeck];
        }
        return null;
    }
    public Deck GetNextBelowDeck() {
        if (currentDeck != decks.Count - 1) {

            return decks[++currentDeck];
        }
        return null;
    }

    public Deck SetNextBelowDeck() {
        if (currentDeck != decks.Count - 1) {
            int prevDeck = currentDeck;
            currentDeck++;
            decks[currentDeck].SetVisible(true);
            decks[prevDeck].SetVisible(false);

            return decks[currentDeck];
        }
        return null;
    }
}
