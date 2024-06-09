using UnityEngine;

public class Follower : MonoBehaviour
{
    private Transform hand; // Reference to the Hand object

    void Start()
    {
        // Find the Hand object by tag
        GameObject handObject = GameObject.FindWithTag("Hand");
        if (handObject != null)
        {
            hand = handObject.transform;
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