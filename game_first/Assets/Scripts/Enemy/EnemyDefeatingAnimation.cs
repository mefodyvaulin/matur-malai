using System.Collections;
using UnityEngine;

public class EnemyDefeatingAnimation : MonoBehaviour
{
    [SerializeField] ParticleSystem defeatedExplosion;
    void Start()
    {
        StartCoroutine(PlayAndDestroy());
    }
    
    IEnumerator PlayAndDestroy()
    {
        defeatedExplosion.Play();
        yield return new WaitUntil(() => !defeatedExplosion.isPlaying);
        
        Destroy(gameObject);
    }
}
