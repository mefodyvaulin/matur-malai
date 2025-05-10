using UnityEngine;

public class BulletEnemy: Missile
{
    protected override float LifeTime => 3f;
    protected override int Damage => 5;
    protected override float Speed => 50f;
    protected override void Move()
    {
        transform.Translate(Vector3.forward * (Speed * GameModel.UnscaledDeltaTime - GameModel.Player.speed * Time.deltaTime));
    }
}

/* ВАРИАНТ_2
   private float? previousZ = null;
   protected override void Move()
   {
       var currentZ = GameModel.PlayerPosition.z;
       float deltaZ;
       if (!previousZ.HasValue)
       {
           previousZ = currentZ;
           deltaZ = GameModel.Player.speed * Time.deltaTime;
       }
       else
       {
           deltaZ = currentZ - previousZ.Value;
       }
       transform.Translate(Vector3.forward * (Speed * GameModel.UnscaledDeltaTime - deltaZ));
       previousZ = currentZ;
   }
*/
