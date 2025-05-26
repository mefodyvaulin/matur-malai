using UnityEngine;

public abstract class EnemyShooting : MonoBehaviour
{
    [SerializeField] protected GameObject bulletPrefab;
    
    protected float fireRate = 1f; // Задержка между выстрелами
    protected float minFireRate = 0f;
    private float lastFireTime;  // Таймер для кд

    public virtual void UpdateShooting(float rate = -1)
    {
        if (rate < minFireRate)
            rate = fireRate;
        
        if (!(GameModel.UnscaledTime >= lastFireTime + rate)) return;
        lastFireTime = GameModel.UnscaledTime;
        Shoot();
    }

    protected abstract void Shoot();
}

