using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//This code comes from Sebastian Lague - https://www.youtube.com/channel/UCmtyQOKKmrMVaKuRXz02jbQ


[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI() {
        MapGenerator mapGen = (MapGenerator)(target);

        if(DrawDefaultInspector()) {
            if(mapGen.autoUpdate) {
                mapGen.DrawMapInEditor();
            }
        }

        if(GUILayout.Button("Generate Map")) {
            mapGen.DrawMapInEditor();
        }
    }
}
