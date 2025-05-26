using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;

public class PlayerHitPoint : MonoBehaviour, IDamageable, IFillBarProvider, ICanShield
{
    [SerializeField] ParticleSystem damageExplosion;
    [SerializeField] GameObject defeatingObject;
    [SerializeField] GameObject gameOverPanel;
    public PostProcessVolume postProcessVolume;
    private Vignette vignette;
    public bool isIndestructibleSpeedBuff = false;
    public bool isIndestructibleShield { get; set; }
    
    public int MaxHp => 50;
    public int CurrentHp { get; set; }
    public bool IsAlive => CurrentHp > 0;
    public float MaxValue => MaxHp;
    public float CurrentValue => CurrentHp;
    
    private void Start()
    {
        CurrentHp = MaxHp;
        postProcessVolume.profile.TryGetSettings(out vignette);
        GameModel.SetPlayerHitPoint(this);
    }

    public void TakeDamage(int damage)
    {
        if (isIndestructibleSpeedBuff || isIndestructibleShield) return;
        
        damageExplosion.gameObject.SetActive(true);
        damageExplosion.Play();
        vignette.color.Override(Color.red);
        StartCoroutine(ChangeVignetteColor(Color.white, 1f));
        CurrentHp -= damage;
        if (CurrentHp <= 0)
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
