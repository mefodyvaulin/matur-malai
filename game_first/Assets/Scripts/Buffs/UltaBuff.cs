using System.Collections;

namespace Buffs
{
    public class UltaBuff : AbstractBuff
    {
        private const int partOfUlta = 2;
        
        protected override IEnumerator DoBuff()
        {
            GameModel.WeaponSwitcher.PourInUlta(GameModel.WeaponSwitcher.ShouldEnemiesDieCount/2);
            yield break;
        }
    }
}