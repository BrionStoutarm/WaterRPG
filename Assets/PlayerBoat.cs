using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoat : MonoBehaviour
{
    enum Heading { 
        AHEAD,
        PORT,
        STARB,
        H_PORT,
        H_STARB
    }
    float m_currentSpeed = 0;
    Heading m_currentHeading = Heading.AHEAD;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ExecuteMove() {

    }
}
