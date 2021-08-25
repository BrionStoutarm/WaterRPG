using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Camera m_mainCamera;

    public float moveSpeed = 10f;

    public KeyCode m_cameraLeft = KeyCode.A;
    public KeyCode m_cameraRight = KeyCode.D;
    public KeyCode m_cameraUp = KeyCode.W;
    public KeyCode m_cameraDown = KeyCode.S;
    public KeyCode m_cancel = KeyCode.Escape;
    public KeyCode m_pause = KeyCode.Return;

    public float scrollSpeed = 10f;
    public float sensitivity = 10f;
    public float maxYAngle = 80f;
    private Vector2 currentRotation;

    private void Awake() {
        m_mainCamera = Camera.main;

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(m_cameraLeft)) {
            m_mainCamera.transform.Translate(-m_mainCamera.transform.right * moveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(m_cameraRight)) {
            m_mainCamera.transform.Translate(m_mainCamera.transform.right * moveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(m_cameraDown)) {
            m_mainCamera.transform.Translate(-m_mainCamera.transform.forward * moveSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(m_cameraUp)) {
            m_mainCamera.transform.Translate(m_mainCamera.transform.forward * moveSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetMouseButton(1)) {
            currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
            currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
            currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
            currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
            m_mainCamera.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
        }
    }


    //public void Follow(GameObject obj) {
    //    m_mainCamera.Follow(obj);
    //}

    //public void Center(Vector3 loc) {
    //    m_mainCamera.Center(loc);
    //}

    //public void Unfollow() {
    //    m_mainCamera.Follow(null);
    //}
    //public void CameraLeft() {
    //    m_mainCamera.Left();
    //}
    //public void CameraRight() {
    //    m_mainCamera.Right();
    //}
    //public void CameraUp() {
    //    m_mainCamera.Up();
    //}
    //public void CameraDown() {
    //    m_mainCamera.Down();
    //}

    //public void Left() {
    //    m_startTime = Time.time;
    //    m_toFollow = null;
    //    m_centerPosition.x -= m_moveSpeed;
    //}
    //public void Right() {
    //    m_startTime = Time.time;
    //    m_toFollow = null;
    //    m_centerPosition.x += m_moveSpeed;
    //}
    //public void Up() {
    //    m_startTime = Time.time;
    //    m_toFollow = null;
    //    m_centerPosition.z += m_moveSpeed;
    //}
    //public void Down() {
    //    m_startTime = Time.time;
    //    m_toFollow = null;
    //    m_centerPosition.z -= m_moveSpeed;
    //}
}
