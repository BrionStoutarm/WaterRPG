using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectPlacer : MonoBehaviour
{
    public float m_rotateSpeed = 10000f;
    public float m_heightModifier = .25f;
    public Placeable m_objectToPlace = null;
    private Placeable m_currentObject = null;
    private Transform m_preview = null;
    private bool m_destroy = false;
    private Vector3 m_lastPos;
    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        m_lastPos = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            ClearPlacer();
        }
        if (m_objectToPlace)
        {
            //Get mouse point
            Vector3 placeLoc = GetPlaceLoc();

            //Show preview -- will show whether it can be placed in that spot or not
            ShowPreview(placeLoc);

            //Snap preview to snappable locations

            //Place object
            //TODO:  make sure placeLoc is in playable bounds
            if (Input.GetMouseButtonDown(0) && !StaticMethods.IsPointerOverUIElement())
            {
                if (m_currentObject == null)
                    m_currentObject = Instantiate(m_objectToPlace);

                m_currentObject.Place(placeLoc);
                if (m_currentObject.IsDonePlacing())
                {
                    m_currentObject = null;
                }
                //m_objectToPlace.Place(placeLoc);
            }

        }
        else if(m_destroy)
        {
            GameObject toDestroy = GetObjectAtMouse();

            if(toDestroy && Input.GetMouseButtonDown(0))
            {
                Destroy(toDestroy.gameObject);
            }
        }
    }

    private GameObject GetObjectAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            //Get parent object
            GameObject obj = hit.transform.gameObject;
            if (obj.transform.parent != null)
                obj = obj.transform.parent.gameObject;

            if (obj.CompareTag("Placeable"))
            {
                Debug.Log("Hit Placeable");
                return obj;
            }
        }

        return null;
    }
    public void SetDestroy(bool val)
    {
        Debug.Log("Setting placer destroy to: " + val.ToString());
        m_destroy = val;
    }

    private void ShowPreview(Vector3 placeLoc)
    {
        if (m_preview)
        {
            m_preview.transform.position = placeLoc;
        }
        else
        {
            Debug.Log("Should only see this once");
            m_preview = m_objectToPlace.GetPreview();
            if(m_preview != null)
                StaticMethods.ApplyIgnoreRaycastLayer(m_preview.transform);
        }
    }

    public void ClearPreview()
    {
        m_preview = null;
    }

    public void ClearPlacer()
    {
        if (m_preview != null)
            Destroy(m_preview.gameObject);

        if (m_objectToPlace)
            m_objectToPlace.CancelPlacement();

        m_objectToPlace = null;
        m_preview = null;
        m_destroy = false;
    }

    public void SetObjectToPlace(Placeable obj)
    {
        if (m_objectToPlace)
            m_objectToPlace.CancelPlacement();

        m_objectToPlace = null;
        m_preview = null;
        m_objectToPlace = obj;
    }


    //TODO:
    //  -- Make the height modifier a Placeable method
    Vector3 GetPlaceLoc()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        Vector3 placePos = new Vector3(0, 0, 0);

        if (Physics.Raycast(ray, out hit))
        {
            //Only change y value when shift is held down
            if(Input.GetKey(KeyCode.LeftShift) && m_lastPos.magnitude > 0)
            {
                placePos = m_lastPos;
                if(Input.GetAxis("Mouse Y") > 0)
                {
                    placePos.y += m_heightModifier * Time.deltaTime;
                }
                else if(Input.GetAxis("Mouse Y") < 0)
                {
                    placePos.y -= m_heightModifier * Time.deltaTime;
                }
            }            
            else
            {
                Vector3 offset = m_objectToPlace.transform.localScale;
                placePos = hit.point;
                placePos.y = placePos.y + (offset.y / 2);
            }
            m_lastPos = placePos;
        }

        return placePos;
    }

    int FindSideHit(RaycastHit hit)
    {
        Transform cubeTransform = hit.collider.gameObject.transform;

        //Debug.DrawLine(cubeTransform.position, hit.point, Color.red, 10f);

        float dot = Vector3.Dot(cubeTransform.up, hit.normal);

        float angle = Vector3.SignedAngle(hit.normal, transform.right, Vector3.up);

        if (dot == 1) //top
            return 1;

        else if (dot == -1) //bottom
            return -1;

        else if (angle == 0) //"right" side
            return 2;

        else if (angle == -90) //"front" side
            return 3;

        else if (angle == 180) //"left" side
            return 4;

        else if (angle == 90) //"back" side
            return 5;


        return 0;
    }

}
