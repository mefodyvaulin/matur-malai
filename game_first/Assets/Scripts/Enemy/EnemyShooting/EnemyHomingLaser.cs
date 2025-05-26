using System;
using System.Collections;
using UnityEngine;

public class EnemyHomingLaser : EnemyShooting
{
    private LaserBeam laserBeam;
    private float homingTime = 5f;
    private GameObject target;
    private float rotationSpeed = 5f;
    private void Awake()
    {
        fireRate = homingTime * 1.5f;
        minFireRate = homingTime + 0.5f;
        target = GameModel.PlayerMovement.gameObject;
        laserBeam = bulletPrefab.GetComponent<LaserBeam>();
        if (laserBeam == null) throw new Exception("LaserBeam component doesn't exist");
        laserBeam.width = 0.3f;
    }

    protected override void Shoot()
    {
        laserBeam.gameObject.SetActive(true);
        StartCoroutine(HomingCoroutine());
    }

    private IEnumerator HomingCoroutine()
    {
        var elapsedTime = 0f;
        while (elapsedTime < homingTime)
        {
            elapsedTime += GameModel.UnscaledDeltaTime;

            var direction = target.transform.position - transform.position;
            if (direction == Vector3.zero)
            {
                yield return null;
                continue;
            }

            var targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * GameModel.UnscaledDeltaTime
            );
            yield return null;
        }
        transform.localRotation = Quaternion.identity;;
        laserBeam.gameObject.SetActive(false);
    }
}