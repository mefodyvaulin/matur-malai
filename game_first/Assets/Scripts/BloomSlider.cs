using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class BloomSlider : MonoBehaviour
{
    [SerializeField] private PostProcessVolume postProcessVolume; 
    [SerializeField] private Slider bloomSlider; 

    private Bloom bloomEffect;

    void Start()
    {
        if (postProcessVolume.profile.TryGetSettings(out bloomEffect))
        {
            bloomEffect.intensity.value = bloomSlider.value;
            bloomSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    void OnSliderValueChanged(float value)
    {
        if (bloomEffect != null)
            bloomEffect.intensity.value = value;
    }
}

