using UnityEngine;
using System.Collections;

public abstract class Missile : MonoBehaviour
{
    protected abstract float LifeTime { get; } // Время жизни пули
    protected abstract int Damage { get; }
    protected abstract float Speed { get; } // Скорость полёта вперёд (единиц в секунду)
    public AudioSource AudioSource;
    
    protected virtual void Start()
    {
        AudioSource.Play();
        StartCoroutine(DestroyAfterUnscaledTime(LifeTime));
    }

    private IEnumerator DestroyAfterUnscaledTime(float time)
    {
        var elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += GameModel.UnscaledDeltaTime;
            yield return null;
        }
        Die();
    }
    
    protected virtual void OnTriggerEnter(Collider other) //OnCollisionEnter(Collision other) <- можно заменить на это, если будет добавлена физика
    {
        var damageable = other.GetComponent<IDamageable>(); // Проверяем, есть ли на объекте интерфейс IDamageable
        damageable?.TakeDamage(Damage);
        Die();
    }
    
    private void Update()
    {
        Move();
    }
    
    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected abstract void Move();
}
