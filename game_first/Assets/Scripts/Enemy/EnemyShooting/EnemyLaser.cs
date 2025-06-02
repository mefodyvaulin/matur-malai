using System;
using UnityEngine;

public class EnemyLaser : EnemyShooting
{
    private LaserShoot laserShoot;
    protected override void Awake()
    {
        fireRate = 2.7f;
        minFireRate = 0.5f;
        laserShoot = bulletPrefab.GetComponent<LaserShoot>();
        if (laserShoot == null) throw new Exception("LaserShoot component doesn't exist");
        base.Awake();
        
        var buffLayer = LayerMask.NameToLayer("Buff");
        var shieldLayer = LayerMask.NameToLayer("LazerWall");
        var buffMask = 1 << buffLayer;
        var shieldMask = 1 << shieldLayer;
        laserShoot.SetLayerMask(~(buffMask | shieldMask));
    }

    protected override void Shoot()
    {
        laserShoot.gameObject.SetActive(true);
    }

    protected override void StopShoot()
    {
    }
}