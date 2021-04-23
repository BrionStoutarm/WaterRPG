﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//This code comes from Sebastian Lague - https://www.youtube.com/channel/UCmtyQOKKmrMVaKuRXz02jbQ


[CustomEditor(typeof(MapPreview))]
public class MapPreviewEditor : Editor
{
    public override void OnInspectorGUI() {
        MapPreview mapPreview = (MapPreview)(target);

        if(DrawDefaultInspector()) {
            if(mapPreview.autoUpdate) {
                mapPreview.DrawMapInEditor();
            }
        }

        if(GUILayout.Button("Generate Map")) {
            mapPreview.DrawMapInEditor();
        }
    }
}
