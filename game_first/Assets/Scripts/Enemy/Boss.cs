public class Boss : EnemyAbstarct, IFillBarProvider
{
    public float CurrentValue => health.CurrentHp;
    public float MaxValue => health.MaxHp;
}
