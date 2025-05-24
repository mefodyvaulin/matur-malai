namespace Buffs
{
    public class HealthBuff : AbstractBuff
    {
        private const int heal = 10;
        protected override void DoBuff()
        {
            if (GameModel.PlayerHitPoint.MaxValue >= GameModel.PlayerHitPoint.CurrentValue + heal)
                GameModel.PlayerHitPoint.CurrentHp += heal;
            else
                GameModel.PlayerHitPoint.CurrentHp = (int)GameModel.PlayerHitPoint.MaxValue;
        }
    }
}