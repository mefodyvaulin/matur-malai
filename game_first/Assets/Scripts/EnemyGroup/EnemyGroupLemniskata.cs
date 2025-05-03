using System.Collections.Generic;
using UnityEngine;

public class EnemyGroupLemniskata : EnemyGroupAbstract
{
    private readonly float radius;
    private readonly float deltaAngle;
    private float angle;
    private readonly Vector3 LemniskataCenter;
    private List<float> enemyAngles;
    
    public EnemyGroupLemniskata(int countDrones, Vector3 spawnPosition) : base(countDrones, spawnPosition)
    {
        radius = 9;
        deltaAngle = 0.01f;
        angle = 0;
        LemniskataCenter = spawnPosition + new Vector3(15.5f, -2, 0);
        enemyAngles = new List<float>();
    }
    
    public override Vector3 TakePosition(int index)
    {
        enemyAngles.Add(Mathf.PI / 5 * index);
        var xAndY = GetXAndY(Mathf.PI / 5 * index);
        return LemniskataCenter + new Vector3(
            xAndY.Item1,
            xAndY.Item2,
            0);
    }

    public override void MoveGroup(Enemy enemy)
    {
        var enemyIndex = GameModel.Enemies[enemy] - 1;
        enemyAngles[enemyIndex] += deltaAngle;
        var xAndY = GetXAndY(enemyAngles[enemyIndex]);
        enemy.transform.position += new Vector3(
            xAndY.Item1 + LemniskataCenter.x - enemy.transform.position.x, 
            xAndY.Item2 + LemniskataCenter.y - enemy.transform.position.y, 
            0);
        enemy.shooting.UpdateShooting();
    }

    private (float, float) GetXAndY(float ang)
    {
        return (radius * Mathf.Cos(ang)
                / (1 + Mathf.Sin(ang) * Mathf.Sin(ang)),
            Mathf.Sqrt(5) * radius * Mathf.Cos(ang) * Mathf.Sin(ang)
            / (1 + Mathf.Sin(ang) * Mathf.Sin(ang)));
    }
}
