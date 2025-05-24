using UnityEngine;
using System.Linq;

public class EnemyGroupHorizontallyOrVertically : EnemyGroupAbstract
{
    private readonly Vector3 startOffset;
    private readonly Vector3 endOffset;
    private readonly Vector3[] randomRatios;
    private readonly int maxRatio;

    private static readonly float verticalSpeed = 4f;

    private Vector3 moveDirection;
    public EnemyGroupHorizontallyOrVertically(int countDrones, Vector3 spawnPosition)
        : base(countDrones, spawnPosition)
    {
        startOffset = new Vector3(maxX - 1, maxY - 1, spawnPosition.z);
        endOffset = new Vector3(minX + 1, minY + 1, spawnPosition.z - 30);

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

        if (moveDirection == Vector3.up)
        {
            var y = enemy.transform.position.y;
            if (y >= maxY)
            {
                enemy.movement.direction = -1;
            }
            else if (y <= minY)
            {
                enemy.movement.direction = 1;
            }
        }
        else
        {
            var x = enemy.transform.position.x;
            if (x >= maxX)
            {
                enemy.movement.direction = -1;
            }
            else if (x <= minX)
            {
                enemy.movement.direction = 1;
            }
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
