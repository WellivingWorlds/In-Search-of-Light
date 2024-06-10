using UnityEngine;
using System.Collections;

public class Hand : MonoBehaviour
{
    public float followDuration = 5.0f; // Time to follow Motte2 before moving down
    public float moveDownTime = 2.0f; // Time to ease down to the target y-coordinate
    public float waitTime = 2.0f; // Time to wait when reaching the target y-coordinate
    public float easeBackTime = 2.0f; // Time to ease back to initial y-coordinate
    public float followSpeed = 3.0f; // Speed at which Hand follows Motte2
    public float smoothTime = 0.3f; // Time for the smoothing effect
    public float targetYCoordinate = -2.0f; // Target y-coordinate for the Hand to stop moving down

    public GlassController glassController; // Reference to the GlassController script

    private bool isFollowing = true; // Flag to determine if Hand should follow Motte2
    private bool isWaiting = false; // Flag to determine if Hand is waiting after reaching the target y-coordinate
    private float followTime; // Time when following started
    private Transform motte2; // Reference to the Motte2 object
    private Vector3 initialPosition; // Initial position of Hand
    private Vector3 velocity = Vector3.zero; // Current velocity, used by SmoothDamp
    private float moveDownStartTime; // Time when easing down starts
    private float easeBackStartTime; // Time when easing back starts

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
                moveDownStartTime = Time.time; // Record the time when moving down starts
            }
        }
        else if (!isFollowing)
        {
            // Easing down to the target y-coordinate using SmoothDamp
            float elapsedTime = Time.time - moveDownStartTime;
            float t = elapsedTime / moveDownTime;

            // Calculate the smooth step
            float easedY = Mathf.SmoothStep(initialPosition.y, targetYCoordinate, t);

            // Update the Hand's position
            transform.position = new Vector3(transform.position.x, easedY, transform.position.z);

            // Check if the Hand has reached the target y-coordinate
            if (Mathf.Abs(transform.position.y - targetYCoordinate) < 0.01f)
            {
                // Notify the GlassController to trigger the jump
                if (glassController != null)
                {
                    glassController.TriggerJump();
                }

                // Start the waiting process
                isWaiting = true;
                StartCoroutine(WaitAndReset());
            }
        }
    }

    private IEnumerator WaitAndReset()
    {
        // Wait for the specified wait time
        yield return new WaitForSeconds(waitTime);

        // Record the start time for easing back
        easeBackStartTime = Time.time;

        // Easing back to the initial y-coordinate using SmoothDamp
        while (Mathf.Abs(transform.position.y - initialPosition.y) > 0.01f)
        {
            float elapsedTime = Time.time - easeBackStartTime;
            float t = elapsedTime / easeBackTime;

            // Calculate the smooth step
            float easedY = Mathf.SmoothStep(transform.position.y, initialPosition.y, t);

            // Update the Hand's position
            transform.position = new Vector3(transform.position.x, easedY, transform.position.z);

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
