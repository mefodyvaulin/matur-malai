using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;
    [SerializeField] public EnemyMovement movement;
    [SerializeField] private EnemyShooting shooting;
    
    
    private void Awake()
    {
        GameModel.AddEnemy(this);
    }

    private void OnDestroy()
    {
        GameModel.RemoveEnemy(this);
    }

    private void Update()
    {
        if (!health.IsAlive) return;
        movement.Move?.Invoke(this);
        
        if (GameModel.CountEnemies != 1) return;
        movement.ClearMove();
        movement.Move += movement.MoveFollowerPlayer;
    }
}