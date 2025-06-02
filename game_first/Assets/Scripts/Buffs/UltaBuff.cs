using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Buffs
{
    public class UltaBuff : AbstractBuff
    {
        private const int partOfUlta = 2;
        [SerializeField] private Renderer rend;

        protected override IEnumerator DoBuff()
        {
            GameModel.WeaponSwitcher.PourInUlta(GameModel.WeaponSwitcher.ShouldEnemiesDieCount/2);
            yield break;
        }

        protected void Awake()
        {
        }

        protected override void Update()
        {
            base.Update();
            if (rend is null) return;
            rend.material.color = GameModel.WeaponSwitcher.CurrentWeapon.UltaColor;
        }
    }
}