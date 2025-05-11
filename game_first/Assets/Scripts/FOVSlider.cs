using UnityEngine;
using UnityEngine.UI;

public class FOVSlider : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; 
    [SerializeField] private Slider fovSlider;
    
    void Start()
    {
        mainCamera.fieldOfView = fovSlider.value;
        fovSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnSliderValueChanged(float value)
    {
        mainCamera.fieldOfView = value;
    }
}

