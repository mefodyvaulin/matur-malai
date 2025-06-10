using System.Collections;
using UnityEngine;

public class EnemyGun : EnemyShooting
{
    protected override void Awake()
    {
        fireRate = 1f + GameModel.Harder * 0.3f;
        minFireRate = 0f + GameModel.Harder * 0.3f;
        base.Awake();
    }

    protected override void Shoot()
    {
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        for (var i = 0; i < 1 + GameModel.Harder; i++)
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(0.3f);
        }
    }
    
    protected override void StopShoot()
    {
    }
}