using System.Collections;
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

        StartCoroutine(DestroyAfterUnscaledTime(0.1f));
    }
    
    private IEnumerator DestroyAfterUnscaledTime(float time)
    {
        var elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += GameModel.UnscaledDeltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
