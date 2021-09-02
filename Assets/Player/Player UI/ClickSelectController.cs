using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSelectController : MonoBehaviour
{
    [SerializeField] private Camera m_camera;

    public static event Action<PlacedObject> OnSelectedObjectChanged;

    public static PlacedObject SelectedObject { get; private set; }

    string text;
    private void Awake() {
        PlayerInput.OnLeftClickEvent += HandleLeftClick;
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
            }
            else {
                Debug.Log("Didnt hit placedObject");
            }
        }
    }
}
