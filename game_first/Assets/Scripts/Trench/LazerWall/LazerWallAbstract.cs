using System.Collections;
using UnityEngine;

public abstract class LazerWallAbstract : MonoBehaviour
{
    protected abstract int Damage { get; }

    void Start()
    {
        StartCoroutine(Move());
    }
    protected virtual void OnTriggerEnter(Collider other) //OnCollisionEnter(Collision other) <- можно заменить на это, если будет добавлена физика
    {
        var damageable = other.GetComponent<IDamageable>(); // Проверяем, есть ли на объекте интерфейс IDamageable
        damageable?.TakeDamage(Damage);
    }
    
    protected virtual void Update()
    {
        ShouldDie();
    }
    
    private void ShouldDie()
    {
        if (GameModel.PlayerPosition.z - transform.position.z > 100f)
            Destroy(gameObject);
    }

    protected abstract IEnumerator Move();
}