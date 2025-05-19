using System.Collections;
using UnityEngine;

public class MoveProjectile: MonoBehaviour
{
    public float moveSpeed = 5f;
    private const float lifetime = 15f;

    private void Start()
    {
        StartCoroutine(MoveAndDestroyCoroutine());
    }

    private IEnumerator MoveAndDestroyCoroutine()
    {
        var elapsedTime = 0f;

        while (elapsedTime < lifetime)
        {
            transform.position +=  moveSpeed * Time.deltaTime * transform.forward;
            elapsedTime += Time.deltaTime;
            yield return null; 
        }
        
        Destroy(gameObject);
    }
}

