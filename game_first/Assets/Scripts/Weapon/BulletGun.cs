using UnityEngine;

public class BulletGun : MissileGun
{
    protected override void Start()
    {
        base.Start();
        UltaTime = 30f;
        isBuffShooting = true;
    }

    protected override void Shoot()
    {
        if (!(GameModel.UnscaledTime >= LastFireTime + fireRate && currentClip > 0)) return;
        if (!isUltaActive) return;
        
        float[] angles = { -5f, -2.5f, 2.5f,  5f };
        foreach (var angle in angles)
        {
            var rotationWithOffset = Quaternion.Euler(transform.rotation.eulerAngles.x + angle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            Instantiate(bulletPrefab, transform.position, rotationWithOffset);
        }
        base.Shoot();
    }
    
    protected override void Ulta()
    {
    }
}
