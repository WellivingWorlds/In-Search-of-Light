using UnityEngine;

public class GlassController : MonoBehaviour
{
    public Transform hand;
    public float targetYCoordinate;
    public float jumpHeight = 2f;
    public float jumpWidth = 5f;
    public float jumpDuration = 1f;
    public float gravityT = 0.5f; // Adjustable parameter for gravity effect
    public float proximityThreshold = 1f; // Variable proximity threshold
    public float Werkzeugkasten; // X coordinate of Werkzeugkasten

    private Vector2 startPoint;
    private Vector2 controlPoint;
    private Vector2 endPoint;
    private float jumpTime;

    private bool isJumping = false;

    void Update()
    {
        if (isJumping)
        {
            jumpTime += Time.deltaTime;
            float t = jumpTime / jumpDuration;

            if (t > 1f)
            {
                t = 1f;
                isJumping = false;
            }

            transform.position = CalculateGravityBezierPoint(t, startPoint, controlPoint, endPoint, gravityT);
        }
    }

    public void TriggerJump()
    {
        if (!isJumping)
        {
            StartJump();
        }
    }

    void StartJump()
    {
        startPoint = transform.position;
        float jumpDirection = Mathf.Sign(hand.position.x - transform.position.x); // Determine jump direction

        // Determine if the glass is to the right or left of Werkzeugkasten
        bool isRightOfWerkzeugkasten = transform.position.x > Werkzeugkasten;
        bool isHandToTheLeftOfGlass = hand.position.x < transform.position.x;
        bool isHandWithinProximity = Mathf.Abs(hand.position.x - transform.position.x) <= proximityThreshold;

        if (isRightOfWerkzeugkasten)
        {
            if (isHandToTheLeftOfGlass)
            {
                // If the hand is to the left of the glass, allow jumps to the left
                controlPoint = new Vector2(transform.position.x - (jumpWidth / 2), transform.position.y + jumpHeight);
                endPoint = new Vector2(transform.position.x - jumpWidth, transform.position.y);
            }
            else if (isHandWithinProximity || hand.position.x > transform.position.x)
            {
                // If the hand is to the right of or within the proximity threshold of the glass, only jump upwards
                controlPoint = new Vector2(transform.position.x, transform.position.y + jumpHeight);
                endPoint = new Vector2(transform.position.x, transform.position.y);
            }
        }
        else
        {
            // Normal jumping behavior when left of Werkzeugkasten
            if (Mathf.Abs(hand.position.x - transform.position.x) > proximityThreshold)
            {
                controlPoint = new Vector2(transform.position.x + jumpDirection * (jumpWidth / 2), transform.position.y + jumpHeight);
                endPoint = new Vector2(transform.position.x + jumpDirection * jumpWidth, transform.position.y);
            }
            else
            {
                controlPoint = new Vector2(transform.position.x, transform.position.y + jumpHeight);
                endPoint = new Vector2(transform.position.x, transform.position.y);
            }
        }

        jumpTime = 0f;
        isJumping = true;
    }

    Vector2 CalculateGravityBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, float gravityT)
    {
        // Adjust the parameter t to simulate gravity effect
        t = Mathf.Pow(t, gravityT);

        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector2 p = uu * p0; // u^2 * p0
        p += 2 * u * t * p1; // 2 * u * t * p1
        p += tt * p2; // t^2 * p2

        return p;
    }
}
