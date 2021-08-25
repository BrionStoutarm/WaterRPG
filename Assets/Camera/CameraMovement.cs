using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 100f;
    public float scrollSpeed = 10f;


    public float sensitivity = 10f;
    public float maxYAngle = 80f;
    private Vector2 currentRotation;

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
        }

        if(Input.GetKey("d"))
        {
            transform.Translate(transform.right * moveSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("a"))
        {
            transform.Translate(-transform.right * moveSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("s"))
        {
            transform.Translate(-transform.forward * moveSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            transform.position += scrollSpeed * new Vector3(0, -Input.GetAxis("Mouse ScrollWheel"), 0);
        }


        if(Input.GetMouseButton(1))
        {
            currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
            currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
            currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
            currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
            Camera.main.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
        }
    }
}
