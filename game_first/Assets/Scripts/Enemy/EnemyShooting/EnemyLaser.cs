using System;
using UnityEngine;

public class EnemyLaser : EnemyShooting
{
    private LaserShoot laserShoot;
    private int damgePerSecond = 5;
    protected override void Awake()
    {
        fireRate = 2.7f;
        minFireRate = 0.5f;
        laserShoot = bulletPrefab.GetComponent<LaserShoot>();
        if (laserShoot == null) throw new Exception("LaserShoot component doesn't exist");
        laserShoot.damagePerSecondShoot = damgePerSecond + 1 * GameModel.Harder;
        laserShoot.timeMaxAdd += 0.3f * GameModel.Harder;
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