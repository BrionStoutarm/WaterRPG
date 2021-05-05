using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
using UnityEngine;

[CustomEditor(typeof(BoatMovement))]
public class BoatMovementEditor : Editor
{
    public override void OnInspectorGUI() {
        BoatMovement mapPreview = (BoatMovement)(target);


        //if (GUILayout.Button("Direction")) {
        //    mapPreview.ShowForwardDirection();
        //}
    }
}
