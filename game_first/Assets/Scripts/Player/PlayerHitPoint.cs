using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

public class PlayerHitPoint : MonoBehaviour, IDamageable, IFillBarProvider
{
    [SerializeField] private int maxHp = 50;
    [FormerlySerializedAs("currentHp")] public int CurrentHp = 50;
    [SerializeField] ParticleSystem damageExplosion;
    [SerializeField] GameObject defeatingObject;
    public PostProcessVolume postProcessVolume;
    private Vignette vignette;
    
    public float MaxValue => maxHp;
    public float CurrentValue => CurrentHp;
    
    private void Start()
    {
        CurrentHp = maxHp;
        postProcessVolume.profile.TryGetSettings(out vignette);
        GameModel.SetPlayerHitPoint(this);
    }
    
    public void TakeDamage(int damage)
    {
        damageExplosion.gameObject.SetActive(true);
        damageExplosion.Play();
        vignette.color.Override(Color.red);
        StartCoroutine(ChangeVignetteColor(Color.white, 1f));
        CurrentHp -= damage;
        if (CurrentHp <= 0)
        {
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
