using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private Transform visual;
    private PlaceableScriptableObject buildingType;

    private void Start() {
        //RefreshVisual();

        GridBuildingSystem.Instance.OnSelectedChanged += Instance_OnSelectedChanged;
        GridBuildingSystem.Instance.OnBuildingSysActive += Instance_OnBuildingSysActive;
    }

    private void Instance_OnBuildingSysActive(object sender, GridBuildingSystem.OnBuildingSysActiveArgs e) {
        if (e.active) {
            RefreshVisual();
        }
        else {
            ClearVisual();
        }

    }

    private void ClearVisual() {
        if (visual != null) {
            Destroy(visual.gameObject);
            visual = null;
        }
    }

    private void Instance_OnSelectedChanged(object sender, System.EventArgs e) {
        RefreshVisual();
    }

    private void LateUpdate() {
        Vector3 targetPosition = GridBuildingSystem.Instance.GetMouseWorldSnappedPosition();
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
        transform.rotation = Quaternion.Lerp(transform.rotation, GridBuildingSystem.Instance.GetPlacedObjectRotation(), Time.deltaTime * 15f);
    }

    private void RefreshVisual() {
        ClearVisual();

        PlaceableScriptableObject buildingObjectType = GridBuildingSystem.Instance.GetPlacedObjectType();

        if(buildingObjectType != null) {
            visual = Instantiate(buildingObjectType.visual, Vector3.zero, Quaternion.identity);
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;
            //SetLayerRecursive(visual.gameObject, 11);
        }
     }
}
