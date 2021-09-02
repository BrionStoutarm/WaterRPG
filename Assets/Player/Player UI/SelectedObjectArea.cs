using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObjectArea : MonoBehaviour {

    public Rect area;
    public GUILayout guiLayout;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnGUI() {
        GUI.BeginGroup(area);
        GUI.Box(new Rect(0, 0, 100, 100), "Group is here");
        GUI.Button(new Rect(10, 40, 80, 30), "Click me");
        GUI.EndGroup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
