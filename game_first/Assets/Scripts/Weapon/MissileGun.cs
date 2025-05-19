public abstract class MissileGun : Weapon
{
    protected override void Recharge()
    {
        if (currentClip >= maxClip) return;
        ReloadTimer += GameModel.UnscaledDeltaTime;
        if (!(ReloadTimer >= reloadCooldown)) return;
        currentClip++;
        ReloadTimer = 0f;
    }

    protected override void Shoot()
    {
        if (!(GameModel.UnscaledTime >= LastFireTime + fireRate && currentClip > 0)) return;
        Instantiate(bulletPrefab, transform.position, transform.rotation);
        currentClip--;
        LastFireTime = GameModel.UnscaledTime;
    }
}