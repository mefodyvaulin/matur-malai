using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class BulletGun : Weapon
{
    protected override void Start()
    {
        base.Start();
        UltaTime = 1f;
    }

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

    protected override void Ulta()
    {
        StartCoroutine(UltaCoroutine());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator UltaCoroutine()
    {
        for (var i = 0; i < 5; i++)
        {
            foreach (var enemy in GetRandomizedEnemiesOrNulls())
            {
                var bulletObj = Instantiate(bulletPrefab, transform.position, transform.rotation);
                var rocket = bulletObj.GetComponent<Rocket>();
                if (rocket is not null)
                {
                    rocket.target = enemy;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private static Enemy[] GetRandomizedEnemiesOrNulls()
    {
        if (GameModel.Enemies.Keys.Count > 0) 
            return GameModel.Enemies.Keys
                .OrderBy(_ => Guid.NewGuid())
                .ToArray();
        return new Enemy[] { null, null, null };        
    }
}