using UnityEngine;
using Random = System.Random;
using System.Linq;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Enemy[] enemies;
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
        var droneCount = new Random().Next(3, 7);
        var maxRatio = droneCount - 1; // вот это 

        var randomRatios = GenerateRandomUniqueRatios(droneCount); // вот это

        for (var i = 0; i < droneCount; i++)
        {
            var enemyIndex = new Random().Next(enemies.Length);
            var enemy = Instantiate(enemies[enemyIndex], transform.position, transform.rotation);

            var startOffset = enemy.transform.position + new Vector3(5, 10, 0);
            var endOffset = enemy.transform.position + new Vector3(25, -10, -30);

            var finalPosition = InterpolateWithRatio(startOffset, endOffset, randomRatios[i], maxRatio); // вот это должно быть в группе

            StartMoving(enemy, finalPosition);
            enemy.movement.Move += MoveUpDown;
        }
    }

    private void StartMoving(Enemy enemy, Vector3 targetPosition)
    {
        StartCoroutine(EnemySpawnStartAnimation.MoveToPosition(enemy, targetPosition));
    }

    private static Vector3[] GenerateRandomUniqueRatios(int count)
    {
        var rand = new Random();
        var indices = Enumerable.Range(0, count).ToArray();

        var xRatios = indices.OrderBy(_ => rand.Next()).ToArray();
        var yRatios = indices.OrderBy(_ => rand.Next()).ToArray();
        var zRatios = indices.OrderBy(_ => rand.Next()).ToArray();

        var result = new Vector3[count];
        for (var i = 0; i < count; i++)
        {
            result[i] = new Vector3(xRatios[i], yRatios[i], zRatios[i]);
        }

        return result;
    }

    private static Vector3 InterpolateWithRatio(Vector3 from, Vector3 to, Vector3 ratioVector, float totalRatio)
    {
        return (Vector3.Scale(to, ratioVector)
                + Vector3.Scale(from, new Vector3(totalRatio, totalRatio, totalRatio) - ratioVector))
               / totalRatio;
    }
    
    private static void MoveUpDown(Enemy enemy)
    {
        var minY = Trench.initialSegmentPosition.y + 5;
        var maxY = Trench.initialSegmentPosition.y + 28;

        var speed = 4f;

        enemy.transform.position += Vector3.up * (enemy.movement.direction * speed * Time.deltaTime);


        var y = enemy.transform.position.y;
        if (y >= maxY)
        {
            enemy.transform.position = new Vector3(enemy.transform.position.x, maxY, enemy.transform.position.z);
            enemy.movement.direction = -1;
        }
        else if (y <= minY)
        {
            enemy.transform.position = new Vector3(enemy.transform.position.x, minY, enemy.transform.position.z);
            enemy.movement.direction = 1;
        }
    }
}