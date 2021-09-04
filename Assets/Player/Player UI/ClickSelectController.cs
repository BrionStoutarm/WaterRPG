using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSelectController : MonoBehaviour
{
    [SerializeField] private Camera m_camera;
    [SerializeField] private SelectedObjectArea m_selectedObjectArea;

    public static event Action<PlacedObject> OnSelectedObjectChanged;

    public static PlacedObject SelectedObject { get; private set; }

    string text;
    private void Awake() {
        PlayerInput.OnLeftClickEvent += HandleLeftClick;
        PlayerInput.OnRightClickEvent += HandleRightClick;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HandleLeftClick(object sender, PlayerInput.OnLeftClickArgs args) {
        if (!GridBuildingSystem.Instance.IsActive()) {
            RaycastHit hit = StaticFunctions.GetMouseRaycastHit();
            PlacedObject placedObject = GridBuildingSystem.Instance.GetPlacedObjectAtWorldPosition(hit.point);
            if (placedObject != null) {
                text = placedObject.GetObjectName();
                m_selectedObjectArea.UpdateUI(placedObject);
                SelectedObject = placedObject;
            }
            else {
                Debug.Log("Didnt hit placedObject");
            }
        }
    }

    private void HandleRightClick(object sender, PlayerInput.OnRightClickArgs args) {
        if (!GridBuildingSystem.Instance.IsActive()) {
          
            if (SelectedObject != null) {
                m_selectedObjectArea.Clear();
                SelectedObject = null;
            }
            else {
                Debug.Log("Didnt hit placedObject");
            }
        }
    }
}
