using UnityEngine;
using System.Collections;

public class SchattenController : MonoBehaviour
{
    public Hand handScript; // Reference to the Hand script
    public float startFadeInTime = 2.0f; // Time before Hand starts moving to start fade-in
    public float fadeInDuration = 2.0f; // Duration of the fade-in effect
    public float fadeOutDuration = 2.0f; // Duration of the fade-out effect

    private SpriteRenderer spriteRenderer; // SpriteRenderer component of the Schatten
    private float fadeStartTime; // Time when the fade-in starts
    private float fadeOutStartTime; // Time when the fade-out starts
    private bool isFadingIn = false; // Flag to determine if fading in
    private bool hasStartedFading = false; // Flag to check if fade-in process has started
    private bool isFadingOut = false; // Flag to determine if fading out
    private bool hasStartedFadingOut = false; // Flag to check if fade-out process has started

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ResetSchatten(); // Initialize Schatten state
        StartCoroutine(FindHandObject());
    }

    IEnumerator FindHandObject()
    {
        while (handScript == null)
        {
            GameObject handObject = GameObject.FindWithTag("Hand");
            if (handObject != null)
            {
                handScript = handObject.GetComponent<Hand>();
            }
            yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds before trying again
        }
    }

    void Update()
    {
        if (handScript == null)
            return;

        // Check if Hand has been following and calculate the time to start fading in
        if (!hasStartedFading && handScript.IsFollowing)
        {
            float timeSinceFollowStart = Time.time - handScript.FollowTime;

            // Start fading in at the appropriate time before Hand starts moving down
            if (timeSinceFollowStart >= handScript.followDuration - startFadeInTime)
            {
                fadeStartTime = Time.time;
                isFadingIn = true;
                hasStartedFading = true;
            }
        }

        // Perform the fade-in effect
        if (isFadingIn)
        {
            float elapsedTime = Time.time - fadeStartTime;
            float t = Mathf.Clamp01(elapsedTime / fadeInDuration);

            // Update the opacity
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(0f, 1f, t); // Interpolate opacity from 0% to 100%
            spriteRenderer.color = color;

            // Check if fade-in is complete
            if (t >= 1f)
            {
                isFadingIn = false; // Stop updating once fade-in is complete
            }
        }

        // Check if Hand has started moving back up to its initial position and start fade-out
        if (!hasStartedFadingOut && handScript.IsWaiting && handScript.isEasingBack)
        {
            fadeOutStartTime = Time.time;
            isFadingOut = true;
            hasStartedFadingOut = true;
        }

        // Perform the fade-out effect
        if (isFadingOut)
        {
            float elapsedTime = Time.time - fadeOutStartTime;
            float t = Mathf.Clamp01(elapsedTime / fadeOutDuration);

            // Update the opacity
            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(1f, 0f, t); // Interpolate opacity from 100% to 0%
            spriteRenderer.color = color;

            // Check if fade-out is complete
            if (t >= 1f)
            {
                isFadingOut = false; // Stop updating once fade-out is complete
                ResetSchatten(); // Reset Schatten state to loop the process
            }
        }
    }

    private void ResetSchatten()
    {
        // Reset state variables
        isFadingIn = false;
        hasStartedFading = false;
        isFadingOut = false;
        hasStartedFadingOut = false;

        // Reset the opacity to 0%
        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;
    }
}
