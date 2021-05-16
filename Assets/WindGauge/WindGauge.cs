using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindGauge : MonoBehaviour
{ 
    private GameManager m_gameManager;
    // Start is called before the first frame update
    void Start()
    {
        m_gameManager = GameManager.Get();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentAngle = transform.eulerAngles;
        currentAngle = new Vector3(currentAngle.x, currentAngle.y, m_gameManager.Weather().WindAngle());
        UnityEngine.UI.Text txt = GetComponentInChildren<UnityEngine.UI.Text>();
        txt.text = ((int)m_gameManager.Weather().WindSpeed()).ToString();
        transform.eulerAngles = currentAngle;
    }
}
