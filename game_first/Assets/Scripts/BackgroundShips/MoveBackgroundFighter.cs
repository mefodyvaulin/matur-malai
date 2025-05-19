using System.Collections;
using UnityEngine;

public class MovebackgroundFighter: MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] ParticleSystem damageExplosion;
    public float moveSpeed = 40f; // Скорость движения
    private const float lifetime = 18f; // Время жизни объекта
    public float shootRate = 0.5f;
    private float nextSpawnShoot = 0f;
    private float nextSpawnDamage = 0f;
    public float damageRate = 0.5f;

    void Start()
    {
        // Запускаем корутину для движения и удаления объекта
        nextSpawnShoot =  Random.Range(0f, 1f / shootRate);
        StartCoroutine(MoveAndDestroyCoroutine());
    }

    private IEnumerator MoveAndDestroyCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < lifetime)
        {
            // Двигаем объект вперед
            if (Time.time >= nextSpawnShoot)
            {
                for(var i = 0; i < Random.Range(1, 4); i++)
                    Shoot();
                nextSpawnShoot = Time.time + 1f / shootRate;
            }
            if (Time.time >= nextSpawnDamage)
            {
                damageExplosion.Play();
                nextSpawnDamage = Time.time + 1f / damageRate;
            }
            transform.position += moveSpeed * Time.deltaTime * transform.forward;
            transform.Rotate(-Mathf.Abs(0.5f * Mathf.Sin(Time.time)), 
                0f, 
                0.8f * Mathf.Cos(Time.time));
            elapsedTime += Time.deltaTime;
            yield return null; // Ждем следующий кадр
        }

        // Удаляем объект со сцены
        Destroy(gameObject);
    }
    
    void Shoot()
    {
        // Создаем объект с новым поворотом
        Instantiate(projectilePrefab, 
            transform.position + Random.Range(-2, 4) * transform.forward, 
            transform.rotation);
    }
}
