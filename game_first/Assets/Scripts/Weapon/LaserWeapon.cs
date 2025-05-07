using UnityEngine;
using UnityEngine.InputSystem;

public class LaserWeapon : Weapon
{
    [SerializeField] private LaserBeam laserBeam;
    private bool isFiring;

    protected override void Start()
    {
        base.Start();
        if (laserBeam is null)
        {
            Debug.LogError("laserBeam не назначен!");
        }
        laserBeam?.gameObject.SetActive(false); // Лазер по умолчанию выключен
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        InputManager.LeftClick.performed += OnFirePressed;
        InputManager.LeftClick.canceled += OnFireReleased;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        InputManager.LeftClick.performed -= OnFirePressed;
        InputManager.LeftClick.canceled -= OnFireReleased;
        laserBeam?.gameObject.SetActive(false); // отключаем лазер при выключении оружия
    }

    private void OnFirePressed(InputAction.CallbackContext context)
    {
        isFiring = true;
    }

    private void OnFireReleased(InputAction.CallbackContext context)
    {
        isFiring = false;
        laserBeam?.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();

        if (isFiring)
            Shoot();
    }

    protected override void Shoot()
    {
        if (currentClip <= 0)
        {
            laserBeam?.gameObject.SetActive(false); // отключаем лазер
            return;
        }

        if (Time.time - LastFireTime < fireRate)
            return;

        LastFireTime = Time.time;
        
        if (!laserBeam.gameObject.activeSelf)
        {
            laserBeam.gameObject.SetActive(true);
        }

        currentClip--;
    }

    protected override void Recharge()
    {
        if (currentClip >= maxClip)
            return;

        ReloadTimer += Time.deltaTime;
        if (ReloadTimer >= reloadCooldown)
        {
            currentClip++;
            ReloadTimer = 0f;
        }
    }
}
