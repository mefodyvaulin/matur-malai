using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Enemy[] enemiesPrefab;
    [SerializeField] private Animator hatchAnimation;
    [SerializeField] private Shield shield;

    private int spawnedAfter = 1;
    private bool spawned = false;
    public static bool CanSpawn = true;
    private static readonly Random rand = new();
    
    private static WeightedRandomStack<Func<int, Vector3, EnemyGroupAbstract>> randomGroup = new
        (
            new Func<int, Vector3, EnemyGroupAbstract>[]
            {
                (countDrones, spawnPosition) => new EnemyGroupHorizontallyOrVertically(countDrones, spawnPosition),
                (countDrones, spawnPosition) => new EnemyGroupCircle(countDrones, spawnPosition),
                (countDrones, spawnPosition) => new EnemyGroupLemniskata(countDrones, spawnPosition),
            },
            new [] {3, 2, 2}
        );

    private void Awake()
    {
        Trench.OnGenerateContinuationOfTrench += CountFragmentsToSpawn;
    }
    
    private void OnDestroy()
    {
        Trench.OnGenerateContinuationOfTrench -= CountFragmentsToSpawn;
    }

    private void CountFragmentsToSpawn(float segmentLenght)
    {
        if (!CanSpawn) return;
        
        if (!spawned &&
            transform.position.z - GameModel.PlayerPosition.z <= spawnedAfter * segmentLenght &&
            transform.position.z - GameModel.PlayerPosition.z > spawnedAfter * segmentLenght - 0.96f &&
            GameModel.CountEnemies <= 0
            )
        {
            spawned = true;
            StartCoroutine(SpawnGroup());
        }
    }

    private IEnumerator SpawnGroup()
    {
        hatchAnimation.SetBool("spawnMoment", true);
        var countDrones = rand.Next(3, 7);
        var group = CreateRandomGroup(countDrones, transform.position);
        
        for (var i = 0; i < countDrones; i++)
        {
            if (Time.timeScale == 0)
            {
                yield return null;
                continue;
            }
            if (!CanSpawn) yield break;
            
            var enemyIndex = rand.Next(enemiesPrefab.Length);
            var enemy = Instantiate(enemiesPrefab[enemyIndex], transform.position, transform.rotation);
            SpawnWithShield(enemy);
            var finalPosition = group.TakePosition(i);

            StartMoving(enemy, finalPosition, group.MoveGroup, i);
            yield return new WaitForSeconds(0.2f);
        }
    }
    
    private static EnemyGroupAbstract CreateRandomGroup(int countDrones, Vector3 spawnPosition)
    {
        return randomGroup.Pop()(countDrones, spawnPosition);
    }

    private void StartMoving(Enemy enemy, Vector3 targetPosition, Action<EnemyAbstract> moveGroup, int i)
    {
        StartCoroutine(EnemySpawnStartAnimation.MoveToPosition(enemy, targetPosition, moveGroup, i));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void SpawnWithShield(Enemy enemy)
    {
        if (!(UnityEngine.Random.value <= 0.05f)) return;
        var currentShield = Instantiate(shield, enemy.health.EnemyCollider.bounds.center, enemy.transform.rotation);
        currentShield.Init(enemy.health.EnemyCollider);
    }
}