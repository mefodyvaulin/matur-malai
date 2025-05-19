using System.Collections;
using UnityEngine;

public class MoveProjectile: MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость движения
    private const float lifetime = 15f; // Время жизни объекта

    void Start()
    {
        // Запускаем корутину для движения и удаления объекта
        StartCoroutine(MoveAndDestroyCoroutine());
    }

    private IEnumerator MoveAndDestroyCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < lifetime)
        {
            // Двигаем объект вперед
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null; // Ждем следующий кадр
        }

        // Удаляем объект со сцены
        Destroy(gameObject);
    }
}

