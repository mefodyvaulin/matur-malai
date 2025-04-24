using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;
    [SerializeField] public EnemyMovement movement;
    [SerializeField] private EnemyShooting shooting;
    
    public Action<Enemy> Move => movement.Move;
    
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
        Move?.Invoke(this);
    }
}