using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour
{
    private Transform hand; // Reference to the Hand object

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
            }
            yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds before trying again
        }
    }

    void Update()
    {
        if (hand != null)
        {
            // Directly copy the x-coordinate of the Hand
            transform.position = new Vector3(hand.position.x, transform.position.y, transform.position.z);
        }
    }
}
