using TMPro;
using UnityEngine;

public class EnemyHealth : MonoBehaviour,  IDamageable
{
    [SerializeField] private int hp = 50;
    [SerializeField] public AudioSource[] audioSources;
    [SerializeField] private GameObject floatingText;
    
    
    public bool IsAlive => hp > 0;

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (!IsAlive)
        {
            GetComponent<Collider>().enabled = false;
            GetComponentInChildren<MeshRenderer>().enabled = false;
            audioSources[1].Play();
            Destroy(gameObject, audioSources[1].clip.length);
        }
        else
        {
            audioSources[0].Play();
            var textDamage = Instantiate(floatingText, transform.position, Quaternion.identity);
            textDamage.GetComponentInChildren<TextMeshPro>().text = damage.ToString();
            Destroy(textDamage, 1f);
        }
    }
}