using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class EnemyAbstarct : MonoBehaviour
{
    [SerializeField] public EnemyHealth health;
    [SerializeField] public EnemyMovement movement;
    [SerializeField] private EnemyShooting[] shootings;
    public int ShootingsCount => shootings.Length;
    protected float deathTime;
    
    protected virtual void Awake()
    {
        //deathTime = health.audioSources[1].clip.length;
        GameModel.AddEnemy(this);
    }

    protected virtual void OnDestroy()
    {
        GameModel.RemoveEnemy(this);
    }

    protected virtual void Update()
    {
        if (!health.IsAlive)
        {
            EnableAllShootings(false);
            movement.enabled = false;
            Destroy(gameObject, deathTime);
            return;
        }

        movement.Move?.Invoke(this);
    }

    protected virtual void EnableAllShootings(bool enable)
    {
        foreach (var shooting in shootings)
        {
            shooting.enabled = enable;
        }
    }

    public virtual void UpdateAllShootings(float rate = -1, int indexAnimation = 0, bool animation = true)
    {
        if (!health.IsAlive) return;
        shootings[indexAnimation].UpdateShootAnimation(this, rate, animation);
        foreach (var shooting in shootings)
        {
            shooting.UpdateShooting(rate, animation);
        }
    }

    public virtual void UpdateShootings(IEnumerable<int> indexes, float rate = -1, int indexAnimation = 0, bool animation = true)
    {
        if (!health.IsAlive) return;
        var indexesArr = indexes as int[] ?? indexes.ToArray();
        shootings[indexesArr[indexAnimation]].UpdateShootAnimation(this, rate, animation);
        foreach (var index in indexesArr)
        {
            if (index < 0 || index >= shootings.Length) continue;
            
            shootings[index].UpdateShooting(rate, animation);
        }
    }

    
    public void UpdateShootings(int index, float rate = -1, bool animation = true)
    {
        UpdateShootings(new[] { index }, rate, index, animation);
    }
}