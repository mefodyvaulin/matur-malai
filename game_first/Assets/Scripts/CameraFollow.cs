using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    private UserInputAction _cameraAction;
    private InputAction _movement;
    public float smoothTime = 0.3f; // Время сглаживания
    private Vector3 velocity = Vector3.zero; // Текущая скорость камеры

    private void Awake()
    {
        _cameraAction = new UserInputAction();
    }

    private void OnEnable()
    {
        _movement = _cameraAction.Camera.MouseMovment;
        _movement.Enable();

        //_cameraAction.Camera.MouseMovment.performed += Rotate;
        _cameraAction.Camera.MouseMovment.Enable();
    }

    private void OnDisable()
    {
        _movement.Disable();
        _cameraAction.Camera.MouseMovment.Disable();

    }


    private void Rotate(InputAction.CallbackContext context)
    {
        // тут можно будет реализовать вращение камеры и подключить его расскоменитв строчку выше
    }

    private void Update()
    {
        var desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        //transform.rotation = target.rotation; // пока что камер просто повторяет вращение самолета
    }
}