using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuffWeapon : AbstractBuff
{
    public GameObject laserRendererObj;
    public GameObject bulletRendererObj;
    public GameObject rocketRendererObj;

    private WeaponType weaponType;

    private void Start()
    {
        weaponType = (WeaponType)Random.Range(0, System.Enum.GetValues(typeof(WeaponType)).Length);

        switch (weaponType)
        {
            case WeaponType.Laser:
                laserRendererObj.SetActive(true);
                break;
            case WeaponType.Bullet:
                bulletRendererObj.SetActive(true);
                break;
            case WeaponType.Rocket:
                rocketRendererObj.SetActive(true);
                break;
        }
    }
    
    protected override IEnumerator DoBuff()
    {
        switch (weaponType)
        {
            case WeaponType.Bullet:
                GameModel.WeaponSwitcher.SetWeapon<BulletGun>();
                break;
            case WeaponType.Rocket:
                GameModel.WeaponSwitcher.SetWeapon<RocketGun>();
                break;
            case WeaponType.Laser:
                GameModel.WeaponSwitcher.SetWeapon<LaserWeapon>();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        yield break;
    }
}

public enum WeaponType
{
    Bullet,
    Rocket,
    Laser
}
