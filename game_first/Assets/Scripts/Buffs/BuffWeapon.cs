using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuffWeapon : AbstractBuff
{
    private WeaponType weaponType;
    
    private void Start()
    {
        weaponType = (WeaponType)Random.Range(0, System.Enum.GetValues(typeof(WeaponType)).Length);
        
        var rend = GetComponent<Renderer>();
        if (rend is null) return;
        rend.material.color = weaponType switch
        {
            WeaponType.Laser => new Color32(62, 173, 62, 255),
            WeaponType.Bullet => new Color32(255, 129, 200, 255),
            WeaponType.Rocket => new Color32(140, 92, 233, 255),
            _ => rend.material.color
        };
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
