using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class RocketGun : MissileGun
{
    protected override void Start()
    {
        base.Start();
        UltaTime = 4f;
        fireRate = 0.5f;
        maxClip = 10;
        reloadCooldown = 1f;
    }
    
    protected override void Ulta()
    {
        StartCoroutine(UltaCoroutine());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator UltaCoroutine()
    {
        for (var i = 0; i < 2; i++)
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
