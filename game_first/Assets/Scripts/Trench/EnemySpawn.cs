using System;
using UnityEngine;
using Random = System.Random;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Enemy[] enemies;
    private int spawnedAfter;

    private void Awake()
    {
        Trench.OnGenerateContinuationOfTrench += CountFragmentsToSpawn;
    }

    private void CountFragmentsToSpawn(Trench.TrenchState state)
    {
        spawnedAfter++;
        if (spawnedAfter == 4) // спавн прямо при влете в эту часть туннеля
        {
            SpawnEnemy();
            Trench.OnGenerateContinuationOfTrench -= CountFragmentsToSpawn;
        }
    }

    private void SpawnEnemy()
    {
        // появляется примерно на середине поля, а не облетает туннель
        // нужно реализовать логику подлета
        var indexDron = new Random().Next(enemies.Length);
        var enemy = Instantiate(enemies[indexDron], transform.position, transform.rotation);
        StartMoving(enemy, enemy.transform.position + new Vector3(20, -10, -30));
    }


    private float duration = 2f;

    private void StartMoving(Enemy enemy, Vector3 toPosition)
    {
        StartCoroutine(MoveToPosition(enemy, toPosition));
    }

    private IEnumerator MoveToPosition(Enemy enemy, Vector3 toPosition)
    {
        var startPosition = enemy.transform.position;

        var controlPoint1 = new Vector3(
            (startPosition.x + toPosition.x) * 0.75f,
            2 * startPosition.y - toPosition.y,
            startPosition.z
        );

        var controlPoint2 = new Vector3(
            toPosition.x,
            toPosition.y,
            toPosition.z + (startPosition.z - toPosition.z) * 0.5f
        );

        var elapsed = 0f;
        var previousPosition = startPosition;

        while (elapsed < duration)
        {
            var t = elapsed / duration;
            var curvedPosition = Bezier.Cubic(
                startPosition, controlPoint1, controlPoint2, toPosition, t);

            enemy.transform.position = curvedPosition;

            // Направление между текущей и предыдущей позицией
            var direction = (curvedPosition - previousPosition).normalized;

            if (direction != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(direction);
                enemy.transform.rotation =
                    Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            previousPosition = curvedPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }

        enemy.transform.position = toPosition;
        enemy.transform.rotation = Quaternion.LookRotation((toPosition - controlPoint2).normalized);
        enemy.canMove = true;
    }
}