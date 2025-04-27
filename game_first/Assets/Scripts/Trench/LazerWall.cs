using UnityEngine;

public class LazerWall : MonoBehaviour
{
    private int Damage = 10;

    protected virtual void OnTriggerEnter(Collider other) //OnCollisionEnter(Collision other) <- можно заменить на это, если будет добавлена физика
    {
        var damageable = other.GetComponent<IDamageable>(); // Проверяем, есть ли на объекте интерфейс IDamageable
        damageable?.TakeDamage(Damage);
    }
}