using UnityEngine;
using UnityEngine.UI;

public class FillBar : MonoBehaviour
{
    [SerializeField] private MonoBehaviour providerScript; // Любой скрипт, реализующий IBarValueProvider
    private IFillBarProvider provider;
    private Image barImage; // Компонент Image с типом Filled на этом объекте

    private void Awake()
    {
        provider = providerScript as IFillBarProvider;
        barImage = GetComponent<Image>();

        if (provider == null)
            Debug.LogError("FillBar: объект не реализует IBarValueProvider!");
    }

    private void Update()
    {
        barImage.fillAmount = provider.CurrentValue / provider.MaxValue;
    }
}


