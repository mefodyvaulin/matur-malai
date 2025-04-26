using UnityEngine;

public class PlayerHitPoint : MonoBehaviour, IDamageable, IFillBarProvider
{
    [SerializeField] private int maxHp = 50;
    [SerializeField] private int currentHp = 50;
    
    public float MaxValue => maxHp;
    public float CurrentValue => currentHp;
    
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0) Destroy(gameObject);
    }
}
