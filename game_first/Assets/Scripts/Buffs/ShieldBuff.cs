using System.Collections;
using UnityEngine;

namespace Buffs
{
    public class ShieldBuff : AbstractBuff
    {
        [SerializeField] private Shield shield;

        // ReSharper disable Unity.PerformanceAnalysis
        protected override IEnumerator DoBuff()
        {
            var currentShield = Instantiate(shield, GameModel.PlayerCollider.bounds.center, GameModel.PlayerMovement.transform.rotation);
            currentShield.Init(GameModel.PlayerCollider);
            yield return null;
        }
    }
}