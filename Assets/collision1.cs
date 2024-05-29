using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicStopOnCollision : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the kinematic object

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null || rb.bodyType != RigidbodyType2D.Kinematic)
        {
            Debug.LogError("This script requires the object to have a kinematic Rigidbody2D.");
        }
    }

    void Update()
    {
        // Example of moving the kinematic object; replace with your actual movement logic
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DynamicObject")) // Ensure the dynamic object has this tag
        {
            // Stop the kinematic object
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;

            // Optionally, you can stop further movement by disabling this script
            // this.enabled = false;
        }
    }
}

