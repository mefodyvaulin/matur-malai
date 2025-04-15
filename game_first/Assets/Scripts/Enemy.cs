using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] private float speed = 10f;
    [SerializeField] private int hp = 50;
    [SerializeField] public bool canMove = false;

    public Vector3 player;
    public float tiltAngle = 15f;
    public float tiltSpeed = 1f;
    public float omega1;
    public float omega2;
    public float omega3;
    public float phase;
    public float a;
    public float distanceToEnemy;


    public AudioSource[] audioSources;
    private int i = 1; // для проверки хп игрока
    public bool CanMove => canMove;
    private void Awake() // Вызывается при создании объекта
    {
        tiltAngle = 30;
        omega1 = Random.Range(1.75f, 3f);
        omega2 = Random.Range(1.75f, 3f);
        omega3 = Random.Range(1.75f, 3f);
        phase = Random.Range(0f, 2 * Mathf.PI);
        a = Random.Range(4.5f, 5.5f);
        tiltSpeed = 1f;
        distanceToEnemy = Random.Range(25f, 35f);
    }
    
    private void Update() // Главный игровой цикл — вызывается каждый кадр
    {
        i++;
        if (canMove)
            Move();
        if (i != 100) return;
        i = 0;
        Shoot();
    }
    
    protected virtual void Shoot()
    {
        Instantiate(bulletPrefab, transform.position + transform.forward * 4f, transform.rotation);
    }

    protected virtual void Move()
    {
        player = XWing.posSt;
        var tilt = Mathf.Cos(2f * omega1 * Time.time * tiltSpeed / 2.35f + phase) *
                   tiltAngle;
        var yTilt = -180 -tilt / 5;
        var sinOfAng = Mathf.Sin(omega1 * Time.time * tiltSpeed / 2.35f + phase);
        var cosOfAng = Mathf.Cos(omega1 * Time.time * tiltSpeed / 2.35f + phase);
        var x = player.x / 1.65f + a * cosOfAng / (1 + sinOfAng * sinOfAng) + 0.45f * Mathf.Sin(omega2 * Time.time * tiltSpeed);
        var y = 2.75f + player.y + a * cosOfAng * sinOfAng / (1 + sinOfAng * sinOfAng) - 1.5f * Mathf.Sin(omega3 * Time.time * tiltSpeed);
        transform.position = new Vector3(x, y, player.z + distanceToEnemy + 3 * sinOfAng);
        transform.rotation = Quaternion.Euler(0, yTilt, tilt);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"Enemy took {damage} damage. HP now: {hp}");
        if (hp <= 0)
        {
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;

            Debug.Log("Enemy destroyed!");
            audioSources[1].Play();
            Destroy(gameObject, audioSources[1].clip.length);
        }
        else
        {
            audioSources[0].Play();
            transform.GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.white, new Color(0.9f,0.51f,0.51f), 1f);
        }

    }

    public void BulletExit()
    {
        transform.GetComponent<MeshRenderer>().material.color = Color.Lerp(new Color(0.9f,0.51f,0.51f), Color.white, 1f);
    }
}