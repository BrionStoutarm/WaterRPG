using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private Transform visual;
    private BuildingScriptableObject buildingType;

    private void Start() {
        RefreshVisual();

        GridBuildingSystem.Instance.OnSelectedChanged += Instance_OnSelectedChanged;
    }

    private void Instance_OnSelectedChanged(object sender, System.EventArgs e) {
        RefreshVisual();
    }

    private void LateUpdate() {
        if (GridBuildingSystem.Instance.IsActive()) {
            Vector3 targetPosition = GridBuildingSystem.Instance.GetMouseWorldSnappedPosition();
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
            transform.rotation = Quaternion.Lerp(transform.rotation, GridBuildingSystem.Instance.GetPlacedObjectRotation(), Time.deltaTime * 15f);
        }
    }

    private void RefreshVisual() {
        if(visual != null) {
            Destroy(visual.gameObject);
            visual = null;
        }

        BuildingScriptableObject buildingObjectType = GridBuildingSystem.Instance.GetPlacedObjectType();

        if(buildingObjectType != null) {
            visual = Instantiate(buildingObjectType.visual, Vector3.zero, Quaternion.identity);
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;
            //SetLayerRecursive(visual.gameObject, 11);
        }
    }
}
