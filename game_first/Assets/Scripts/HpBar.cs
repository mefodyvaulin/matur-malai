using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private XWing xWing; // Ссылка на скрипт оружия
    private Image ammoBar; // Компонент Image с типом Filled на этом объекте

    private void Awake()
    {
        // Получаем компонент Image на этом же объекте
        ammoBar = GetComponent<Image>();
    }

    private void Update()
    {
        ammoBar.fillAmount = (float) xWing.CurrentHp / xWing.MaxHp;
    }
}
