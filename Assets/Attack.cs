using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour
{
    public float followDuration = 5.0f; // Time to follow Motte2 before moving down
    public float moveDownSpeed = 2.0f; // Speed at which Hand moves down on the y-axis
    public float waitTime = 2.0f; // Time to wait when colliding with Tisch
    public float moveBackSpeed = 2.0f; // Speed at which Hand moves back to initial y-coordinate
    public float followSpeed = 3.0f; // Speed at which Hand follows Motte2
    public float smoothTime = 0.3f; // Time for the smoothing effect

    private bool isFollowing = true; // Flag to determine if Hand should follow Motte2
    private bool isWaiting = false; // Flag to determine if Hand is waiting after collision
    private float followTime; // Time when following started
    private Transform motte2; // Reference to the Motte2 object
    private Vector3 initialPosition; // Initial position of Hand
    private Vector3 velocity = Vector3.zero; // Current velocity, used by SmoothDamp

    void Start()
    {
        initialPosition = transform.position; // Record the initial position
        followTime = Time.time; // Record the time when following started
    }

    void Update()
    {
        // Try to find Motte2 if it has not been assigned
        if (motte2 == null)
        {
            GameObject motte2Object = GameObject.FindWithTag("Motte2");
            if (motte2Object != null)
            {
                motte2 = motte2Object.transform;
            }
        }

        if (isWaiting) return; // If waiting, do nothing

        if (motte2 != null && isFollowing)
        {
            // Follow Motte2 on the x-axis smoothly using SmoothDamp
            Vector3 targetPosition = new Vector3(motte2.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

            // Check if the follow duration has passed
            if (Time.time - followTime >= followDuration)
            {
                isFollowing = false;
            }
        }
        else if (!isFollowing)
        {
            // Move down on the y-axis using MoveTowards
            Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y - moveDownSpeed * Time.deltaTime, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveDownSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tisch"))
        {
            // Stop moving when colliding with Tisch and start the waiting process
            isWaiting = true;
            StartCoroutine(WaitAndReset());
        }
    }

    private IEnumerator WaitAndReset()
    {
        // Wait for the specified wait time
        yield return new WaitForSeconds(waitTime);

        // Move back to the initial y-coordinate using MoveTowards
        while (Mathf.Abs(transform.position.y - initialPosition.y) > 0.01f)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, initialPosition.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveBackSpeed * Time.deltaTime);
            yield return null;
        }

        // Snap to the exact initial position to avoid any minor discrepancies
        transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z);

        // Reset variables to start the behavior from the beginning
        isFollowing = true;
        followTime = Time.time;
        isWaiting = false;
    }
}
