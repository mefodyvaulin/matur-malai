using UnityEngine;

public abstract class Missile: MonoBehaviour
{
    protected abstract float LifeTime { get; } // Время жизни пули
    protected abstract int Damage { get; }
    protected abstract float Speed { get; } // Скорость полёта вперёд (единиц в секунду)
    public AudioSource AudioSource;
    private void Start()
    {
        AudioSource.Play();
        Destroy(gameObject, LifeTime);
    }

    protected virtual void OnTriggerEnter(Collider other) //OnCollisionEnter(Collision other) <- можно заменить на это, если будет добавлена физика
    {
        var damageable = other.GetComponent<IDamageable>(); // Проверяем, есть ли на объекте интерфейс IDamageable
        damageable?.TakeDamage(Damage);
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        Destroy(gameObject); // Уничтожаем пулю
    }

    private void Update()
    {
        Move();
    }

    protected abstract void Move();
}
