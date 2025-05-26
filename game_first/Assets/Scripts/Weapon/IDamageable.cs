public interface IDamageable
{
    int MaxHp { get; }
    int CurrentHp { get; set; } 
    void TakeDamage(int damage);
}
