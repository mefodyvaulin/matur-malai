using System;
using System.Collections;
using UnityEngine;

public class EnemyHomingLaser : EnemyShooting
{
    private LaserBeam laserBeam;
    private float homingTime = 10f;
    private GameObject target;
    private float rotationSpeed = 10f;
    
    private Coroutine shootCoroutine;

    protected override void Awake()
    {
        fireRate = homingTime + 2f;
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
        
        var minRotationSpeed = 1f;    
        var maxRotationSpeed = rotationSpeed;

        while (elapsedTime < homingTime)
        {
            elapsedTime += GameModel.UnscaledDeltaTime;

            var toTarget = target.transform.position - transform.position;
            if (toTarget.sqrMagnitude > 0.001f)
            {
                var direction = toTarget.normalized;
                var targetRotation = Quaternion.LookRotation(direction);
                var angle = Quaternion.Angle(transform.rotation, targetRotation);
                
                var t = Mathf.InverseLerp(0f, 20f, angle); 
                t = Mathf.SmoothStep(0f, 1f, t); 
                var currentSpeed = Mathf.Lerp(minRotationSpeed, maxRotationSpeed, t);

                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    currentSpeed * GameModel.UnscaledDeltaTime
                );
            }

            yield return null;
        }

        transform.localRotation = Quaternion.identity;
        laserBeam.gameObject.SetActive(false);
    }
}