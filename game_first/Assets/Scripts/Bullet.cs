using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;        // Время жизни пули
    [SerializeField] private float speed = 50f;          // Скорость полёта вперёд (единиц в секунду)
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
    
    private void Update() // Главный игровой цикл — вызывается каждый кадр
    {
        MoveForward();
    }

    private void OnTriggerEnter(Collider other) //OnCollisionEnter(Collision other) <- можно заменить на это, если будет добавлена физика
    {
        var damageable = other.GetComponent<IDamageable>(); // Проверяем, есть ли на объекте интерфейс IDamageable
        damageable?.TakeDamage(10); // Например, наносим 10 единиц урона
        Destroy(gameObject); // Уничтожаем пулю
    }
    
    private void MoveForward() // Постоянное движение вперёд(по Z)
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }
}
