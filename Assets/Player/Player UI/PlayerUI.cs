using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    Text currentSelectedObjectText;
    // Start is called before the first frame update
    void Start()
    {
        currentSelectedObjectText = GameObject.Find("SelectedObjectText").GetComponent<Text>();
        PlayerInput.OnLeftClickEvent += HandleLeftClick;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleLeftClick(object sender, PlayerInput.OnLeftClickArgs args) {
        if(!GridBuildingSystem.Instance.IsActive()) {
            RaycastHit hit = StaticFunctions.GetMouseRaycastHit();
            PlacedObject placedObject = GridBuildingSystem.Instance.GetPlacedObjectAtWorldPosition(hit.point);
            if(placedObject != null) {
                currentSelectedObjectText.text = placedObject.GetObjectName();
            }
            else {
                Debug.Log("Didnt hit placedObject");
            }
        }
    }
}
