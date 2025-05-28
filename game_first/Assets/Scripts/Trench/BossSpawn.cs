using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

public class BossSpawn : MonoBehaviour
{
    [SerializeField] private Boss BossPrefab;
    //[SerializeField] private Animator hatchAnimation;
    
    private int spawnedAfter;
    
    private void Awake()
    {
        Trench.OnGenerateContinuationOfTrench += CountFragmentsToSpawn;
    }
    
    private void OnDestroy()
    {
        Trench.OnGenerateContinuationOfTrench -= CountFragmentsToSpawn;
    }

    private void CountFragmentsToSpawn()
    {
        spawnedAfter++;
        if (spawnedAfter == 2)//&& GameModel.CountEnemies <= 0)
        {
            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
        //hatchAnimation.SetBool("spawnMoment", true);
        var bossGroup = new EnemyGroupBoss(1, transform.position);
        var boss = Instantiate(BossPrefab, transform.position, transform.rotation);
        var finalPosition = bossGroup.TakePosition(0);
        StartCoroutine(
            EnemySpawnStartAnimation.MoveToPosition(boss, finalPosition, bossGroup.MoveGroup, 0,
                controlPoint1: new Vector3(
                    finalPosition.x,
                    (finalPosition.y + boss.transform.position.y) / 2,
                    (finalPosition.z + boss.transform.position.z) / 2),
                controlPoint2: new Vector3(
                    finalPosition.x,
                    finalPosition.y,
                    finalPosition.z + 10),
                flightTime: 6f));
    }
}