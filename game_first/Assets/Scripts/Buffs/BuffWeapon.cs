using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuffWeapon : AbstractBuff
{
    private WeaponType weaponType;
    
    private void Start()
    {
        weaponType = (WeaponType)Random.Range(0, System.Enum.GetValues(typeof(WeaponType)).Length);
        
        // Получаем Renderer
        var rend = GetComponent<Renderer>();
        if (rend is null) return;
        rend.material.color = weaponType switch
        {
            WeaponType.Laser => Color.red,
            WeaponType.Bullet => Color.green,
            WeaponType.Rocket => Color.cyan,
            _ => rend.material.color
        };
    }
    
    protected override void DoBuff()
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
    }
}

public enum WeaponType
{
    Bullet,
    Rocket,
    Laser
}
