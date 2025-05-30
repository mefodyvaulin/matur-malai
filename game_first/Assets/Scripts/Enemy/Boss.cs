public class Boss : EnemyAbstract, IFillBarProvider
{
    public float CurrentValue => health.CurrentHp;
    public float MaxValue => health.MaxHp;
}
