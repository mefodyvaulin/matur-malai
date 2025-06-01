using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyGroupBoss : EnemyGroupAbstract
{
    private bool isHalfHpStart = false;
    private float halfHpStartTime;
    private float breakBetweenStagesTime = 1.5f; // время паузы между фазами (в секундах)
    private Vector3 originalLocalPosition;
    private bool isFirstMove  = true;
    private float noiseOffsetX;
    private float noiseOffsetY;
    private float perlinOffsetX;
    private float perlinOffsetY;

    public EnemyGroupBoss(int countDrones, Vector3 spawnPosition) : base(countDrones, spawnPosition)
    {
        if (countDrones != 1)
            throw new Exception("Boss count must be 1");
    }

    public override Vector3 TakePosition(int index)
    {
        return new Vector3((maxX + minX) / 2, (maxY + minY) / 2, spawnPosition.z);
    }

    public override void MoveGroup(EnemyAbstract enemy)
    {
        if (isFirstMove)
        {
            originalLocalPosition = enemy.transform.localPosition;
            noiseOffsetX = UnityEngine.Random.Range(0f, 100f);
            noiseOffsetY = UnityEngine.Random.Range(0f, 100f);
            perlinOffsetX = UnityEngine.Random.Range(0f, 100f);
            perlinOffsetY = UnityEngine.Random.Range(0f, 100f);
            isFirstMove = false;
        }
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


            float time = GameModel.UnscaledTime;

            // Основные параметры
            var amplitude = 2f;
            var frequency = 1.5f;

            // Основное синусоидальное качание
            var baseX = Mathf.Sin((time + noiseOffsetX) * frequency);
            var baseY = Mathf.Cos((time + noiseOffsetY) * frequency);

            // Лёгкий шум сверху (не сильный, просто чтобы ломать регулярность)
            var noiseX = (Mathf.PerlinNoise(time * 0.5f + perlinOffsetX, 0f) - 0.5f) * 0.5f;
            var noiseY = (Mathf.PerlinNoise(0f, time * 0.5f + perlinOffsetY) - 0.5f) * 0.5f;

            var shakeX = baseX + noiseX;
            var shakeY = baseY + noiseY;

            var currentZ = enemy.transform.localPosition.z;
            enemy.transform.localPosition = new Vector3(
                originalLocalPosition.x + shakeX * amplitude,
                originalLocalPosition.y + shakeY * amplitude,
                currentZ
            );
        }
    }
}
