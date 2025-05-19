using System.Collections;
using UnityEngine;

public class EnemyDefeatingAnimation : MonoBehaviour
{
    [SerializeField] ParticleSystem defeatedExplosion;

    private void Start()
    {
        StartCoroutine(PlayAndDestroy());
    }

    private IEnumerator PlayAndDestroy()
    {
        defeatedExplosion.Play();
        yield return new WaitUntil(() => !defeatedExplosion.isPlaying);
        
        Destroy(gameObject);
    }
}
