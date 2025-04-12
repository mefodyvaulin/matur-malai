using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] private float speed = 10f;
    [SerializeField] private int hp = 50;
    [SerializeField] public bool canMove = false;
    //private int i = 1; // для проверки хп игрока
    public bool CanMove => canMove;
    private void Awake() // Вызывается при создании объекта
    {
        // сгенерировать рандомно пулю или как то там
    }
    
    private void Update() // Главный игровой цикл — вызывается каждый кадр
    {
        //i++;
        if (canMove)
            MoveBack();
        /*if (i != 100) return;
        i = 0;
        Shoot();*/
    }
    
    protected virtual void Shoot()
    {
        Instantiate(bulletPrefab, transform.position + transform.forward * 4f, transform.rotation);
    }

    protected virtual void MoveBack()
    {
        transform.Translate(-Vector3.forward * (speed * Time.deltaTime));
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log($"Enemy took {damage} damage. HP now: {hp}");
        if (hp <= 0)
        {
            Debug.Log("Enemy destroyed!");
            Destroy(gameObject);
        }
        if (hp <= 0) Destroy(gameObject);
    }
}