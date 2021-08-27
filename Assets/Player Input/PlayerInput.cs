using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static event EventHandler<OnLeftClickArgs> OnLeftClickEvent;
    public class OnLeftClickArgs : EventArgs {
        public Vector3 worldPosition;
    }

    public static event EventHandler<OnRightClickArgs> OnRightClickEvent;
    public class OnRightClickArgs : EventArgs {
        public Vector3 worldPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            if (OnLeftClickEvent != null) OnLeftClickEvent(this, new OnLeftClickArgs { worldPosition = Input.mousePosition });
        }

        if(Input.GetMouseButtonDown(1)) {
            if (OnRightClickEvent != null) OnRightClickEvent(this, new OnRightClickArgs { worldPosition = Input.mousePosition });
        }
    }

    //should probably be double click to auto zoom on object
    private void HandleLeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawLine(ray.GetPoint(100.0f), Camera.main.transform.position, Color.red, 10.0f);

        RaycastHit hit;

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            Debug.Log("Missed");
            return;
        }

        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (meshCollider == null || meshCollider.sharedMesh == null)
        {
            Debug.Log("Invalid collider");
        }
        Mesh mesh = meshCollider.sharedMesh;
    }
}
