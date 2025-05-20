using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public EnemyHealth health;
    [SerializeField] public EnemyMovement movement;
    [SerializeField] public EnemyShooting shooting;
    private Animator animator;
    private bool startFollow;
    
    private void Awake()
    {
        GameModel.AddEnemy(this);
        animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        GameModel.RemoveEnemy(this);
    }

    private void Update()
    {
        if (!health.IsAlive)
        {
            animator.enabled = false;
            shooting.enabled = false;
            movement.enabled = false;
            Destroy(gameObject, health.audioSources[1].clip.length);
            return;
        }

        movement.Move?.Invoke(this);
        if (GameModel.CountEnemies != 1) return;
        if (startFollow) return;
        animator.SetLayerWeight(1, 1);
        movement.ClearMove();
        movement.Move = movement.MoveFollowerPlayer;
        startFollow = true;
    }

}