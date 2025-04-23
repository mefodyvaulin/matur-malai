using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform weapon;
    
    private void Shoot()
    {
        Instantiate(bulletPrefab, weapon.position, transform.rotation);
    }

    public void UpdateShooting()
    {
        // Логика для обновления стрельбы
        Shoot();
    }

    public void BulletExit()
    {
        // Логика для завершения стрельбы
    }
}

