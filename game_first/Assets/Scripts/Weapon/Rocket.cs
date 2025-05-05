using System.Linq;
using UnityEngine;

public class Rocket : Missile
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float rotationSpeed = 60f;
    protected override float LifeTime => 4f;
    protected override int Damage => 0;
    protected override float Speed => 30f;
    private Enemy target;

    private void Awake()
    {
        target = TakeTarget();
    }
    
    private void OnDestroy()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
    
    protected override void Move()
    {
        if (target is not null && target.health.IsAlive)
        {
            // Направление к цели
            var direction = (target.transform.position - transform.position).normalized;

            // Целевое вращение
            var targetRotation = Quaternion.LookRotation(direction);

            // Плавный поворот к цели
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
        transform.Translate(Vector3.forward * (Speed * Time.deltaTime));

        if (!(target is not null && target.health.IsAlive))
        {
            target = TakeTarget();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        var damageable = other.GetComponent<IDamageable>();
        damageable?.TakeDamage(Damage);
        // взрыв перед удалением
        Destroy(gameObject); 
    }

    private Enemy TakeTarget()
    {
        return GameModel.Enemies.Keys
            .OrderBy(enemy => Vector3.Distance(enemy.transform.position, transform.position))
            .FirstOrDefault();
    }
}