using System;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour, IFillBarProvider
{
    [SerializeField] private FillBar fillBar;
    [SerializeField] private GameObject[] guns;
    [SerializeField] public Material ultaLight;
    [SerializeField] public GameObject ultaLightSwitcher;
    private Weapon[][] allWeapons = new Weapon[3][];
    
    public Weapon CurrentWeapon { get; private set; }
    private int countCurrentWeapon = 0;
    public float MaxValue => CurrentWeapon.isUltaActive ? CurrentWeapon.UltaTime : ShouldEnemiesDieCount;
    public float CurrentValue => CurrentWeapon.isUltaActive ? CurrentWeapon.СurUltaTime : curEnemiesDieCount;

    public int ShouldEnemiesDieCount {get; private set;}
    private int curEnemiesDieCount;
    private int talkSetIsUltaActiveCount = 0;
    public bool CanUlta => ShouldEnemiesDieCount <= curEnemiesDieCount;

    private void Start()
    {
        ShouldEnemiesDieCount = 10;
        curEnemiesDieCount = ShouldEnemiesDieCount / 2;
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
        countCurrentWeapon = 0;
        for (var gunIndex = 0; gunIndex < guns.Length; gunIndex++)
        {
            foreach (var weapon in allWeapons[gunIndex])
            {
                weapon.enabled = shouldEnablePredicate(weapon);
                if (!weapon.enabled) continue;
                
                countCurrentWeapon++;
                if (fullRecharge) weapon.FullRecharge();
                
                if (isFirst) continue;
                isFirst = true;
                CurrentWeapon = weapon;
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
    
    public void PourInUlta(int value)
    {
        //if (CurrentWeapon.isUltaActive) return;
        curEnemiesDieCount += value;
    }

    public void SetIsUltaActive()
    {
        talkSetIsUltaActiveCount++;
        if (talkSetIsUltaActiveCount != countCurrentWeapon) return;
        
        talkSetIsUltaActiveCount = 0;
        curEnemiesDieCount = 0;
    }
}