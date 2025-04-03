using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    private UserInputAction _cameraAction;
    private InputAction _movement;

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
        var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        transform.rotation = target.rotation; // пока что камер просто повторяет вращение самолета
    }
}