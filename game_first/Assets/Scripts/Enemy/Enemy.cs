using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;
    [SerializeField] private EnemyMovement movement;
    [SerializeField] private EnemyShooting shooting;
    public bool canMove = false;
    
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
        if (!canMove) return;
        movement.MoveBack();
    }
}