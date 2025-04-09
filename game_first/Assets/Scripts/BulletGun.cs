using UnityEngine;

public class BulletGun : Weapon
{
    protected override void Recharge()
    {
        if (currentClip >= maxClip) return;
        ReloadTimer += Time.deltaTime;
        if (!(ReloadTimer >= reloadCooldown)) return;
        currentClip++;
        ReloadTimer = 0f;
    }

    protected override void Shoot()
    {
        if (!(Time.time >= LastFireTime + fireRate && currentClip > 0)) return;
        Instantiate(bulletPrefab, transform.position, transform.rotation);
        currentClip--;
        LastFireTime = Time.time;
    }
}