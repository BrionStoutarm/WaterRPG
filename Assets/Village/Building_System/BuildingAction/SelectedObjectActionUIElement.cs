using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectedObjectActionUIElement : MonoBehaviour
{
    public Button actionButton;
    public Text actionNameText;
    private Func<int> actionButtonFunction;

    private void Awake() {
    }

    public void SetProperties(string actionName, Func<int> actionButtonFunction) {        
        actionNameText.text = actionName;
        this.actionButtonFunction = actionButtonFunction;
        actionButton.onClick.AddListener(HandleActionButton);

        Text actionText = actionButton.GetComponentInChildren<Text>();
        actionText.text = "testtting";
    }

    public void HandleActionButton() {
        int val = actionButtonFunction();
        //can do error checking here?
    }
}
