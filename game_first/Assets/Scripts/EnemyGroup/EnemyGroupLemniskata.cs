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
        deltaAngle = 1.4f / 2;
        angle = 0;
        
        LemniskataCenter = new Vector3((maxX + minX) / 2, (maxY + minY) / 2, spawnPosition.z - 5);
        
        enemyAngles = new List<float>();
    }
    
    public override Vector3 TakePosition(int index)
    {
        enemyAngles.Add(Mathf.PI / 6 * index);
        var xAndY = GetXAndY(Mathf.PI / 5 * index);
        return LemniskataCenter + new Vector3(
            xAndY.Item1,
            xAndY.Item2,
            0);
    }

    public override void MoveGroup(EnemyAbstract enemy)
    {
        var enemyIndex = GameModel.Enemies[enemy] - 1;
        enemyAngles[enemyIndex] += deltaAngle * GameModel.UnscaledDeltaTime;
        var xAndY = GetXAndY(enemyAngles[enemyIndex]);
        enemy.transform.position += new Vector3(
            xAndY.Item1 + LemniskataCenter.x - enemy.transform.position.x, 
            xAndY.Item2 + LemniskataCenter.y - enemy.transform.position.y, 
            0);
        enemy.UpdateAllShootings();
    }

    private (float, float) GetXAndY(float ang)
    {
        return (radius * Mathf.Cos(ang)
                / (1 + Mathf.Sin(ang) * Mathf.Sin(ang)),
            Mathf.Sqrt(5) * radius * Mathf.Cos(ang) * Mathf.Sin(ang)
            / (1 + Mathf.Sin(ang) * Mathf.Sin(ang)));
    }
}
