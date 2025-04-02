using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using Vector2 = System.Numerics.Vector2;

public class UserInput: MonoBehaviour
{
    private Vector2 mousePosition;
    public static UserInput userInput;
    public delegate void Rotate(float x, float y);

    public event Rotate MoveMouse;
    void Start()
    {
        userInput = this;
    }

    public void FixedUpdate()
    {
        mousePosition.X += Input.GetAxis("Mouse X");
        mousePosition.Y += Input.GetAxis("Mouse Y");
        MoveMouse?.Invoke(-mousePosition.Y, mousePosition.X);
    }

}