using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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




    private void GameControls() {
    //    if (Input.GetKeyDown(m_cancel)) {
    //        Deselect();
    //        m_gameManager.Unfollow();
    //        if (m_placer) {
    //            m_placer.ClearPlacer();
    //        }
    //    }
    //    if (Input.GetKeyDown(m_pause)) {
    //        m_gameManager.TogglePause();
    //    }
    }
}
