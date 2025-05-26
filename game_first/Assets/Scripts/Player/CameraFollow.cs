using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothTime = 0.3f; // Время сглаживания
    private Vector3 velocity = Vector3.zero; // Текущая скорость камеры

    private void Awake()
    {
        GameModel.SetCameraFollow(this);
    }

    private void LateUpdate()
    {
        var desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        //transform.rotation = target.rotation; // пока что камер просто повторяет вращение самолета
    }
}