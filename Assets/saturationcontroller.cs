using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectSaturationController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Range(0, 1)] public float saturation = 1.0f;  // Public field to adjust in the Inspector

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Adjust saturation in real-time
        SetSaturation(saturation);
    }

    public void SetSaturation(float saturation)
    {
        // Ensure saturation is clamped between 0 and 1
        saturation = Mathf.Clamp01(saturation);

        // Get the current color of the sprite
        Color color = spriteRenderer.color;

        // Convert the color to grayscale
        float gray = color.r * 0.299f + color.g * 0.587f + color.b * 0.114f;

        // Blend the original color with the grayscale color
        color.r = Mathf.Lerp(gray, color.r, saturation);
        color.g = Mathf.Lerp(gray, color.g, saturation);
        color.b = Mathf.Lerp(gray, color.b, saturation);

        // Apply the adjusted color back to the sprite
        spriteRenderer.color = color;

        // Debug log to check if method is being called
        Debug.Log($"Saturation set to: {saturation}, Color set to: {color}");
    }
}
