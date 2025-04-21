using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;        // Время жизни пули
    [SerializeField] private float speed = 50f;          // Скорость полёта вперёд (единиц в секунду)
    [SerializeField] private int damage = 10;
    public AudioSource AudioSource;
    private void Start()
    {
        AudioSource.Play();
        Destroy(gameObject, lifetime);
    }
    
    private void Update()
    {
        MoveForward();
    }

    private void OnTriggerEnter(Collider other) //OnCollisionEnter(Collision other) <- можно заменить на это, если будет добавлена физика
    {
        var damageable = other.GetComponent<IDamageable>(); // Проверяем, есть ли на объекте интерфейс IDamageable
        damageable?.TakeDamage(damage);
    }

    private void OnTriggerExit(Collider other)
    {
        var damageable = other.GetComponent<IDamageable>();
        damageable?.BulletExit();
        Destroy(gameObject); // Уничтожаем пулю
    }
    
    private void MoveForward() // Постоянное движение вперёд(по Z)
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }
}
