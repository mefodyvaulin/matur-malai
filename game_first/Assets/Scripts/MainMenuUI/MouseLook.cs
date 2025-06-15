using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1f; // Скорость вращения
    [SerializeField] private float maxPitchAngle = 30f;
    
    private Vector2 mouseDelta;
    private float rotationX = 0f;
    private float rotationY = 0f;
    private Quaternion initialRotation;
    
    [SerializeField] private float returnDelay = 5f;
    [SerializeField] private float returnSpeed = 1f;
    private float timeSinceLastInput = 0f;
    private bool isReturning = false;
    
    [SerializeField] private RectTransform mouseZone;
    [SerializeField] private Canvas canvas;
    private bool isInActiveZone;


    private void Start()
    {
        PlayerMovement.sensivity = PlayerPrefs.GetFloat("sensivity");
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        var mousePosition = Mouse.current.position.ReadValue();

        isInActiveZone = isInActiveZone || RectTransformUtility.RectangleContainsScreenPoint(
            mouseZone, mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera
        );
        
        if (isInActiveZone && Mouse.current.leftButton.isPressed)
        {
            mouseDelta = Mouse.current.delta.ReadValue();

            rotationY -= mouseDelta.x * rotationSpeed * PlayerMovement.sensivity;
            rotationX -= mouseDelta.y * rotationSpeed * PlayerMovement.sensivity;
            //rotationX = Mathf.Clamp(rotationX, -46f, 4f);

            timeSinceLastInput = 0f;
            isReturning = false;
        }
        else
        {
            isInActiveZone = false;
            timeSinceLastInput += Time.deltaTime;
            if (timeSinceLastInput >= returnDelay) isReturning = true;
            if (isReturning)
            {
                rotationX = Mathf.Lerp(rotationX, 0f, Time.deltaTime * returnSpeed);
                rotationY = Mathf.Lerp(rotationY, 0f, Time.deltaTime * returnSpeed);
            }
        }

        var xQuat = Quaternion.AngleAxis(rotationX, Vector3.right);
        var yQuat = Quaternion.AngleAxis(rotationY, Vector3.up);
        transform.rotation = initialRotation * yQuat * xQuat;
    }
    /*
    void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            mouseDelta = Mouse.current.delta.ReadValue();
            var rotationAmount = (-1) * mouseDelta.x * rotationSpeed;
            transform.Rotate(0, rotationAmount, 0);
        }
    }
    */
}


