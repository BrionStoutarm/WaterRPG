using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatObject : MonoBehaviour {
    [System.Serializable]
    public class GridSize {
        public int width;
        public int height;
        public float cellSize;
    }

    public GameObject[] deckObjects;
    public GridSize[] gridSizes;

    private List<Deck> deckData;
    private int currentDeck = 0;

    private void Start() {
        deckData = new List<Deck>();

        for(int i = 0; i < deckObjects.Length; i++) {
            GameObject deckObject = deckObjects[i];
            Vector2Int gridSize = new Vector2Int(gridSizes[i].width, gridSizes[i].height);

            deckData.Add(new Deck(deckObject, gridSize, gridSizes[i].cellSize));
        }
    }

    public Deck GetDeck(int deckNumber) {
        return deckData[deckNumber];
    }

    public Deck GetNextAboveDeck() {
        if (currentDeck != 0) {
            int prevDeck = currentDeck;
            currentDeck--;
            deckData[currentDeck].SetVisible(true);
            deckData[prevDeck].SetVisible(false);

            return deckData[currentDeck];
        }
        return null;
    }

    public Deck GetNextBelowDeck() {
        if (currentDeck != deckData.Count - 1) {
            int prevDeck = currentDeck;
            currentDeck++;
            deckData[currentDeck].SetVisible(true);
            deckData[prevDeck].SetVisible(false);

            return deckData[currentDeck];
        }
        return null;
    }
}