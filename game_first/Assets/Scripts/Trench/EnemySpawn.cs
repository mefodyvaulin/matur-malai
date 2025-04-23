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
        StartMoving(enemy, enemy.transform.position + new Vector3(20, 10, -20));
    }


    private float duration = 2f;
    private Quaternion toRotation = Quaternion.Euler(0, 180, 0);


    private void StartMoving(Enemy enemy, Vector3 toPosition)
    {
        StartCoroutine(MoveToPosition(enemy, toPosition));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator MoveToPosition(Enemy enemy, Vector3 toPosition)
    {
        var startPosition = enemy.transform.position;

        // управляющая точка для дуги (можно немного приподнять или сместить)
        var controlPoint = new Vector3(
            (enemy.transform.position.x + toPosition.x) / 4 * 3,
            2 * enemy.transform.position.y - toPosition.y,
            enemy.transform.position.z
            );

        var elapsed = 0f;
        var startRotation = enemy.transform.rotation;
        
        while (elapsed < duration)
        {
            var t = elapsed / duration;

            // Bezier формула: B(t) = (1−t)² * P0 + 2(1−t)t * P1 + t² * P2
            var curvedPosition =
                Mathf.Pow(1 - t, 2) * startPosition +
                2 * (1 - t) * t * controlPoint +
                Mathf.Pow(t, 2) * toPosition;

            enemy.transform.position = curvedPosition;
            enemy.transform.rotation = Quaternion.Slerp(startRotation, toRotation, t);
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        enemy.transform.position = toPosition;
        enemy.transform.rotation = toRotation;
        enemy.сanMove = true; // пока похуй
    }
}