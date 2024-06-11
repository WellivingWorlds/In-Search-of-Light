using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHand2 : MonoBehaviour
{
    private Transform hand; // Reference to the Hand object
    private Vector3 lastHandPosition; // To store the previous position of the Hand

    void Start()
    {
        // Start the coroutine to find the Hand object
        StartCoroutine(FindHandObject());
    }

    IEnumerator FindHandObject()
    {
        while (hand == null)
        {
            // Try to find the Hand object by tag
            GameObject handObject = GameObject.FindWithTag("Hand");
            if (handObject != null)
            {
                hand = handObject.transform;
                lastHandPosition = hand.position; // Initialize lastHandPosition with Hand's initial position
            }
            yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds before trying again
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
