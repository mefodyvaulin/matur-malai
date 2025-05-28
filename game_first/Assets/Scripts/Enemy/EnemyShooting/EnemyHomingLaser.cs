using System;
using System.Collections;
using UnityEngine;

public class EnemyHomingLaser : EnemyShooting
{
    private LaserBeam laserBeam;
    private float homingTime = 5f;
    private GameObject target;
    private float rotationSpeed = 5f;

    private float shakeAmplitude = 0.1f;
    private float shakeFrequency = 20f;

    private Vector3 originalLocalPosition;
    
    private Coroutine shootCoroutine;

    private void Awake()
    {
        fireRate = homingTime * 1.5f;
        minFireRate = homingTime + 0.5f;
        target = GameModel.PlayerMovement.gameObject;

        laserBeam = bulletPrefab.GetComponent<LaserBeam>();
        if (laserBeam == null) throw new Exception("LaserBeam component doesn't exist");
        laserBeam.width = 0.3f;
        base.Awake();
    }

    protected override void Shoot()
    {
        laserBeam.gameObject.SetActive(true);
        shootCoroutine = StartCoroutine(HomingCoroutine());
    }
    
    protected override void StopShoot()
    {
        StopCoroutine(shootCoroutine);
        laserBeam.gameObject.SetActive(false);
    }

    private IEnumerator HomingCoroutine()
    {
        var elapsedTime = 0f;

        while (elapsedTime < homingTime)
        {
            elapsedTime += GameModel.UnscaledDeltaTime;

            // Наведение
            var direction = (target.transform.position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * GameModel.UnscaledDeltaTime
                );
            }
            yield return null;
        }
        transform.localRotation = Quaternion.identity;
        laserBeam.gameObject.SetActive(false);
    }
    
    protected override void Animation(EnemyAbstarct enemy)
    {
        base.Animation(enemy);
        originalLocalPosition = enemy.transform.localPosition;
        StartCoroutine(HomingAnimation(enemy));
    }
    
    private IEnumerator HomingAnimation(EnemyAbstarct enemy)
    {
        var elapsedTime = 0f;
        while (elapsedTime < homingTime)
        {
            elapsedTime += GameModel.UnscaledDeltaTime;

            // Тряска по X и Y (переменная со временем, чтобы не было резких прыжков)
            var shakeX = Mathf.PerlinNoise(GameModel.UnscaledTime * shakeFrequency, 0f) * 2f - 1f;
            var shakeY = Mathf.PerlinNoise(0f, GameModel.UnscaledTime * shakeFrequency) * 2f - 1f;

            var currentZ = enemy.transform.localPosition.z;
            transform.localPosition = new Vector3(
                originalLocalPosition.x + shakeX * shakeAmplitude,
                originalLocalPosition.y + shakeY * shakeAmplitude,
                currentZ
            );
            yield return null;
        }
        transform.localPosition = originalLocalPosition;
    }
}