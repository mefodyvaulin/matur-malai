using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour,  IDamageable
{
    [SerializeField] private int hp = 50;
    [SerializeField] public AudioSource[] audioSources;
    [SerializeField] private GameObject floatingText;
    [SerializeField] ParticleSystem damageExplosion;
    [SerializeField] GameObject defeatingObject;
    
    public bool IsAlive => hp > 0;

    // ReSharper disable Unity.PerformanceAnalysis
    public void TakeDamage(int damage)
    {
        damageExplosion.gameObject.SetActive(true);
        hp -= damage;
        if (!IsAlive)
        {
            Instantiate(defeatingObject, transform.position, Quaternion.identity);
            
            GetComponent<Collider>().enabled = false;
            GetComponentInChildren<MeshRenderer>().enabled = false;
            audioSources[1].Play();
            Destroy(gameObject, audioSources[1].clip.length);
        }
        else
        {
            damageExplosion.Play();
            
            audioSources[0].Play();
            var textDamage = Instantiate(floatingText, transform.position, Quaternion.identity);
            textDamage.GetComponentInChildren<TextMeshPro>().text = damage.ToString();
            Destroy(textDamage, 1f);
        }
    }
}