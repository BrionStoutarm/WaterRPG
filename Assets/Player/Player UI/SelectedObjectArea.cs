using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedObjectArea : MonoBehaviour {
    public Text selectedObjectText;
    public LayoutGroup layoutGroup;
    public SelectedObjectActionUIElement actionPrefab;
    List<SelectedObjectActionUIElement> selectedObjectActions;
    private void Awake() {
        selectedObjectActions = new List<SelectedObjectActionUIElement>();
    }

    public void UpdateUI(PlacedObject placedObject) {
        ClearActionElements();

        selectedObjectText.text = placedObject.GetObjectName();
        foreach (PlaceableAction action in placedObject.ActionList) {
            SelectedObjectActionUIElement newAction = Instantiate(actionPrefab, layoutGroup.transform);
            newAction.SetProperties(action.ActionName, action.ActionActivate);
            selectedObjectActions.Add(newAction);
        }
        
    }

    public void Clear() {
        ClearActionElements();
        ResetText();
    }

    private void ResetText() {
        selectedObjectText.text = "";
    }

    private void ClearActionElements() {
        if(selectedObjectActions.Count != 0) {
            foreach(SelectedObjectActionUIElement uiElement in selectedObjectActions) {
                Destroy(uiElement.gameObject);
            }
        }
        selectedObjectActions.Clear();
    }
}
