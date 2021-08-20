using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatManager : MonoBehaviour
{
    public int gridWidth, gridHeight;

    Grid topDeckGrid;
    Grid middleDeckGrid;
    Grid bottomDeckGrid;

    public GameObject topDeckObject;
    public GameObject middleDeckObject;
    public GameObject bottomDeckObject;

    // Start is called before the first frame update
    void Start()
    {
        Renderer rend = topDeckObject.GetComponent<Renderer>();
        topDeckGrid = new Grid(gridWidth, gridHeight, 1f, new Vector3(rend.bounds.min.x, 1, rend.bounds.min.z), new Vector3(rend.bounds.max.x, 1, rend.bounds.max.z));

        Renderer midRend = middleDeckObject.GetComponent<Renderer>();
        middleDeckGrid = new Grid(gridWidth, gridHeight, 1f, new Vector3(midRend.bounds.min.x, 0, midRend.bounds.min.z), new Vector3(midRend.bounds.max.x, 0, midRend.bounds.max.z));

        Renderer botRend = bottomDeckObject.GetComponent<Renderer>();
        bottomDeckGrid = new Grid(gridWidth, gridHeight, 1f, new Vector3(botRend.bounds.min.x, -1, botRend.bounds.min.z), new Vector3(botRend.bounds.max.x, -1, botRend.bounds.max.z));


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            //topDeckGrid.SetValue(Grid.GetMouseWorldPosition(), 56);
        }

    }
}
