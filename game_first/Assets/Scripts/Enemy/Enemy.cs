using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public EnemyHealth health;
    [SerializeField] public EnemyMovement movement;
    [SerializeField] public EnemyShooting[] shootings;
    private float deathTime;
    private bool isStartFollow;
    
    private void Awake()
    {
        deathTime = health.audioSources[1].clip.length;
        GameModel.AddEnemy(this);
    }

    private void OnDestroy()
    {
        GameModel.RemoveEnemy(this);
    }

    private void Update()
    {
        if (!health.IsAlive)
        {
            EnableShootings(false);
            movement.enabled = false;
            Destroy(gameObject, deathTime);
            return;
        }

        movement.Move?.Invoke(this);
        if (GameModel.CountEnemies != 1) return;
        if (isStartFollow) return;
        
        StartCoroutine(StartFollow());
    }

    private IEnumerator StartFollow()
    {
        yield return new WaitForSeconds(0.5f);
        if (GameModel.CountEnemies != 1) yield break;
        
        movement.ClearMove();
        movement.Move = movement.MoveFollowerPlayer;
        isStartFollow = true;
    }

    private void EnableShootings(bool enable)
    {
        foreach (var shooting in shootings)
        {
            shooting.enabled = enable;
        }
    }

    public void UpdateShootings(float rate = -1)
    {
        foreach (var shooting in shootings)
        {
            shooting.UpdateShooting(rate);
        }
    }
}