using System;
using UnityEngine;
using UnityEngine.Audio;using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField]public string volumeParameter;
    public AudioMixer mixer;
    public Slider slider;
    private const float _multiplier = 20f;
    private float _volumeValue;

    private void Awake()
    {
        slider.onValueChanged.AddListener(HandleSliderValueChange);
    }

    private void Start()
    {
        _volumeValue = PlayerPrefs.GetFloat(volumeParameter, Mathf.Log10(slider.value) * _multiplier);
        slider.value = Mathf.Pow(10f, _volumeValue / _multiplier);
    }

    private void HandleSliderValueChange(float value)
    {
        _volumeValue = Mathf.Log10(value) * _multiplier;
        mixer.SetFloat(volumeParameter, _volumeValue);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeParameter, _volumeValue);
    }
}

