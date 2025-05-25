using System.Collections;
using System.Linq;
using UnityEngine;

namespace Buffs
{
    public class SpeedBuff : AbstractBuff
    {
        private const int boost = 100;
        private const float timeBoost = 1.5f;
        private float lastPlayerSpeed;
        private float lastPlayerRotationSpeed;
        private float lastCameraSmoothTime;

        protected override IEnumerator DoBuff()
        {
            if (GameModel.PlayerHitPoint.isIndestructibleSpeedBuff) yield break;
            
            GameModel.PlayerHitPoint.isIndestructibleSpeedBuff = true;
            lastPlayerRotationSpeed = GameModel.PlayerMovement.rotationSpeed;
            lastPlayerSpeed = GameModel.PlayerMovement.speed;
            lastCameraSmoothTime = GameModel.CameraFollow.smoothTime;
            EnemySpawn.CanSpawn = false;
            
            KillEnemies();
            GameModel.PlayerMovement.rotationSpeed = 0;
            
            yield return CenteringCoroutine();
            
            GameModel.PlayerMovement.speed = boost * lastPlayerSpeed;
            GameModel.CameraFollow.smoothTime = lastCameraSmoothTime / boost;
            
            yield return DestroyAfterUnscaledTime(timeBoost);
        }

        private static void KillEnemies()
        {
            foreach (var enemy in GameModel.Enemies.Keys)
            {
                enemy.health.isIndestructibleShield = false;
                enemy.health.TakeDamage(1000);
            }
        }

        private static IEnumerator CenteringCoroutine()
        {
            var targetRotation = Quaternion.identity;
            
            while (Quaternion.Angle(GameModel.PlayerMovement.transform.rotation, targetRotation) > 0.01f)
            {
                GameModel.PlayerMovement.transform.rotation = Quaternion.Slerp(
                    GameModel.PlayerMovement.transform.rotation,
                    targetRotation,
                    5f * GameModel.UnscaledDeltaTime
                );
                yield return null;
            }
            GameModel.PlayerMovement.transform.rotation = targetRotation;
        }
        
        private IEnumerator DestroyAfterUnscaledTime(float time)
        {
            var elapsedTime = 0f;
            while (elapsedTime < time)
            {
                elapsedTime += GameModel.UnscaledDeltaTime;
                yield return null;
            }
            GameModel.PlayerMovement.rotationSpeed = lastPlayerRotationSpeed;
            GameModel.PlayerMovement.speed = lastPlayerSpeed;
            GameModel.CameraFollow.smoothTime = lastCameraSmoothTime;
            
            EnemySpawn.CanSpawn = true;
            GameModel.PlayerHitPoint.isIndestructibleSpeedBuff = false;
        }
    }
}