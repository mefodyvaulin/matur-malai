using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SensivitySlider:MonoBehaviour
{
    [SerializeField] private Slider sensivitySlider;
    private float sensivity = PlayerMovement.sensivity;

    private void Start()
    {
        sensivitySlider.value = PlayerPrefs.GetFloat("sensivity");
        sensivitySlider.onValueChanged.AddListener(HandleSliderValueChange);
    }

    private void HandleSliderValueChange(float value)
    {
        sensivity = value;
        PlayerMovement.UpdateSensitivity(sensivity);
        PlayerPrefs.SetFloat("sensivity", sensivity);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("sensivity", sensivitySlider.value);
    }
}
