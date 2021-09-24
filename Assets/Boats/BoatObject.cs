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
        decks[0].SetVisible(true);
    }

    public Deck GetDeck(int deckNumber) {
        return decks[deckNumber];
    }

    public int GetNumDecks() {
        return decks.Count;
    }

    //how much it should rotate and gridPos to determine which direction should rotate
    public void RotateBoat(int weight, Vector2Int gridPos) {
        //Port side
        if(gridPos.x <= gridSizes[0].width / 2) {
            transform.Rotate(new Vector3(0, 0, -1), BoatManager.Instance.rotationFactor * weight);
        }

        //Starboard side
        else {
            transform.Rotate(new Vector3(0, 0, 1), BoatManager.Instance.rotationFactor * weight);
        }
    }
}
