using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class RocketGun : MissileGun
{
    private Rocket rocketPrefab;
    protected override void Start()
    {
        base.Start();
        UltaTime = 4f;
        fireRate = 0.5f;
        maxClip = 10;
        reloadCooldown = 1f;
        rocketPrefab = bulletPrefab.GetComponent<Rocket>();
    }
    
    protected override void Ulta()
    {
        StartCoroutine(UltaCoroutine());
    }
    
    private IEnumerator UltaCoroutine()
    {
        for (var i = 0; i < 4; i++)
        {
            foreach (var enemy in GetRandomizedEnemiesOrNulls())
            {
                var rocket = Instantiate(rocketPrefab, transform.position, transform.rotation);
                rocket.target = enemy;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private static EnemyAbstarct[] GetRandomizedEnemiesOrNulls()
    {
        if (GameModel.Enemies.Keys.Count > 0) 
            return GameModel.Enemies.Keys
                .OrderBy(_ => Guid.NewGuid())
                .ToArray();
        return new EnemyAbstarct[] { null, null, null };        
    }
}
