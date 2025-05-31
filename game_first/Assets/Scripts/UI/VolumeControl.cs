using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] private string volumeParameter;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider slider;
    private const float _multiplier = 20f;
    private float _volumeValue;

    private void Awake()
    {
        _volumeValue = PlayerPrefs.GetFloat(volumeParameter, Mathf.Log10(slider.value) * _multiplier);
        slider.value = Mathf.Pow(10f, _volumeValue / _multiplier);
        slider.onValueChanged.AddListener(HandleSliderValueChange);
    }

    private void Start()
    {
        mixer.SetFloat(volumeParameter, _volumeValue);
    }

    private void HandleSliderValueChange(float value)
    {
        _volumeValue = value <= 0.0001f ? -80f : Mathf.Log10(value) * _multiplier;
        mixer.SetFloat(volumeParameter, _volumeValue);
        PlayerPrefs.SetFloat(volumeParameter, _volumeValue);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat(volumeParameter, _volumeValue);
    }
}