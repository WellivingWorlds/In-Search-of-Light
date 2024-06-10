using UnityEngine;

public class GlassController : MonoBehaviour
{
    public Transform hand;
    public float targetYCoordinate;
    public float jumpHeight = 2f;
    public float jumpWidth = 5f;
    public float jumpDuration = 1f;
    public float gravityT = 0.5f; // Adjustable parameter for gravity effect

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

        controlPoint = new Vector2(transform.position.x + jumpDirection * (jumpWidth / 2), transform.position.y + jumpHeight);
        endPoint = new Vector2(transform.position.x + jumpDirection * jumpWidth, transform.position.y);
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
