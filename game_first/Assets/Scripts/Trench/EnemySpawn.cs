using UnityEngine;
using Random = System.Random;
using System.Linq;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private Enemy[] enemies;
    private int spawnedAfter;

    private void Awake()
    {
        Trench.OnGenerateContinuationOfTrench -= CountFragmentsToSpawn;
    }
    
    private void OnEnable() //возможно стоит сделать так (Для МЕФОДИЯ)
    {
        //Trench.OnGenerateContinuationOfTrench += CountFragmentsToSpawn;
    }
    
    private void OnDisable()
    {
        //Trench.OnGenerateContinuationOfTrench -= CountFragmentsToSpawn;
    }

    private void CountFragmentsToSpawn(Trench.TrenchState state)
    {
        spawnedAfter++;
        if (spawnedAfter == 4 && GameModel.CountEnemies <= 0) // спавн прямо при влете в эту часть туннеля
        {
            SpawnGroup();
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

    private void SpawnGroup()
    {
        var droneCount = new Random().Next(3, 7);
        var maxRatio = droneCount - 1;

        var randomRatios = GenerateRandomUniqueRatios(droneCount);

        for (var i = 0; i < droneCount; i++)
        {
            var enemyIndex = new Random().Next(enemies.Length);
            var enemy = Instantiate(enemies[enemyIndex], transform.position, transform.rotation);

            var startOffset = enemy.transform.position + new Vector3(5, 10, 0);
            var endOffset = enemy.transform.position + new Vector3(25, -10, -30);

            var finalPosition = InterpolateWithRatio(startOffset, endOffset, randomRatios[i], maxRatio);

            StartMoving(enemy, finalPosition);
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
}