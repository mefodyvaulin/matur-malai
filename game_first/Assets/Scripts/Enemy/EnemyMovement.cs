using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    
    public int direction;

    [SerializeField] private float distanceToEnemy;

    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float maxRotationAngle = 15f;
    
    private float? previousZ = null;

    public Action<Enemy> Move;

    private void Awake()
    {
        direction = Random.Range(0, 2) == 1 ? 1 : -1;
        distanceToEnemy = Random.Range(25f, 35f);
    }

    public void MoveFollowerPlayer(Enemy enemy)
    {
        var player = GameModel.PlayerPosition;
        transform.position = Vector3.Lerp(enemy.transform.position,
            new Vector3(player.x,
                        player.y,
                        player.z + distanceToEnemy),
            speed * GameModel.UnscaledDeltaTime * 0.3f);
        if (Mathf.Abs(player.x - (transform.position.x)) < 0.01f
            || Mathf.Abs(player.x - (transform.position.x)) < 0.01f)
            enemy.shooting.UpdateShooting(0.3f);
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