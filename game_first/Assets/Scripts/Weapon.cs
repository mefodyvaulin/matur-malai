using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float fireRate = 0.25f;        // Задержка между выстрелами
    [SerializeField] protected GameObject bulletPrefab;       // Снаряд (нужно определять для каждого оружия отдельно в [SerializeField])
    [SerializeField] protected int maxClip = 20;              // размер обоймы
    [SerializeField] protected float reloadCooldown = 0.5f;     // Обновляет один патрон в reloadCooldown секунд
    [SerializeField] protected int currentClip;               // Текущая обойма
    protected float lastFireTime;                             // Последний выстрел для кд между патронами
    protected float reloadTimer;                              // Таймер для кд
    
    private void Start() // для [SerializeField]
    {
        currentClip = maxClip;
    }
    
    protected virtual void Update()
    {
        if (currentClip < maxClip)
        {
            reloadTimer += Time.deltaTime; // Добавляем время, прошедшее с последнего кадра
            if (reloadTimer >= reloadCooldown)
            {
                currentClip++;
                reloadTimer = 0f;
            }
        }
        if (!Mouse.current.leftButton.isPressed) return;
        Shoot();
    }
    
    protected abstract void Shoot(); // Метод должен реализовать каждая конкретная пушка
}
