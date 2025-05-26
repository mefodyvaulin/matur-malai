using System;
using UnityEngine;

public class EnemyLaser : EnemyShooting
{
    private LaserShoot laserShoot;
    private void Awake()
    {
        fireRate = 2.7f;
        minFireRate = 0.5f;
        laserShoot = bulletPrefab.GetComponent<LaserShoot>();
        if (laserShoot == null) throw new Exception("LaserShoot component doesn't exist");
    }

    protected override void Shoot()
    {
        laserShoot.gameObject.SetActive(true);
    }
    
    
}