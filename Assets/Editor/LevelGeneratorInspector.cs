using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorInspector : Editor {
	
	public override void OnInspectorGUI() {
		//base.OnInspectorGUI();
		DrawDefaultInspector();
		
		if(GUILayout.Button("Regenerate")) {
			LevelGenerator levelGen = (LevelGenerator)target;
			levelGen.GenerateMap();
		}
	}
}
