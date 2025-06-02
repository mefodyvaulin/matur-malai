using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class LaserWeapon : Weapon
{
    [SerializeField] private LaserBeam laserBeam;
    [SerializeField] private float ultaTime = 10f;
    private float defaultWidth;
    private int defaultDamage;
    private float defaultInterval;

    public override Color UltaColor => new Color32(62, 173, 62, 255);

    protected override void Start()
    {
        base.Start();
        if (laserBeam is null)
        {
            Debug.LogError("laserBeam не назначен!");
        }
        else
        {
            UltaTime = 10f;
            defaultWidth = laserBeam.width;
            defaultDamage = laserBeam.damagePerSecond;
            defaultInterval = laserBeam.damageInterval;
            laserBeam.gameObject.SetActive(false); // по умолчанию лазер выключен
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        InputManager.LeftClick.canceled += OnFireReleased;
    }

    protected void OnDisable()
    {
        InputManager.LeftClick.canceled -= OnFireReleased;
        laserBeam?.gameObject.SetActive(false); // отключаем лазер при выключении оружия
    }

    private void OnFireReleased(InputAction.CallbackContext context)
    { 
        if (!isUltaActive)
            laserBeam?.gameObject.SetActive(false);
    }

    protected override void Shoot()
    {
        if (currentClip <= 0)
        {
            laserBeam.gameObject.SetActive(false); // отключаем лазер
            return;
        }

        if (GameModel.UnscaledTime - LastFireTime < fireRate)
            return;

        LastFireTime = GameModel.UnscaledTime;

        if (!laserBeam.gameObject.activeSelf)
        {
            laserBeam.gameObject.SetActive(true);
        }

        currentClip--;
    }

    protected override void Ulta()
    {
        laserBeam.width = 2f;
        laserBeam.damagePerSecond = 15;
        laserBeam.damageInterval = 0.4f;
        if (!laserBeam.gameObject.activeSelf) 
        {
            laserBeam.gameObject.SetActive(true);
        }
        StartCoroutine(WaitAndDisableUlta());
    }

    private IEnumerator WaitAndDisableUlta()
    {
        while (isUltaActive)
        {
            yield return null;
        }
        laserBeam.gameObject.SetActive(false);
        laserBeam.width = defaultWidth;
        laserBeam.damagePerSecond = defaultDamage;
        laserBeam.damageInterval = defaultInterval;
    }

    protected override void Recharge()
    {
        if (currentClip >= maxClip)
            return;

        ReloadTimer += GameModel.UnscaledDeltaTime;
        if (ReloadTimer >= reloadCooldown)
        {
            currentClip++;
            ReloadTimer = 0f;
        }
    }
}
