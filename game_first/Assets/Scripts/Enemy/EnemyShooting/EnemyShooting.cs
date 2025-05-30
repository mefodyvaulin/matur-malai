using System;
using System.Collections;
using UnityEngine;

public abstract class EnemyShooting : MonoBehaviour
{
    [SerializeField] protected GameObject bulletPrefab;
    
    protected float fireRate = 1f; // Задержка между выстрелами
    protected float minFireRate = 0f;
    
    protected float lastShootTime;
    protected float lastAnimationTime;
    
    protected Vector3 startPosition;
    protected Quaternion startRotation;

    private float returnTime = 1f;

    protected virtual void Awake()
    {
        lastShootTime = -fireRate;
        lastAnimationTime = -fireRate;
    }

    public virtual void UpdateShooting(float rate = -1, bool animate = true)
    {
        if (!animate)
        {
            StopShoot();
            return;
        }
        if (rate < minFireRate)
            rate = fireRate;
        
        if (!(GameModel.UnscaledTime >= lastShootTime + rate)) return;
        lastShootTime = GameModel.UnscaledTime;
        Shoot();
    }

    public virtual void UpdateShootAnimation(EnemyAbstract enemy, float rate = -1, bool animate = true)
    {
        if (!animate)
        {
            StopShootAnimation(enemy);
            return;
        }
        if (rate < minFireRate)
            rate = fireRate;
        
        if (!(GameModel.UnscaledTime >= lastAnimationTime + rate)) return;
        lastAnimationTime = GameModel.UnscaledTime;
        Animation(enemy);
    }
    
    protected abstract void Shoot();
    protected abstract void StopShoot();

    protected virtual void Animation(EnemyAbstract enemy)
    {
        startPosition = enemy.transform.position;
        startRotation = enemy.transform.rotation;
    }

    private void StopShootAnimation(EnemyAbstract enemy)
    {
        StopAllCoroutines();
        StartCoroutine(ReturnToStartCoroutine(enemy));
    }

    private IEnumerator ReturnToStartCoroutine(EnemyAbstract enemy)
    {
        var fromPos = enemy.transform.position;
        var fromRot = enemy.transform.rotation;

        var elapsed = 0f;

        while (elapsed < returnTime)
        {
            elapsed += Time.unscaledDeltaTime;
            var t = Mathf.Clamp01(elapsed / returnTime);
            
            var z = enemy.transform.position.z;
            var newX = Mathf.Lerp(fromPos.x, startPosition.x, t);
            var newY = Mathf.Lerp(fromPos.y, startPosition.y, t);
            enemy.transform.position = new Vector3(newX, newY, z);
            enemy.transform.rotation = Quaternion.Slerp(fromRot, startRotation, t);

            yield return null;
        }

        enemy.transform.position = new Vector3(startPosition.x, startPosition.y, enemy.transform.position.z);
        enemy.transform.rotation = startRotation;
    }
}

