using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    
    public int direction;
    
    [SerializeField] private float tiltAngle = 30f;
    [SerializeField] private float tiltSpeed = 1f;
    [SerializeField] private float omega1, omega2, omega3, phase, a, distanceToEnemy;
    
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float maxRotationAngle = 15f;
    
    private float? previousZ = null;

    public Action<Enemy> Move;

    private void Awake()
    {
        direction = Random.Range(0, 2) == 1 ? 1 : -1;
        
        tiltAngle = 30;
        omega1 = Random.Range(1.75f, 3f);
        omega2 = Random.Range(1.75f, 3f);
        omega3 = Random.Range(1.75f, 3f);
        phase = Random.Range(0f, 2 * Mathf.PI);
        a = Random.Range(4.5f, 5.5f);
        tiltSpeed = 1f;
        distanceToEnemy = Random.Range(25f, 35f);
    }

    public void MoveFollowerPlayer(Enemy enemy)
    {
        var player = GameModel.PlayerPosition;
        var tilt = Mathf.Cos(2f * omega1 * GameModel.UnscaledTime * tiltSpeed / 2.35f + phase) *
                   tiltAngle;
        var yTilt = -180 -tilt / 5;
        var sinOfAng = Mathf.Sin(omega1 * GameModel.UnscaledTime * tiltSpeed / 2.35f + phase);
        var cosOfAng = Mathf.Cos(omega1 * GameModel.UnscaledTime * tiltSpeed / 2.35f + phase);
        var x = player.x / 1.65f + a * cosOfAng / (1 + sinOfAng * sinOfAng) + 0.45f * Mathf.Sin(omega2 * GameModel.UnscaledTime * tiltSpeed);
        var y = 2.75f + player.y + a * cosOfAng * sinOfAng / (1 + sinOfAng * sinOfAng) - 1.5f * Mathf.Sin(omega3 * GameModel.UnscaledTime * tiltSpeed);
        transform.position = new Vector3(x, y, player.z + distanceToEnemy + 3 * sinOfAng);
        transform.rotation = Quaternion.Euler(0, yTilt, tilt);
        enemy.shooting.UpdateShooting();
    }

    public void DefaultMove()
    {
        Move += MoveBack;
        Move += Sway;
    }
    
    private void MoveBack(Enemy enemy)
    {
        var currentZ = GameModel.PlayerPosition.z;

        if (!previousZ.HasValue)
        {
            previousZ = currentZ;
            return;
        }

        var deltaZ = currentZ - previousZ.Value;
        previousZ = currentZ;

        transform.Translate(Vector3.back * deltaZ);
    }
    
    // Задание:
    // Реализовать плавные колебания врага по осям X и Y с использованием кватернионов.
    // Ось Z не должна изменяться.
    // Колебания должны быть плавными.
    // Параметр rotationSpeed контролирует скорость колебаний, а maxRotationAngle - максимальный угол отклонения по осям X и Y.
    // Углы колебаний должны быть ограничены от -maxRotationAngle до maxRotationAngle.
    private void Sway(Enemy enemy)
    {
    }

    public void ClearMove()
    {
        Move = null;
    }
}