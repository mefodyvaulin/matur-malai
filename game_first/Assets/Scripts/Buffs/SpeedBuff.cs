using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Buffs
{
    public class SpeedBuff : AbstractBuff
    {
        public int boost = 50;
        private const float timeBoost = 1f;
        private float lastPlayerSpeed;
        private float lastPlayerRotationSpeed;
        private float lastCameraSmoothTime;
        private System.Type lastWeapon;

        private void Awake()
        {
            if (GameModel.GenerateTrench.IsBossLocation 
                && transform.position.z >= GameModel.GenerateTrench.BossLocationSegmentPosition) 
                Destroy(gameObject);
        }

        protected override IEnumerator DoBuff()
        {
            if (GameModel.PlayerHitPoint.isIndestructibleSpeedBuff) yield break;
            GameModel.SetSpeedBuff(this);
            
            GameModel.PlayerHitPoint.isIndestructibleSpeedBuff = true;
            lastPlayerRotationSpeed = GameModel.PlayerMovement.rotationSpeed;
            lastPlayerSpeed = GameModel.PlayerMovement.speed;
            lastCameraSmoothTime = GameModel.CameraFollow.smoothTime;
            EnemySpawn.CanSpawn = false;
            
            KillEnemies();
            GameModel.PlayerMovement.rotationSpeed = 0;
            lastWeapon = GameModel.WeaponSwitcher.DisableAllAndGetActiveWeaponType();
            
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
                if (Time.timeScale == 0)
                {
                    yield return null;
                    continue;
                }
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
                if (Time.timeScale == 0)
                {
                    yield return null;
                    continue;
                }
                if (GameModel.GenerateTrench.speedStop)
                {
                    break;
                }
                elapsedTime += GameModel.UnscaledDeltaTime;
                yield return null;
            }
            AllReturn();
        }

        private void AllReturn()
        {
            GameModel.SetSpeedBuff(null);
            
            GameModel.PlayerMovement.rotationSpeed = lastPlayerRotationSpeed;
            GameModel.PlayerMovement.speed = lastPlayerSpeed;
            GameModel.CameraFollow.smoothTime = lastCameraSmoothTime;
            GameModel.WeaponSwitcher.SetWeapon(lastWeapon, fullRecharge: false);
            
            EnemySpawn.CanSpawn = true;
            GameModel.PlayerHitPoint.isIndestructibleSpeedBuff = false;

            Destroy(gameObject);
        }

        public void Stop()
        {
            StopAllCoroutines();
            AllReturn();
        }
    }
}