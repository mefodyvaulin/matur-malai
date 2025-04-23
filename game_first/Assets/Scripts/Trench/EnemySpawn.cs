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
    }
    
    private void OnEnable()
    {
        Trench.OnGenerateContinuationOfTrench += CountFragmentsToSpawn;
    }
    
    private void OnDisable()
    {
        Trench.OnGenerateContinuationOfTrench -= CountFragmentsToSpawn;
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
    
    private void StartMoving(Enemy enemy, Vector3 toPosition)
    {
        StartCoroutine(EnemySpawnStartAnimation.MoveToPosition(enemy, toPosition));
    }
}