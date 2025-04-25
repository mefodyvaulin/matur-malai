using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform weapon;
    
    private float fireRate = 1f; // Задержка между выстрелами
    private float lastFireTime;    // Таймер для кд
    
    private void Shoot()
    {
        Instantiate(bulletPrefab, weapon.position, transform.rotation);
    }

    public void UpdateShooting()
    {
        if (!(Time.time >= lastFireTime + fireRate)) return;
        lastFireTime = Time.time;
        Shoot();
    }
}

