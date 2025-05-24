using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public EnemyHealth health;
    [SerializeField] public EnemyMovement movement;
    [SerializeField] public EnemyShooting shooting;
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
            shooting.enabled = false;
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
}