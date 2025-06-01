using System;
using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour,  IDamageable, ICanShield
{
    [SerializeField] int maxHp = 50;
    [SerializeField] int Hp = 50;
    public int MaxHp => maxHp;
    public int CurrentHp { get; set; }
    [SerializeField] public AudioSource[] audioSources;
    [SerializeField] private GameObject floatingText;
    [SerializeField] ParticleSystem damageExplosion;
    [SerializeField] GameObject defeatingObject;
    public bool isIndestructibleShield { get; set; }
    public bool isIndestructibleSpawn { get; set; }
    
    public bool IsAlive => CurrentHp > 0;
    public Collider EnemyCollider;

    public void Awake()
    {
        CurrentHp = MaxHp;
        EnemyCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        Hp = CurrentHp;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void TakeDamage(int damage)
    {
        if (isIndestructibleShield || isIndestructibleSpawn) return;
        
        damageExplosion.gameObject.SetActive(true);
        CurrentHp -= damage;
        if (!IsAlive)
        {
            Instantiate(defeatingObject, transform.position, Quaternion.identity);
            
            EnemyCollider.enabled = false;
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