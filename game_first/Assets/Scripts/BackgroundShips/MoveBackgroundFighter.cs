using System.Collections;
using UnityEngine;

public class MovebackgroundFighter: MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] ParticleSystem damageExplosion;
    public float moveSpeed = 40f; 
    private const float lifetime = 13f; 
    public float shootRate = 0.5f;
    private float nextSpawnShoot = 0f;
    private float nextSpawnDamage = 0f;
    public float damageRate = 0.5f;

    private void Start()
    {
        nextSpawnShoot =  Random.Range(0f, 1f / shootRate);
        StartCoroutine(MoveAndDestroyCoroutine());
    }

    private IEnumerator MoveAndDestroyCoroutine()
    {
        var elapsedTime = 0f;

        while (elapsedTime < lifetime)
        {
            if (GameModel.UnscaledTime >= nextSpawnShoot)
            {
                for(var i = 0; i < Random.Range(1, 4); i++)
                    Shoot();
                nextSpawnShoot = GameModel.UnscaledTime + 1f / shootRate;
            }
            if (GameModel.UnscaledTime >= nextSpawnDamage)
            {
                damageExplosion.Play();
                nextSpawnDamage = GameModel.UnscaledTime + 1f / damageRate;
            }
            var dynamicBoost = Mathf.Clamp(Time.timeScale, 0f, 2f);
            transform.position += (moveSpeed * dynamicBoost * GameModel.UnscaledDeltaTime) * transform.forward;
            transform.Rotate(-Mathf.Abs(0.4f * Mathf.Sin(GameModel.UnscaledTime)),
                0f, 
                0.6f * Mathf.Cos(GameModel.UnscaledTime));
            elapsedTime += GameModel.UnscaledDeltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void Shoot()
    {
        Instantiate(projectilePrefab, 
            transform.position + Random.Range(-2, 4) * transform.forward, 
            transform.rotation);
    }
}
