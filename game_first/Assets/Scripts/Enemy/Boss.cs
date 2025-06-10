public class Boss : EnemyAbstract, IFillBarProvider
{
    public float CurrentValue => health.CurrentHp;
    public float MaxValue => health.MaxHp;

    protected override void Awake()
    {
        GameModel.GenerateTrench.bossBar.gameObject.SetActive(true);
        GameModel.GenerateTrench.bossBar.gameObject.GetComponentInChildren<FillBar>().SetProvider(this);
        KillEnemies();
        GameModel.BossIsAlive = true;
        base.Awake();
        EnemySpawn.CanSpawn = false;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        // ReSharper disable once PossibleLossOfFraction
        Statistic.sessionScore += 900 * (1 + GameModel.Harder / 2);
        GameModel.BossIsAlive = false;
        EnemySpawn.CanSpawn = true;
        GameModel.GenerateTrench.bossBar.gameObject.SetActive(false);
        GameModel.GenerateTrench.BossTrenchExists = false;
        GameModel.Harder++;
    }
    
    private static void KillEnemies()
    {
        foreach (var enemy in GameModel.Enemies.Keys)
        {
            enemy.health.isIndestructibleShield = false;
            enemy.health.TakeDamage(1000);
        }
    }
}
