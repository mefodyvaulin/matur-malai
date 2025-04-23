using UnityEngine;

public class EnemyHealth : MonoBehaviour,  IDamageable
{
    [SerializeField] private int hp = 50;
    [SerializeField] private AudioSource[] audioSources;
    
    
    public bool IsAlive => hp > 0;

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (!IsAlive)
        {
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            
            audioSources[1].Play();
            Destroy(gameObject, audioSources[1].clip.length);
        }
        else
        {
            audioSources[0].Play();
            transform.GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.white, new Color(0.9f,0.51f,0.51f), 1f);
        }
    }
    
    public void BulletExit()
    {
        transform.GetComponent<MeshRenderer>().material.color = Color.Lerp(new Color(0.9f,0.51f,0.51f), Color.white, 1f);
    }
}