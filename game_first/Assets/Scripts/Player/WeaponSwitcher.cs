using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private FillBar fillBar;
    [SerializeField] private GameObject[] guns;

    private Weapon[][] allWeapons = new Weapon[3][];

    void Start()
    {
        GameModel.SetWeaponSwitcher(this);
        for (var i = 0; i < guns.Length; i++)
        {
            allWeapons[i] = guns[i].GetComponents<Weapon>();
        }
        SetWeapon<BulletGun>();
    }

    public void SetWeapon<T>() where T : Weapon
    {
        var isFirst = false;
        for (var gunIndex = 0; gunIndex < guns.Length; gunIndex++)
        {
            foreach (var weapon in allWeapons[gunIndex])
            {
                weapon.enabled = weapon is T;
                
                if (!weapon.enabled || isFirst) continue;
                isFirst = true;
                fillBar.SetProvider(weapon);
            }
        }
    }
}

