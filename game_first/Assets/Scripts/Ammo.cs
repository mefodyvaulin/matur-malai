using UnityEngine;
using UnityEngine.UI;

public class Ammo : MonoBehaviour
{
    [SerializeField] private Image ammoBar; // Ссылка на UI Image с типом Filled
    [SerializeField] private BulletGun bulletGun; // Ссылка на ваш скрипт оружия

    private void Update()
    {
        // Предполагаем, что BulletGun имеет публичные свойства или методы для получения текущего заряда и максимального заряда
        ammoBar.fillAmount = (float) bulletGun.CurrentClip / bulletGun.MaxClip;
    }
}