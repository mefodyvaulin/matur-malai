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
            var dynamicBoost = Mathf.Clamp(Time.timeScale, 0f, 2f);
            transform.position +=  (moveSpeed * dynamicBoost * GameModel.UnscaledDeltaTime) * transform.forward;
            elapsedTime += GameModel.UnscaledDeltaTime;
            yield return null; 
        }
        
        Destroy(gameObject);
    }
}

