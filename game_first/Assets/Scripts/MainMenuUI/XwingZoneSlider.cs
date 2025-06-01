using UnityEngine;
using UnityEngine.InputSystem;

public class XwingZoneSlider : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main; 
    }

    private void Update()
    {
        var mousePosition = Mouse.current.position.ReadValue();
        var ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        Physics.Raycast(ray, out hit);
        if (hit.collider != null && hit.collider.gameObject == gameObject)
            GameModel.mouseInXwingColliderZone = true;
        else
            GameModel.mouseInXwingColliderZone = false;
    }
}

