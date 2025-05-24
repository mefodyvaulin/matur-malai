using UnityEngine;

public class Bullet : Missile
{

    protected override float LifeTime => 3f;
    protected override int Damage => 10;
    protected override float Speed => 40f;

    protected override void Move() // Постоянное движение вперёд(по Z)
    {
        transform.Translate(Vector3.forward * (GameModel.PlayerMovement.speed * Time.deltaTime + Speed * GameModel.UnscaledDeltaTime));
    }
}
