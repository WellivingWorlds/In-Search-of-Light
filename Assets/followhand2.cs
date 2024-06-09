using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Followhand2 : MonoBehaviour
{
    private Transform hand; // Reference to the Hand object
    private Vector3 lastHandPosition; // To store the previous position of the Hand

    void Start()
    {
        // Find the Hand object by tag
        GameObject handObject = GameObject.FindWithTag("Hand");
        if (handObject != null)
        {
            hand = handObject.transform;
            lastHandPosition = hand.position; // Initialize lastHandPosition with Hand's initial position
        }
    }

    void Update()
    {
        if (hand != null)
        {
            // Calculate the difference in Hand's position since the last frame
            Vector3 handDelta = hand.position - lastHandPosition;

            // Apply the same delta to the follower object
            transform.position += handDelta;

            // Update lastHandPosition to the current position of the Hand
            lastHandPosition = hand.position;
        }
    }
}