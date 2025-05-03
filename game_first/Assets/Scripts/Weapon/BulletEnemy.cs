using UnityEngine;

public class BulletEnemy: Missile
{
    protected override float LifeTime => 3f;
    protected override int Damage => 5;
    protected override float Speed => 50f;
    protected override void Move()
    {
        transform.Translate(Vector3.forward * (Speed * Time.deltaTime));
    }
}
