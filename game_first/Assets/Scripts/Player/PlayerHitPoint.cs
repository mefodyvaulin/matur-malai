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
    [SerializeField] PlayerShield shield;
    public PostProcessVolume postProcessVolume;
    private Vignette vignette;
    public bool isIndestructibleSpeedBuff = false;
    public bool isIndestructibleShield { get; set; }
    private bool isIndestructibleReload = false;
    
    public int MaxHp => 80;
    public int CurrentHp { get; set; }
    public bool IsAlive => CurrentHp > 0;
    public float MaxValue => MaxHp;
    public float CurrentValue => CurrentHp;
    
    public bool isTraining = false;
    private float blinkDuration = 4f;
    private float visibleInterval = 0.3f;
    public Renderer playerRenderer;
    
    private void Start()
    {
        GameModel.SetPlayerShied(shield);
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
            if (isTraining)
            {
                CurrentHp = MaxHp;
                if (GameModel.Shield != null)
                    GameModel.Shield.ReanimateShield();
                GameModel.Shield.gameObject.SetActive(true);
                StartCoroutine(RealoadLife(blinkDuration));
                return;
            }
            gameOverPanel.SetActive(true);
            Instantiate(defeatingObject, transform.position, Quaternion.identity);
            Destroy(gameObject);
            GameModel.ResetModel();
        }
    }
    
    private IEnumerator ChangeVignetteColor(Color targetColor, float duration)
    {
        var startColor = vignette.color.value;
        var elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            if (Time.timeScale == 0)
            {
                yield return null;
                continue;
            }
            vignette.color.Override(Color.Lerp(startColor, targetColor, elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null; 
        }
        vignette.color.Override(targetColor);
    }

    private IEnumerator RealoadLife(float time)
    {
        var elapsedTime = 0f;
        isIndestructibleReload = true;
        while (elapsedTime < time)
        {
            if (Time.timeScale == 0)
            {
                yield return null;
                continue;
            }
            var blinkTime = (time - elapsedTime) % (visibleInterval * 2);
            var isVisible = blinkTime < visibleInterval;

            if (playerRenderer != null)
                playerRenderer.enabled = isVisible;
            
            elapsedTime += Time.deltaTime;
            yield return null; 
        }
        isIndestructibleReload = false;
    }
}
