using UnityEngine;

public class BulletGun : Weapon
{
    protected override void Shoot()
    {
        if (!(Time.time >= lastFireTime + fireRate && currentClip > 0)) return;
        Instantiate(bulletPrefab, transform.position, transform.rotation);
        currentClip--;
        lastFireTime = Time.time;
    }
}
