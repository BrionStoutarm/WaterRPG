using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    private GameManager m_gameManager;
    public GameObject m_selected;
    public ObjectPlacer m_placer;

    public KeyCode m_cameraLeft = KeyCode.A;
    public KeyCode m_cameraRight = KeyCode.D;
    public KeyCode m_cameraUp = KeyCode.W;
    public KeyCode m_cameraDown = KeyCode.S;
    public KeyCode m_cancel = KeyCode.Escape;
    public KeyCode m_pause = KeyCode.Return;
    // Start is called before the first frame update
    void Start()
    {
        m_gameManager = GameManager.Get();
    }

    // Update is called once per frame
    void Update()
    {
        CameraMovement();
        GameControls();
        if (StaticMethods.IsPointerOverUIElement())
        {
            return;
        }
        if(m_placer && m_placer.Placing()){
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            var obj = GetObjectAtMouse();
            if (obj)
            {
                if (Select(obj))
                {
                    m_gameManager.Follow(obj);
                }
                else
                {
                    Deselect();
                    m_gameManager.Unfollow();
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            HandleAction();
        }
    }

    private void CameraMovement()
    {
        if (Input.GetKey(m_cameraLeft))
        {
            m_gameManager.CameraLeft();
        }
        if (Input.GetKey(m_cameraRight))
        {
            m_gameManager.CameraRight();
        }
        if (Input.GetKey(m_cameraDown))
        {
            m_gameManager.CameraDown();
        }
        if (Input.GetKey(m_cameraUp))
        {
            m_gameManager.CameraUp();
        }
    }

    private void GameControls()
    {
        if (Input.GetKeyDown(m_cancel))
        {
            Deselect();
            m_gameManager.Unfollow();
            if (m_placer)
            {
                m_placer.ClearPlacer();
            }
        }
        if (Input.GetKeyDown(m_pause))
        {
            m_gameManager.TogglePause();
        }
    }
    private GameObject GetObjectAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject obj = hit.transform.gameObject;
            if (obj.transform.parent != null)
                obj = obj.transform.parent.gameObject;

            return obj;
        }

        return null;
    }

    private Vector3 ClickLocation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return new Vector3(0, 0, 0);
    }

    public void Deselect()
    {
        m_selected = null;
    }

    public bool Select(GameObject obj)
    {
        if (obj.CompareTag("Selectable"))
        {
            m_selected = obj;
            return true;
        }
        return false;
    }

    private void HandleAction()
    {
        if (m_selected)
        {
            VillagerAI v = m_selected.GetComponentInChildren<VillagerAI>();
            if (v)
            {
                v.GoTo(ClickLocation());
            }
        }
    }
}
