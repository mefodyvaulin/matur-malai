using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    private void Start()
    {
        if (UserInput.userInput != null)
            UserInput.userInput.MoveMouse += Rotate;
        transform.LookAt(target);
    }

    private void Rotate(float y, float x)
    {
        transform.rotation = Quaternion.Euler(y, x, 0);
    }

    void FixedUpdate()
    {
        var desiredPosition = target.position + offset;
        var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}