using System;
using EnemyGroup;
using UnityEngine;
using Random = System.Random;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Enemy[] enemiesPrefab;
    private int spawnedAfter;
    private static readonly Random rand = new();


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
        if (spawnedAfter == 2 && GameModel.CountEnemies <= 0) // спавн прямо при влете в эту часть туннеля
        {
            SpawnGroup();
        }
    }

    private void SpawnGroup()
    {
        var countDrones = rand.Next(3, 7);
        var group = CreateRandomGroup(countDrones, transform.position);
        
        for (var i = 0; i < countDrones; i++)
        {
            var enemyIndex = rand.Next(enemiesPrefab.Length);
            var enemy = Instantiate(enemiesPrefab[enemyIndex], transform.position, transform.rotation);
            
            var finalPosition = group.TakePosition(i);

            StartMoving(enemy, finalPosition, group.MoveGroup);
        }
    }
    
    private static EnemyGroupAbstract CreateRandomGroup(int countDrones, Vector3 spawnPosition)
    {
        var type = rand.Next(0, 2); // пока только один тип

        return type switch
        {
            0 => new EnemyGroupHorizontallyOrVertically(countDrones, spawnPosition),
            1 => new EnemyGroupCircle(countDrones, spawnPosition),
            _ => throw new Exception("Unknown group type")
        };
    }

    private void StartMoving(Enemy enemy, Vector3 targetPosition, Action<Enemy> moveGroup)
    {
        StartCoroutine(EnemySpawnStartAnimation.MoveToPosition(enemy, targetPosition, moveGroup));
    }
}