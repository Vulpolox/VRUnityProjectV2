using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleMovement : MonoBehaviour
{
    public float rotationSpeed = 45f; // Rotation speed in degrees per second
    public float amplitude = 1f;      // Amplitude of the sine wave
    public float frequency = 1f;      // Frequency of the sine wave

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Rotate the object around its up axis
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

        // Calculate vertical movement along the sine wave
        float verticalOffset = amplitude * Mathf.Sin(frequency * Time.time);

        // Apply the vertical movement to the object's position
        transform.position = startPosition + Vector3.up * verticalOffset;
    }
}
