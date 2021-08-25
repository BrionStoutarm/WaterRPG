using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class BoatManager : MonoBehaviour
{
    public int gridWidth, gridHeight;

    Grid<DeckGridObject> topDeckGrid;
    Grid<bool> middleDeckGrid;
    Grid<bool> bottomDeckGrid;

    Deck[] deckList;
    public GameObject topDeckObject;
    public GameObject middleDeckObject;
    public GameObject bottomDeckObject;

    public GameManager gameManager;

    private int currentDeck = 0;

    // Start is called before the first frame update
    void Start()
    {
        deckList = new Deck[3];


        Renderer rend = topDeckObject.GetComponent<Renderer>();
        Vector3 origin = new Vector3(rend.bounds.min.x, 1, rend.bounds.min.z);
        Vector3 topRight = new Vector3(rend.bounds.max.x, 1, rend.bounds.max.z);

        topDeckGrid = new Grid<DeckGridObject>(gridWidth, gridHeight, 1f, origin, topRight, (Grid<DeckGridObject> g, int x, int y) => new DeckGridObject(g, x, y), gameManager.OnDebug());
        Deck topDeck = new Deck(topDeckGrid, topDeckObject);
        deckList[0] = topDeck;

        //Renderer midRend = middleDeckObject.GetComponent<Renderer>();
        //middleDeckGrid = new Grid<bool>(gridWidth, gridHeight, 1f, new Vector3(midRend.bounds.min.x, 0, midRend.bounds.min.z), new Vector3(midRend.bounds.max.x, 0, midRend.bounds.max.z), gameManager.OnDebug());
        //Deck midDeck = new Deck(middleDeckGrid, middleDeckObject);
        //deckList[1] = midDeck;
        //midDeck.DeckObj().SetActive(false);

        //Renderer botRend = bottomDeckObject.GetComponent<Renderer>();
        //bottomDeckGrid = new Grid<bool>(gridWidth, gridHeight, 1f, new Vector3(botRend.bounds.min.x, -1, botRend.bounds.min.z), new Vector3(botRend.bounds.max.x, -1, botRend.bounds.max.z), gameManager.OnDebug());
        //Deck botDeck = new Deck(bottomDeckGrid, bottomDeckObject);
        //deckList[2] = botDeck;
        //botDeck.DeckObj().SetActive(false);


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
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            if (currentDeck != 0) {
                int prevDeck = currentDeck;
                currentDeck--;
                deckList[currentDeck].DeckObj().SetActive(true);
                deckList[prevDeck].DeckObj().SetActive(false);
            }
        }

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawLine(ray.GetPoint(100.0f), Camera.main.transform.position, Color.red, 10.0f);

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) {
                Debug.Log(hit.point);
                Vector3 hitPoint = hit.point;
                hitPoint.y = 0f;
                DeckGridObject deckGridObject = deckList[currentDeck].DeckGrid().GetGridObject(hitPoint);
                if(deckGridObject != null) {
                    deckGridObject.ChangeValue(5000);
                }
            }
        }

    }

}

//Buildings will have doorways on specific tiles and whatnot -- will fill in later
public class DeckGridObject {
    Grid<DeckGridObject> grid;
    int x, y;

    public int value;

    public DeckGridObject(Grid<DeckGridObject> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void ChangeValue(int value) {
        this.value = value;
        grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString() {
        return value.ToString();
    }
}

public class Deck {
    private Grid<DeckGridObject> deckGrid;
    private GameObject deckObject;

    public Deck(Grid<DeckGridObject> grid, GameObject obj) {
        deckGrid = grid;
        deckObject = obj;
    }

    public Grid<DeckGridObject> DeckGrid() {
        return deckGrid;
    }

    public GameObject DeckObj() {
        return deckObject;
    }
}
