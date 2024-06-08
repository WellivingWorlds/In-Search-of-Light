using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;

public class MouseFollower3 : MonoBehaviour
{
    public float maxMoveSpeed = 10f;
    public float smoothTime = 0.3f;
    public float minDistance = 1f;

    private Vector3 currentVelocity = Vector3.zero;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D component found on this GameObject.");
        }
    }

    void Update()
    {
        // Get the active camera
        Camera activeCamera = Camera.main; // Assuming the main camera is the active camera

        if (activeCamera == null)
        {
            return;
        }

        // Get the mouse position in world coordinates
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -activeCamera.transform.position.z; // Set z to the distance from the camera
        Vector3 worldMousePosition = activeCamera.ScreenToWorldPoint(mousePosition);

        // Calculate the direction and target position with the offset to maintain minimum distance
        Vector3 direction = (worldMousePosition - transform.position).normalized;
        Vector3 targetPosition = worldMousePosition - direction * minDistance;

        // Ensure the target position is within the bounds of the game world
        targetPosition.z = transform.position.z; // Maintain the same z position for 2D

        // Smoothly move the sprite towards the target position
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime, maxMoveSpeed);
        rb.MovePosition(newPosition);

        // Rotate the sprite to match the active Camera's rotation
        transform.rotation = activeCamera.transform.rotation;
    }
}