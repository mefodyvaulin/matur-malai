using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Shield : MonoBehaviour, IDamageable
{
    public AudioSource AudioSource;
    
    protected Collider targetCollider;
    protected ICanShield targetShield;

    public int MaxHp => 40;
    public int CurrentHp { get; set; }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public virtual void Init(Collider targetCollider)
    {
        this.targetCollider = targetCollider;

        var target = targetCollider.gameObject;

        targetShield = target.GetComponent<ICanShield>();
        if (targetShield == null) throw new Exception("Target can not shield.");

        targetShield.isIndestructibleShield = true;

        transform.SetParent(target.transform);
        transform.position = targetCollider.bounds.center;
        CurrentHp = MaxHp;
        
        if (AudioSource) AudioSource.Play();
    }

    protected virtual void Die()
    {
        targetShield.isIndestructibleShield = false;
        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        CurrentHp -= damage;
        if (CurrentHp <= 0)
             Die();
    }

    private void LateUpdate()
    {
        if (targetShield == null || !targetCollider || !targetShield.IsAlive) Die();
    }
}