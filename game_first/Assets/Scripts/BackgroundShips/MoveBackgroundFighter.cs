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
            transform.Rotate(-Mathf.Abs(0.4f * Mathf.Sin(Time.time)),
                0f, 
                0.6f * Mathf.Cos(Time.time));
            elapsedTime += Time.deltaTime;
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
