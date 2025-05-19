using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform[] firePoints;
    [SerializeField] ParticleSystem[] damageExplosions;
    public float fireRate = 0.1f;
    public float damageRate = 0.1f;
    public Vector3 initialPosition = new (100, 75, 0);
    public float speed = 2f;
    private float nextFireTime = 0f;
    private float nextDamageTime = 0f;
    public float timeToPosition = 25f;
    private float delta = 0;
    public float xRandomAngle = 30f;
    public float yRandomLeftBorderAngle = -60f;
    public float yRandomRightBorderAngle = -35f;

    private void Start()
    {
        nextFireTime =  Random.Range(0f, 1f / fireRate);
        nextDamageTime = Random.Range(0f, 1f / damageRate);
    }

    private void FixedUpdate()
    {
        delta = Time.time <= timeToPosition ? speed * Time.time : delta;
        transform.position = initialPosition + 
                             new Vector3(0, 0, GameModel.PlayerPosition.z) + 
                             transform.forward * delta;
        if (Time.time >= nextFireTime)
        {
            for(var i = 0; i < Random.Range(1, 4); i++)
                Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }

        if (!(Time.time >= nextDamageTime)) return;
        var randomDamagePosition = Random.Range(0, damageExplosions.Length);
        damageExplosions[randomDamagePosition].Play();
        nextDamageTime = Time.time + 1f / damageRate;
    }

    private void Shoot()
    {
        var randomYRotation = Random.Range(yRandomLeftBorderAngle, yRandomRightBorderAngle);
        var randomXRotation = Random.Range(-xRandomAngle, xRandomAngle);
        var randomSpawnPosition = Random.Range(0, firePoints.Length);

        var randomRotation = firePoints[randomSpawnPosition].rotation * Quaternion.Euler(randomXRotation, randomYRotation, 0);

        Instantiate(projectilePrefab, firePoints[randomSpawnPosition].position, randomRotation);
    }
}

