using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    
    [SerializeField] private float tiltAngle = 15f;
    [SerializeField] private float tiltSpeed = 1f;
    [SerializeField] private float omega1, omega2, omega3, phase, a, distanceToEnemy;

    private void Awake()
    {
        tiltAngle = 30;
        omega1 = Random.Range(1.75f, 3f);
        omega2 = Random.Range(1.75f, 3f);
        omega3 = Random.Range(1.75f, 3f);
        phase = Random.Range(0f, 2 * Mathf.PI);
        a = Random.Range(4.5f, 5.5f);
        tiltSpeed = 1f;
        distanceToEnemy = Random.Range(25f, 35f);
    }

    public void Move()
    {
        var player = GameModel.PlayerPosition;
        var tilt = Mathf.Cos(2f * omega1 * Time.time * tiltSpeed / 2.35f + phase) *
                   tiltAngle;
        var yTilt = -180 -tilt / 5;
        var sinOfAng = Mathf.Sin(omega1 * Time.time * tiltSpeed / 2.35f + phase);
        var cosOfAng = Mathf.Cos(omega1 * Time.time * tiltSpeed / 2.35f + phase);
        var x = player.x / 1.65f + a * cosOfAng / (1 + sinOfAng * sinOfAng) + 0.45f * Mathf.Sin(omega2 * Time.time * tiltSpeed);
        var y = 2.75f + player.y + a * cosOfAng * sinOfAng / (1 + sinOfAng * sinOfAng) - 1.5f * Mathf.Sin(omega3 * Time.time * tiltSpeed);
        transform.position = new Vector3(x, y, player.z + distanceToEnemy + 3 * sinOfAng);
        transform.rotation = Quaternion.Euler(0, yTilt, tilt);
    }
    
    public virtual void MoveBack()
    {
        transform.Translate(Vector3.back * (speed * Time.deltaTime));
    }
}