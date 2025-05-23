using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public EnemyHealth health;
    [SerializeField] public EnemyMovement movement;
    [SerializeField] public EnemyShooting shooting;
    private float deathTime;
    private bool startFollow;
    
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
        if (startFollow) return;

        movement.ClearMove();
        movement.Move = movement.MoveFollowerPlayer;
        startFollow = true;
    }

}