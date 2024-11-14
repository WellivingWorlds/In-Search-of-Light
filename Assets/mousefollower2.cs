using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AC;

public class MouseFollower : MonoBehaviour
{
    public float maxMoveSpeed = 10f;
    public float smoothTime = 0.3f;
    public float minDistance = 1f;
    public float fixedDistanceFromCamera = 5f; // Set this to the desired distance from the camera

    private Vector3 currentVelocity = Vector3.zero;
    private Animator animator; // Reference to the Animator component
    private Vector3 previousPosition; // To track the previous position of the object

    void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();

        // Initialize previous position
        previousPosition = transform.position;
    }

    void Update()
    {
        // Get the active camera
        Camera activeCamera = Camera.main; // Assuming the main camera is the active camera

        if (activeCamera == null)
        {
            return;
        }

        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert mouse position to a ray
        Ray ray = activeCamera.ScreenPointToRay(mousePosition);

        // Calculate the plane normal and point on the plane based on the camera's orientation
        Vector3 planeNormal = activeCamera.transform.forward;
        Vector3 planePoint = activeCamera.transform.position + planeNormal * fixedDistanceFromCamera;

        // Create the plane
        Plane plane = new Plane(planeNormal, planePoint);

        // Raycast to find the point on the plane where the mouse is pointing
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 worldMousePosition = ray.GetPoint(distance);

            // Calculate the direction and target position with the offset to maintain minimum distance
            Vector3 direction = (worldMousePosition - transform.position).normalized;
            Vector3 targetPosition = worldMousePosition - direction * minDistance;

            // Ensure the target position is within the 2D plane parallel to the camera's orientation
            targetPosition = planePoint + Vector3.ProjectOnPlane(targetPosition - planePoint, planeNormal);

            // Smoothly move the sprite towards the target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime, maxMoveSpeed);
        }

        // Rotate the sprite to match the active Camera's rotation
        transform.rotation = activeCamera.transform.rotation;

        // Check if the object is moving downwards
        CheckMovementDirection();

        // Update the previous position for the next frame
        previousPosition = transform.position;
    }

    private void CheckMovementDirection()
    {
        Vector3 currentPosition = new Vector3(transform.position.x, transform.position.y);
        currentPosition.z = 0; // Ignore Z-axis if irrelevant
        Vector3 movementDelta = currentPosition - previousPosition;

        // Tolerance to detect if the object is not moving
        float tolerance = 100f; // Experiment with different values
        int movementDirection = 0; // Default to idle

        // Check if the magnitude of movement or velocity is above a small threshold (indicating movement)
        if (movementDelta.sqrMagnitude > tolerance * tolerance || currentVelocity.magnitude > 5f)
        {
            float angle = Mathf.Atan2(movementDelta.y, movementDelta.x) * Mathf.Rad2Deg;

            // Determine direction based on angle
            if (angle >= -22.5f && angle < 22.5f)
            {
                movementDirection = 3; // Right
            }
            else if (angle >= 22.5f && angle < 67.5f)
            {
                movementDirection = 4; // Up-Right
            }
            else if (angle >= 67.5f && angle < 112.5f)
            {
                movementDirection = 2; // Up
            }
            else if (angle >= 112.5f && angle < 157.5f)
            {
                movementDirection = 5; // Up-Left
            }
            else if (angle >= -67.5f && angle < -22.5f)
            {
                movementDirection = 8; // Down-Right
            }
            else if (angle >= -112.5f && angle < -67.5f)
            {
                movementDirection = 1; // Down
            }
            else if (angle >= -157.5f && angle < -112.5f)
            {
                movementDirection = 7; // Down-Left
            }
            else
            {
                movementDirection = 6; // Left
            }
        }

        // Set the animator parameter
        animator.SetInteger("MovementDirection", movementDirection);

        // Update the previous position
        previousPosition = transform.position;
    }
}
