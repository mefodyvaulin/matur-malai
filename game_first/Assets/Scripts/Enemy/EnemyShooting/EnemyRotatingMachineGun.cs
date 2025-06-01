using System;
using System.Collections;
using UnityEngine;

public class EnemyRotatingMachineGun : EnemyShooting
{
    private float[] angles = { -14f, 14f };
    private float[] anglesShoot = { -2.15f, 0, 2.15f };
    private float duration = 1.5f;
    private float preparationTime = 0.75f;
    private float rotateZTime = 1.5f;
    private float allTime;
    const int shotsPerCycle = 10;
    
    private Quaternion cycleOriginalRotation;
    private EnemyAbstract enemy;
    private Coroutine shootCoroutine;
    protected override void Awake()
    {
        allTime = (duration + preparationTime * 2 + rotateZTime) * 4 + preparationTime * 4;
        fireRate = allTime + 1;
        minFireRate = allTime + 1;
        base.Awake();
    }

    protected override void Animation(EnemyAbstract enemy)
    {
        base.Animation(enemy);
        this.enemy = enemy;
        cycleOriginalRotation = startRotation;

        StartCoroutine(RotateSequenceCoroutine());
    }

    protected override void Shoot()
    {
        shootCoroutine = StartCoroutine(ShootOnAngleCoroutine());
    }
    
    protected override void StopShoot()
    {
        StopCoroutine(shootCoroutine);
    }

    private IEnumerator RotateSequenceCoroutine()
    {
        for (var i = 0; i < 4; i++)
        {
            // 1) Поворот к стартовому углу из angles
            yield return StartCoroutine(RotateToOffset(angles[0], preparationTime));
            // 2) Плавное вращение по углам angles за duration
            yield return StartCoroutine(RotateOverDuration(duration));
            // 3) Поворот обратно к нулевому углу
            yield return StartCoroutine(RotateToOffset(0f, preparationTime));

            yield return Wait(preparationTime);
            // 4) Поворот дрона на 90 градусов вокруг оси Z
            yield return StartCoroutine(RotateZBy(90f, rotateZTime));

            // Обновляем базовую ориентацию для следующего прохода
            cycleOriginalRotation = enemy.transform.rotation;
            // Сбросить флаги выстрелов, чтобы снова можно было стрелять по всем углам
        }
    }

    private IEnumerator RotateToOffset(float angleOffset, float time)
    {
        var start = enemy.transform.rotation;
        var target = cycleOriginalRotation * Quaternion.Euler(angleOffset, 0f, 0f);
        var elapsed = 0f;

        while (elapsed < time)
        {
            if (Time.timeScale == 0)
            {
                yield return null;
                continue;
            }
            elapsed += GameModel.UnscaledDeltaTime;
            var t = elapsed / time;
            enemy.transform.rotation = Quaternion.Slerp(start, target, t);
            yield return null;
        }

        enemy.transform.rotation = target;
    }

    private IEnumerator RotateOverDuration(float totalTime)
    {
        var elapsed = 0f;
        while (elapsed < totalTime)
        {
            if (Time.timeScale == 0)
            {
                yield return null;
                continue;
            }
            elapsed += GameModel.UnscaledDeltaTime;
            var t = elapsed / totalTime;
            var currentAngle = Mathf.Lerp(angles[0], angles[^1], t);
            enemy.transform.rotation = cycleOriginalRotation * Quaternion.Euler(currentAngle, 0f, 0f);
            yield return null;
        }

        enemy.transform.rotation = cycleOriginalRotation * Quaternion.Euler(angles[^1], 0f, 0f);
    }

    private IEnumerator ShootOnAngleCoroutine()
    {
        var shootPhaseDuration = duration + preparationTime * 2;
        var timeBetweenShots = shootPhaseDuration / shotsPerCycle;

        for (var i = 0; i < 4; i++) // 4 поворота на 90°
        {
            for (var shotIndex = 0; shotIndex < shotsPerCycle; shotIndex++)
            {
                if (Time.timeScale == 0)
                {
                    yield return null;
                    continue;
                }
                foreach (var angle in anglesShoot)
                {
                    var rotationWithOffset = transform.rotation * Quaternion.Euler(0f, angle, 0f);
                    Instantiate(bulletPrefab, transform.position, rotationWithOffset);
                }
                yield return Wait(timeBetweenShots);
            }
            foreach (var angle in anglesShoot)
            {
                var rotationWithOffset = transform.rotation * Quaternion.Euler(0f, angle, 0f);
                Instantiate(bulletPrefab, transform.position, rotationWithOffset);
            }
            yield return Wait(preparationTime + rotateZTime);
        }
    }

    
    private IEnumerator RotateZBy(float deltaZ, float time)
    {
        var start = enemy.transform.rotation;
        var target = start * Quaternion.Euler(0f, 0f, deltaZ);
        var elapsed = 0f;

        while (elapsed < time)
        {
            if (Time.timeScale == 0)
            {
                yield return null;
                continue;
            }
            elapsed += GameModel.UnscaledDeltaTime;
            var t = elapsed / time;
            enemy.transform.rotation = Quaternion.Slerp(start, target, t);
            yield return null;
        }

        enemy.transform.rotation = target;
    }

    private static IEnumerator Wait(float time)
    {
        var elapsed = 0f;
        while (elapsed < time)
        {
            if (Time.timeScale == 0)
            {
                yield return null;
                continue;
            }
            elapsed += GameModel.UnscaledDeltaTime;
            yield return null;
        }
    }
}