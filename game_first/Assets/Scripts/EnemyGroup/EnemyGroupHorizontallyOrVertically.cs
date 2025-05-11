using UnityEngine;
using System.Linq;

public class EnemyGroupHorizontallyOrVertically : EnemyGroupAbstract
{
    private readonly Vector3 startOffset;
    private readonly Vector3 endOffset;
    private readonly Vector3[] randomRatios;
    private readonly int maxRatio;

    private static readonly float verticalSpeed = 4f;
    private static readonly float minY = Trench.initialSegmentPosition.y + 4f;
    private static readonly float maxY = Trench.initialSegmentPosition.y + 28f;
    private static readonly float minX = Trench.initialSegmentPosition.x - 11f;
    private static readonly float maxX = Trench.initialSegmentPosition.x + 10f;

    private Vector3 moveDirection;
    public EnemyGroupHorizontallyOrVertically(int countDrones, Vector3 spawnPosition) 
        : base(countDrones, spawnPosition)
    {
        startOffset = spawnPosition + new Vector3(5, 10, 0);
        endOffset = spawnPosition + new Vector3(25, -12, -30);

        moveDirection = Random.Range(0, 2) == 0 ? Vector3.up : Vector3.right;
        
        randomRatios = GenerateRandomUniqueRatios(countDrones);
        maxRatio = countDrones - 1;
    }

    public override Vector3 TakePosition(int index)
    {
        return InterpolateWithRatio(startOffset, endOffset, randomRatios[index], maxRatio);
    }

    public override void MoveGroup(Enemy enemy)
    {
        enemy.transform.position += moveDirection * (enemy.movement.direction * verticalSpeed * GameModel.UnscaledDeltaTime);

        var y = enemy.transform.position.y;
        var x = enemy.transform.position.x;
        if (y >= maxY || x >= maxX)
        {
            //enemy.transform.position = new Vector3(enemy.transform.position.x, maxY, enemy.transform.position.z);
            enemy.movement.direction = -1;
        }
        else if (y <= minY || x <= minX)
        {
            //enemy.transform.position = new Vector3(enemy.transform.position.x, minY, enemy.transform.position.z);
            enemy.movement.direction = 1;
        }

        enemy.shooting.UpdateShooting();
    }

    private static Vector3[] GenerateRandomUniqueRatios(int count)
    {
        var rand = new System.Random();
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