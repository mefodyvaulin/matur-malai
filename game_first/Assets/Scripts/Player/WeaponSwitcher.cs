using System;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private FillBar fillBar;
    [SerializeField] private GameObject[] guns;

    private Weapon[][] allWeapons = new Weapon[3][];

    private void Start()
    {
        GameModel.SetWeaponSwitcher(this);
        for (var i = 0; i < guns.Length; i++)
        {
            allWeapons[i] = guns[i].GetComponents<Weapon>();
        }
        SetWeapon<BulletGun>();
    }

    private void SetWeaponInternal(Func<Weapon, bool> shouldEnablePredicate, bool fullRecharge)
    {
        var isFirst = false;
        for (var gunIndex = 0; gunIndex < guns.Length; gunIndex++)
        {
            foreach (var weapon in allWeapons[gunIndex])
            {
                weapon.enabled = shouldEnablePredicate(weapon);
                if (weapon.enabled && fullRecharge) weapon.FullRecharge();
                else if (isFirst) continue;
                isFirst = true;
                fillBar.SetProvider(weapon);
            }
        }
    }
    
    public void SetWeapon<T>(bool fullRecharge = true) where T : Weapon
    {
        SetWeaponInternal(w => w is T, fullRecharge);
    }

    public void SetWeapon(System.Type weaponType, bool fullRecharge = true)
    {
        SetWeaponInternal(w => w.GetType() == weaponType, fullRecharge);
    }

    
    public System.Type DisableAllAndGetActiveWeaponType()
    {
        System.Type activeWeaponType = null;

        for (var gunIndex = 0; gunIndex < guns.Length; gunIndex++)
        {
            foreach (var weapon in allWeapons[gunIndex])
            {
                if (weapon.enabled && activeWeaponType == null)
                {
                    activeWeaponType = weapon.GetType();
                }
                weapon.enabled = false;
            }
        }
        
        fillBar.SetProvider(null);

        return activeWeaponType;
    }
}

