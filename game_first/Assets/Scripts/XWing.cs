using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class XWing : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 10f;          // Скорость движения корабля
    [SerializeField] private float rotationSpeed = 5f;  // Скорость поворота
    [SerializeField] private float maxPitchAngle = 30f;  // Макс. угол наклона вверх/вниз (ось X) от изначального положения
    [SerializeField] private float maxYawAngle = 15f;    // Макс. угол поворота влево/вправо (ось Y) от изначального положения
    private UserInputAction _xWingInputAction;
    private InputAction _movement;

    private void Awake()
    {
        _xWingInputAction = new UserInputAction();
    }

    private void OnEnable()
    {
        _movement = _xWingInputAction.XWing.MouseMovment;
        _movement.Enable();

        _xWingInputAction.XWing.MouseMovment.performed += Rotate;
        _xWingInputAction.XWing.MouseMovment.Enable();
    }

    private void OnDisable()
    {
        _movement.Disable();
        _xWingInputAction.XWing.MouseMovment.Disable();

    }

    public void Update()
    {
        MoveForward();
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    /// <summary>
    /// Плавно поворачивает корабль на заданные углы (ось X и Y).
    /// </summary>
    /// <param name="pitchDelta">Угол смещения наклона вверх/вниз (ось X, -30..30°)</param>
    /// <param name="yawDelta">Угол смещения поворота влево/вправо (ось Y, -15..15°)</param>
    /// <param name="context"></param>
    private void Rotate(InputAction.CallbackContext context)
    {
        var pitchDelta = -context.ReadValue<Vector2>().y;
        var yawDelta = context.ReadValue<Vector2>().x;
        // Нормализуем углы в диапазон -180..180
        var currentRelativeRotationX = NormalizeAngle(transform.rotation.x);
        var currentRelativeRotationY = NormalizeAngle(transform.rotation.y);

        // Вычисляем новые углы с ограничениями
        var newPitch = Mathf.Clamp(
            currentRelativeRotationX + pitchDelta,
            -maxPitchAngle,
            maxPitchAngle
        );

        var newYaw = Mathf.Clamp(
            currentRelativeRotationY + yawDelta,
            -maxYawAngle,
            maxYawAngle
        );

        var newRoll = SolveRoll(newPitch, newYaw);

        // Создаём целевой поворот относительно начальной ориентации
        var targetRotation = Quaternion.Euler(newPitch, newYaw, newRoll);

        // Плавный поворот
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private static float SolveRoll(float pitch, float yaw)
    {
        return -yaw * 0.5f + pitch * 0.1f;
    }

    private static float NormalizeAngle(float angle)
    {
        // Приводим угол к диапазону -180..180
        angle %= 360;
        if (angle > 180) angle -= 360;
        if (angle < -180) angle += 360;
        return angle;
    }
}