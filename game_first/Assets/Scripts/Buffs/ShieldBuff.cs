using System.Collections;
using UnityEngine;

namespace Buffs
{
    public class ShieldBuff : AbstractBuff
    {
        // ReSharper disable Unity.PerformanceAnalysis
        protected override IEnumerator DoBuff()
        {
            if (GameModel.Shield != null)
                GameModel.Shield.ReanimateShield();
            GameModel.Shield.gameObject.SetActive(true);
            yield return null;
        }
    }
}