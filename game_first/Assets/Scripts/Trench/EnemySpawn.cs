using System;
using UnityEngine;
using Random = System.Random;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Enemy[] enemiesPrefab;
    private int spawnedAfter;

    private void Awake()
    {
        Trench.OnGenerateContinuationOfTrench += CountFragmentsToSpawn;
    }
    
    private void OnDestroy()
    {
        Trench.OnGenerateContinuationOfTrench -= CountFragmentsToSpawn;
    }

    private void CountFragmentsToSpawn(Trench.TrenchState state)
    {
        spawnedAfter++;
        if (spawnedAfter == 4 && GameModel.CountEnemies <= 0) // спавн прямо при влете в эту часть туннеля
        {
            SpawnGroup();
        }
    }

    private void SpawnGroup()
    {
        var countDrones = new Random().Next(3, 7);
        var group = new EnemyGroupHorizontallyOrVertically(countDrones, transform.position);
        
        for (var i = 0; i < countDrones; i++)
        {
            var enemyIndex = new Random().Next(enemiesPrefab.Length);
            var enemy = Instantiate(enemiesPrefab[enemyIndex], transform.position, transform.rotation);
            
            var finalPosition = group.TakePosition(i);

            StartMoving(enemy, finalPosition, group.MoveGroup);
        }
    }

    private void StartMoving(Enemy enemy, Vector3 targetPosition, Action<Enemy> moveGroup)
    {
        StartCoroutine(EnemySpawnStartAnimation.MoveToPosition(enemy, targetPosition, moveGroup));
    }
}