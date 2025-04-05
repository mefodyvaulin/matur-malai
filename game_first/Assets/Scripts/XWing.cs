using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class XWing : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 10f;          // Скорость полёта вперёд (единиц в секунду)
    [SerializeField] private float rotationSpeed = 5f;   // Скорость поворота корабля (используется в Slerp)
    [SerializeField] private float maxPitchAngle = 30f;  // Максимальный угол наклона вверх/вниз (по оси X)
    [SerializeField] private float maxYawAngle = 30f;    // Максимальный угол поворота влево/вправо (по оси Y)
    [SerializeField] private float sensitivity = 1f;     // Чувствительность мыши — насколько сильно движение мыши влияет на поворот
    [SerializeField] private float centeringSpeed = 10f; // Скорость возврата корабля в нейтральное положение, когда мышь не двигается
    [SerializeField] private Vector2 trenchSizeUpRight = new Vector2(30f, 30f);     // Ограничение тоннеля верхний правый угол
    [SerializeField] private Vector2 trenchSizeDownLeft = new Vector2(0f, 0f);      // Ограничение тоннеля нижний левый угол
    [SerializeField] private Transform nosePoint;

    // === Ввод пользователя ===
    private UserInputAction _xWingInputAction;           // Объект, содержащий все действия ввода (сгенерирован через Input System)
    private InputAction _movement;                       // Конкретное действие — движение мыши (тип Vector2)

    // === Текущие углы поворота ===
    private float currentPitch = 0f;                     // Текущий угол наклона по оси X (вверх/вниз)
    private float currentYaw = 0f;                       // Текущий угол поворота по оси Y (влево/вправо)

    // === Состояние ввода ===
    private Vector2 inputDelta;                          // Изменение положения мыши (ось X/Y)
    private bool hasInput;                               // Флаг: пользователь двигает мышь или нет
    
    private void Awake() // Вызывается при создании объекта
    {
        _xWingInputAction = new UserInputAction();
    }
    
    private void OnEnable() // Активируется, когда объект включается в сцене
    {
        _movement = _xWingInputAction.XWing.MouseMovment;
        _movement.Enable();

        _movement.performed += OnMouseMove;
        _movement.canceled += OnMouseStop;
    }
    
    private void OnDisable() // Вызывается, когда объект выключается или удаляется
    {
        _movement.performed -= OnMouseMove;
        _movement.canceled -= OnMouseStop;

        _movement.Disable();
    }
    
    private void Update() // Главный игровой цикл — вызывается каждый кадр
    {
        MoveForward();
        UpdateRotation();
        ClampPositionInsideTrench();
    }
    
    private void MoveForward() // Постоянное движение вперёд(по Z)
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
    
    private void UpdateRotation() // Обновление углов поворота корабля
    {
        if (hasInput
            && ( trenchSizeDownLeft.y - 0.1f <= transform.position.y && transform.position.y <= trenchSizeUpRight.y + 0.1f )
            && ( trenchSizeDownLeft.x - 0.1f <= transform.position.x && transform.position.x <= trenchSizeUpRight.x + 0.1f ))
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

        // Плавный поворот объекта от текущего положения к целевому
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime); // RotateTowards

    }
    private static float SolveRoll(float pitch, float yaw)
    {
        return -yaw * 1.5f + pitch * 0.5f;
    }
    
    private void ClampPositionInsideTrench()
    {
        var pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, trenchSizeDownLeft.x, trenchSizeUpRight.x);
        pos.y = Mathf.Clamp(pos.y, trenchSizeDownLeft.y, trenchSizeUpRight.y);

        transform.position = pos;
    }
}
