using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerHitPoint : MonoBehaviour, IDamageable, IFillBarProvider
{
    [SerializeField] private int maxHp = 50;
    [SerializeField] private int currentHp = 50;
    [SerializeField] ParticleSystem damageExplosion;
    [SerializeField] GameObject defeatingObject;
    [SerializeField] GameObject gameOverPanel;
    public PostProcessVolume postProcessVolume;
    private Vignette vignette;
    
    public float MaxValue => maxHp;
    public float CurrentValue => currentHp;
    
    private void Start()
    {
        postProcessVolume.profile.TryGetSettings(out vignette);
    }
    
    public void TakeDamage(int damage)
    {
        damageExplosion.gameObject.SetActive(true);
        damageExplosion.Play();
        vignette.color.Override(Color.red);
        StartCoroutine(ChangeVignetteColor(Color.white, 1f));
        currentHp -= damage;
        if (currentHp <= 0)
        {
            gameOverPanel.SetActive(true);
            Instantiate(defeatingObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    
    private IEnumerator ChangeVignetteColor(Color targetColor, float duration)
    {
        var startColor = vignette.color.value;
        var elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            vignette.color.Override(Color.Lerp(startColor, targetColor, elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null; 
        }
        vignette.color.Override(targetColor);
    }
}
