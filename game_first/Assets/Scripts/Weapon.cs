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
    protected float LastFireTime;                             // Последний выстрел для кд между патронами
    protected float ReloadTimer;                              // Таймер для кд
    private UserInputAction _weaponInputAction;
    private InputAction _movement;
    
    public int CurrentClip => currentClip;
    public int MaxClip => maxClip;

    protected void Awake()
    {
        _weaponInputAction = new UserInputAction();
    }

    protected virtual void Start() // для [SerializeField]
    {
        currentClip = maxClip;
    }

    protected virtual void OnEnable()
    {
        // если какую-то пушку захочется сделать на другую кнопку нужно переопределять этот метод,
        // в котором обращаться к другой кнопке оружия (предварительно, создав ее в InputMap)
        // если таки пушек будет больше половины, лучше сделать абстрактным
        _movement = _weaponInputAction.Weapon.WeaponMovement;
        _movement.Enable();
    }

    protected void OnDisable()
    {
        _movement.Disable();
    }

    protected void Update()
    {
        // Логика перезарядки
        Recharge();

        // Непрерывная стрельба при удержании
        if (_movement.IsPressed())
            Shoot();
    }

    protected abstract void Recharge(); // Метод перезарядки должна реализовать каждая конкретная пушка
    protected abstract void Shoot(); // Метод стрельбы  реализовать каждая конкретная пушка
}
