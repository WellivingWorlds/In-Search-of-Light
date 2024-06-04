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
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        // Get the active camera
        Camera activeCamera = Camera.main; // Assuming the main camera is the active camera
        Time.fixedDeltaTime = 0.02f;

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
            Vector3 cameraRight = activeCamera.transform.right;
            Vector3 cameraUp = activeCamera.transform.up;
            targetPosition = planePoint + Vector3.ProjectOnPlane(targetPosition - planePoint, planeNormal);

            // Smoothly move the sprite towards the target position
            Vector3 newPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime, maxMoveSpeed);

            // Move the Rigidbody2D to the new position
            rb2d.MovePosition(new Vector2(newPosition.x, newPosition.y));
        }

        // Rotate the sprite to match the active Camera's rotation
        transform.rotation = activeCamera.transform.rotation;
    }
}