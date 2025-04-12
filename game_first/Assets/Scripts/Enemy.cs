using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] private float speed = 10f;
    [SerializeField] private int hp = 50;
    [SerializeField] public bool canMove = false;
    public bool CanMove => canMove;
    private void Awake() // Вызывается при создании объекта
    {
        canMove = true;
        // сгенерировать рандомно пулю или как то там
    }
    
    private void Update() // Главный игровой цикл — вызывается каждый кадр
    {
        if (canMove)
            MoveBack();
    }
    
    protected virtual void Shoot()
    {
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }

    protected virtual void MoveBack()
    {
        transform.Translate(-Vector3.forward * (speed * Time.deltaTime));
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0) Destroy(gameObject);
    }
}