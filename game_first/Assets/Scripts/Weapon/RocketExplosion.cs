using UnityEngine;

public class RocketExplosion : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float radius = 5f;

    private void Start()
    {
        var colliders = Physics.OverlapSphere(transform.position, radius);
        
        var processed = new System.Collections.Generic.HashSet<GameObject>();

        foreach (var collider in colliders)
        {
            var obj = collider.gameObject;

            if (processed.Contains(obj))
                continue;

            var damageable = obj.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                processed.Add(obj);
            }
        }
        
        Destroy(gameObject, 0.1f);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
