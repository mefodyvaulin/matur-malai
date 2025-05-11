using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public EnemyHealth health;
    [SerializeField] public EnemyMovement movement;
    [SerializeField] public EnemyShooting shooting;


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
        if (!health.IsAlive)
        {
            GetComponent<Animator>().enabled = false;
            shooting.enabled = false;
            movement.enabled = false;
            Destroy(gameObject, health.audioSources[1].clip.length);
            return;
        }

        movement.Move?.Invoke(this);
        if (GameModel.CountEnemies != 1) return;
        movement.ClearMove();
        GetComponent<Animator>().SetLayerWeight(1, 1);
        movement.Move += movement.MoveFollowerPlayer;
    }
}