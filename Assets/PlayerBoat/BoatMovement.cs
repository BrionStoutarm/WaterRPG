using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    // Movement speed in units per second.
    public float lerpSpeed = 5.0F;

    // How far the boat moves for each movement action
    // - affected by boat speed
    public int moveDistanceModifier = 10;

    // Time when the movement started.
    private float startTime;

    // Total distance between the markers.
    private float journeyLength;

    private Vector3 startPosition, endPosition;
    private bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        //will remove later when we flesh out the control scheme
        if(!isMoving) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                GoForward();
            }

            if(Input.GetKeyDown(KeyCode.LeftArrow)) {
                TurnLeft();
            }

            if(Input.GetKeyDown(KeyCode.RightArrow)) {
                TurnRight();
            }
        }

        if (isMoving) {
            Debug.Log("Boat moving");
            // Distance moved equals elapsed time times speed..
            float distCovered = (Time.time - startTime) * lerpSpeed;

            // Fraction of journey completed equals current distance divided by total distance.
            float fractionOfJourney = distCovered / journeyLength;

            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);

            if (transform.position == endPosition) {
                isMoving = false;
            }
        }

    }

    public void GoForward() {
        startTime = Time.time;
        endPosition = (transform.position + transform.forward) * moveDistanceModifier;
        journeyLength = Vector3.Distance(transform.position, endPosition);

        Debug.DrawLine(transform.position, endPosition, Color.red, 100f);


        isMoving = true;
    }

    public void TurnLeft() {
        transform.Rotate(0, -25f, 0);
    }

    public void TurnRight() {
        transform.Rotate(0, 25f, 0);
    }

    void OnDrawGizmos() {
        // Draw a yellow sphere at the transform's position
        Vector3 fwd = transform.position + (transform.forward * 20);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.localPosition, fwd);
        Gizmos.DrawSphere(fwd, 5f);
    }
}
