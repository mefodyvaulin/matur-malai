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
        transform.GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.white, new Color(0.9f,0.51f,0.51f), 1000f);
    }

    public void BulletExit()
    {
        transform.GetComponent<MeshRenderer>().material.color = Color.Lerp(new Color(0.9f,0.51f,0.51f), Color.white, 1000f);
    }
}
