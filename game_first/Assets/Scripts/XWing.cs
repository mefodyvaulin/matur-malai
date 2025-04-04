using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class XWing : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float maxPitchAngle = 30f;
    [SerializeField] private float maxYawAngle = 30f;
    [SerializeField] private float sensitivity = 1f;
    [SerializeField] private float centeringSpeed = 10f; // Скорость возврата в нейтраль

    private UserInputAction _xWingInputAction;
    private InputAction _movement;

    private float currentPitch = 0f;
    private float currentYaw = 0f;

    private Vector2 inputDelta;
    private bool hasInput;

    private void Awake()
    {
        _xWingInputAction = new UserInputAction();
    }

    private void OnEnable()
    {
        _movement = _xWingInputAction.XWing.MouseMovment;
        _movement.Enable();

        _movement.performed += OnMouseMove;
        _movement.canceled += OnMouseStop; // При отпускании мыши
    }

    private void OnDisable()
    {
        _movement.performed -= OnMouseMove;
        _movement.canceled -= OnMouseStop;

        _movement.Disable();
    }

    private void Update()
    {
        MoveForward();
        UpdateRotation();
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    private void OnMouseMove(InputAction.CallbackContext context)
    {
        inputDelta = context.ReadValue<Vector2>() * sensitivity; // Получаем дельту движения мыши (Vector2: x — влево/вправо, y — вверх/вниз).
        hasInput = true;
    }

    private void OnMouseStop(InputAction.CallbackContext context)
    {
        hasInput = false;
    }

    private void UpdateRotation()
    {
        if (hasInput)
        {
            currentPitch = Mathf.Clamp(currentPitch - inputDelta.y, -maxPitchAngle, maxPitchAngle);
            currentYaw = Mathf.Clamp(currentYaw + inputDelta.x, -maxYawAngle, maxYawAngle);
        }
        else
        {
            // Плавный возврат к нейтрали
            currentPitch = Mathf.MoveTowards(currentPitch, 0f, centeringSpeed * Time.deltaTime);
            currentYaw = Mathf.MoveTowards(currentYaw, 0f, centeringSpeed * Time.deltaTime);
        }

        var roll = SolveRoll(currentPitch, currentYaw);
        var targetRotation = Quaternion.Euler(currentPitch, currentYaw, roll);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private static float SolveRoll(float pitch, float yaw)
    {
        return -yaw * 1.5f + pitch * 0.5f;
    }
}
