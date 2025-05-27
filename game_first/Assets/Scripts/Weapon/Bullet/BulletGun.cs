using UnityEngine;

public class BulletGun : MissileGun
{
    private float[] angles = { -5f, -2.5f, 2.5f, 5f };
    protected override Color UltaColor => Color.red;

    protected override void Start()
    {
        base.Start();
        UltaTime = 30f;
        isBuffShooting = true;
        fireRate = 0.25f;
        maxClip = 20;
        reloadCooldown = 0.5f;

    }

    protected override void Shoot()
    {
        if (!(GameModel.UnscaledTime >= LastFireTime + fireRate && currentClip > 0)) return;
        if (isUltaActive)
        {
            foreach (var angle in angles)
            {
                var rotationWithOffset = Quaternion.Euler(transform.rotation.eulerAngles.x + angle,
                    transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                Instantiate(bulletPrefab, transform.position, rotationWithOffset);
            }
        }
        base.Shoot();

    }
    
    protected override void Ulta()
    {
    }
}
