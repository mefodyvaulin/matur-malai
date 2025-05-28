using UnityEngine;

public class EnemyGun : EnemyShooting
{
    protected override void Shoot()
    {
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
    
    protected override void StopShoot()
    {
    }
    
    protected override void Animation(EnemyAbstarct enemy)
    {
    }
}