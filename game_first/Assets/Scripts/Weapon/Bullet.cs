using UnityEngine;

public class Bullet : Missile
{

    protected override float LifeTime => 3f;
    protected override int Damage => 10;
    protected override float Speed => 50f;

    protected override void Move() // Постоянное движение вперёд(по Z)
    {
        transform.Translate(Vector3.forward * (Speed * Time.deltaTime));
    }
}
