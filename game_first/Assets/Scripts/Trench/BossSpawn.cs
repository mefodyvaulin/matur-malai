using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

public class BossSpawn : MonoBehaviour
{
    [SerializeField] private Boss BossPrefab;
    
    private float spawnedAfter = 1.5f;
    private bool spawned = false;
    
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
        if (!spawned && transform.position.z - GameModel.PlayerPosition.z <= spawnedAfter * segmentLenght 
            && transform.position.z - GameModel.PlayerPosition.z > spawnedAfter * segmentLenght - 0.96f)
        {
            spawned = true;
            SpawnBoss();
        }
    }

    private void SpawnBoss()
    {
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