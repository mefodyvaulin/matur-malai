using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f; // Скорость вращения
    private Vector2 mouseDelta;

    void Update()
    {
        if (Mouse.current.leftButton.isPressed && GameModel.mouseInXwingColliderZone)
        {
            mouseDelta = Mouse.current.delta.ReadValue();
            float rotationAmount = (-1) * mouseDelta.x * rotationSpeed;
            transform.Rotate(0, rotationAmount, 0);
        }
    }
}


