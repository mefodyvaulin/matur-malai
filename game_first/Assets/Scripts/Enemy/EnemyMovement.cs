using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    private static readonly int Wait = Animator.StringToHash("Wait");
    private static readonly int Mans = Animator.StringToHash("Mans");
    [SerializeField] private float speed = 10f;
    
    public int direction;

    [SerializeField] private float distanceToEnemy;

    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float maxRotationAngle = 15f;
    private Animator animator;
    private float? previousZ = null;

    public Action<Enemy> Move;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        direction = Random.Range(0, 2) == 1 ? 1 : -1;
        distanceToEnemy = Random.Range(25f, 35f);
    }

    public void MoveFollowerPlayer(Enemy enemy)
    {
        animator.SetLayerWeight(1, 1);
        var player = GameModel.PlayerPosition;
        if (Mathf.Abs(player.x - GameModel.Player.trenchSizeUpRight.x) < 1f ||
            Mathf.Abs(player.x - GameModel.Player.trenchSizeDownLeft.x) < 1f)
        {
            animator.SetBool(Wait, true);
            animator.SetBool(Mans, false);
        }
        else
        {
            animator.SetBool(Wait, false);
            animator.SetBool(Mans, true);
        }
        transform.position = Vector3.Lerp(enemy.transform.position,
            new Vector3(player.x,
                        player.y,
                        player.z + distanceToEnemy),
            speed * GameModel.UnscaledDeltaTime * 0.3f);

        enemy.shooting.UpdateShooting(0.5f);
    }

    public void DefaultMove()
    {
        Move += MoveBack;
    }
    
    private void MoveBack(Enemy enemy)
    {
        var currentZ = GameModel.PlayerPosition.z;

        if (!previousZ.HasValue)
        {
            previousZ = currentZ;
            return;
        }

        var deltaZ = currentZ - previousZ.Value;
        previousZ = currentZ;

        transform.Translate(Vector3.back * deltaZ);
    }

    public void ClearMove()
    {
        Move = null;
    }
}