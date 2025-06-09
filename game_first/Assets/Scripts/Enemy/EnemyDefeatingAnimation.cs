using System.Collections;
using UnityEngine;

public class EnemyDefeatingAnimation : MonoBehaviour
{
    [SerializeField] ParticleSystem defeatedExplosion;
    [SerializeField] private bool isUnscale = false;

    private void Start()
    {
        StartCoroutine(isUnscale ? PlayAndDestroyUnscale() : PlayAndDestroyScale());
    }

    private IEnumerator PlayAndDestroyScale()
    {
        defeatedExplosion.Play();
        yield return new WaitUntil(() => !defeatedExplosion.isPlaying);
        
        Destroy(gameObject);
    }
    
    private IEnumerator PlayAndDestroyUnscale()
    {
        defeatedExplosion.Play();
        
        var duration = defeatedExplosion.main.duration;

        var elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
