using System;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour, IFillBarProvider
{
    [SerializeField] protected float fireRate = 0.25f;        // Задержка между выстрелами
    [SerializeField] protected GameObject bulletPrefab;       // Снаряд (нужно определять для каждого оружия отдельно в [SerializeField])
    [SerializeField] protected int maxClip = 20;              // размер обоймы
    [SerializeField] protected float reloadCooldown = 0.5f;   // Обновляет один патрон в reloadCooldown секунд
    [SerializeField] protected int currentClip;               // Текущая обойма
    protected float LastFireTime;                             // Последний выстрел для кд между патронами
    protected float ReloadTimer;                              // Таймер для кд
    protected float UltaTime;
    private float curUltaTime;
    protected bool isUltaActive;
    protected bool isBuffShooting = false;
    
    public float MaxValue => maxClip;
    public float CurrentValue => currentClip;


    protected virtual void Start() // для [SerializeField]
    {
        currentClip = maxClip;
    }

    protected virtual void OnEnable()
    {
        // если какую-то пушку захочется сделать на другую кнопку нужно переопределять этот метод,
        // в котором обращаться к другой кнопке оружия (предварительно, создав ее в InputMap)
        // если таки пушек будет больше половины, лучше сделать абстрактным
        InputManager.LeftClick.Enable();
        InputManager.Ulta.Enable();
        isUltaActive = false;
    }
    

    protected virtual void Update()
    {
        Recharge();
        
        if (InputManager.Ulta.IsPressed() && !isUltaActive)
        {
            curUltaTime = UltaTime;
            isUltaActive = true;
            Ulta();
        }
        if (isUltaActive)
        {
            curUltaTime -= GameModel.UnscaledDeltaTime;
            if (curUltaTime <= 0)
            {
                isUltaActive = false;
            }
            if (!isBuffShooting) return;
        }
        if (InputManager.LeftClick.IsPressed())
            Shoot();
    }

    protected abstract void Recharge(); // Метод перезарядки должна реализовать каждая конкретная пушка
    protected abstract void Shoot(); // Метод стрельбы  реализовать каждая конкретная пушка
    protected abstract void Ulta(); // УЛЬТА
}
