using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuffWeapon : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f;
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

    private void Update()
    {
        RotateAround();
    }

    private void RotateAround()
    {
        transform.Rotate(Vector3.up * (rotationSpeed * GameModel.UnscaledDeltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            SwitchWeapon();
            Destroy(gameObject);
        }
    }
    
    private void SwitchWeapon()
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
