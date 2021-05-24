using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;
public class Placeable : MonoBehaviour
{
    public Color startColor;
    public Color highlightColor;

    private bool m_isDonePlacing;
    // Start is called before the first frame update
    void Start()
    {
        m_isDonePlacing = true;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public virtual void Place(Vector3 position)
    {
        Debug.Log("Inside baseclass Place");
        this.transform.position = position;
        IsDonePlacing(true);
    }

    public virtual Transform GetPreview()
    {
        Transform obj = Instantiate(this.transform);
        return obj;
    }

    public virtual void CancelPlacement()
    {
        IsDonePlacing(true);
    }

    public virtual void DestroyThis()
    {
        Destroy(this.gameObject);
    }


    public void IsDonePlacing(bool val)
    {
        m_isDonePlacing = val;
    }

    public bool IsDonePlacing()
    {
        return m_isDonePlacing;
    }    
}
