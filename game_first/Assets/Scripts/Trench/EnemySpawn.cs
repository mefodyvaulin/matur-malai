using System;
using UnityEngine;
using Random = System.Random;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    private int spawnedAfter;
    private void Awake()
    {
        Trench.OnGenerateContinuationOfTrench += CountFragmentsToSpawn;
    }

    private void CountFragmentsToSpawn(Trench.TrenchState state)
    {
        spawnedAfter++;
        if (spawnedAfter == 5) // спавн прямо при влете в эту часть туннеля
        {
            SpawnEnemy();
            Trench.OnGenerateContinuationOfTrench -= CountFragmentsToSpawn;
        }
    }

    private void SpawnEnemy()
    {
        // появляется примерно на середине поля, а не облетает туннель
        // нужно реализовать логику подлета
        var indexDron = new Random().Next(2);
        Instantiate(enemies[indexDron], transform.position + new Vector3(20, 0, 0), transform.rotation);
    }
}