using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyGroupBoss : EnemyGroupAbstract
{
    private bool isHalfHpStart = false;
    private float halfHpStartTime;
    private float breakBetweenStagesTime = 2f; // время паузы между фазами (в секундах)

    public EnemyGroupBoss(int countDrones, Vector3 spawnPosition) : base(countDrones, spawnPosition)
    {
        if (countDrones != 1)
            throw new Exception("Boss count must be 1");
    }

    public override Vector3 TakePosition(int index)
    {
        return new Vector3((maxX + minX) / 2, (maxY + minY) / 2, spawnPosition.z);
    }

    public override void MoveGroup(EnemyAbstarct enemy)
    {
        var halfCount = enemy.ShootingsCount / 2;

        if (enemy.health.CurrentHp > enemy.health.MaxHp / 2)
        {
            enemy.UpdateShootings(Enumerable.Range(0, halfCount));
        }
        else
        {
            if (!isHalfHpStart)
            {
                // Входим в паузу между фазами — отключаем анимации
                enemy.UpdateShootings(Enumerable.Range(0, halfCount), animation: false);
                isHalfHpStart = true;
                halfHpStartTime = GameModel.UnscaledTime;
                return;
            }

            // Ждём, пока пройдёт пауза
            if (GameModel.UnscaledTime - halfHpStartTime <= breakBetweenStagesTime)
                return;
            
            var secondHalfCount = enemy.ShootingsCount - halfCount;
            enemy.UpdateShootings(Enumerable.Range(halfCount, secondHalfCount));
        }
    }
}
