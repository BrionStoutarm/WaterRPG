using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TGMap))]
public class TGMapInspector : Editor {
    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        DrawDefaultInspector();
        if(GUILayout.Button("Regenerate Map")) {
            TGMap tileMap = (TGMap)target;
            tileMap.BuildMesh();
        }
    }
}
