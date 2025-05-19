using UnityEngine;
using UnityEngine.UI;

public class FOVSlider : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; 
    [SerializeField] private Slider fovSlider;

    private void Start()
    {
        mainCamera.fieldOfView = fovSlider.value;
        fovSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        mainCamera.fieldOfView = value;
    }
}

