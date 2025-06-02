using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerShield : MonoBehaviour, IFillBarProvider, IDamageable
{
    [SerializeField] private GameObject fillBar;
    private Renderer shieldRenderer;
    public AudioSource AudioSource;

    private Collider targetCollider;
    private ICanShield targetShield;
    [SerializeField] private GameObject floatingTextPrefab;
    private static float LifeTime => 20f;

    public float CurrentValue => CurrentHp;
    public float MaxValue => MaxHp;

    public int MaxHp => 40;

    public int CurrentHp
    {
        get => _currentHp;
        set => _currentHp = value;
    }

    public bool UpLifeTime;
    
    private float blinkDuration = 3f; // время мигания в секундах
    private float visibleInterval = 0.3f; // сколько щит виден/невидим при мигании
    private int _currentHp;

    // ReSharper disable Unity.PerformanceAnalysis

    private void OnEnable()
    {
        CurrentHp = MaxHp;
        shieldRenderer = GetComponent<Renderer>();

        targetCollider = GameModel.PlayerCollider;


        var target = targetCollider.gameObject;

        targetShield = target.GetComponent<ICanShield>();
        if (targetShield == null) throw new Exception("Target can not shield.");
        targetShield.isIndestructibleShield = true;
        transform.position = targetCollider.bounds.center;


        if (AudioSource) AudioSource.Play();

        fillBar.SetActive(true);
        fillBar.GetComponentInChildren<FillBar>().providerScript = this;

        StartCoroutine(DestroyAfterUnscaledTime(LifeTime));
    }

    public void ReanimateShield()
    {
        UpLifeTime = true;
        CurrentHp = MaxHp;
    }

    private IEnumerator DestroyAfterUnscaledTime(float time)
    {
        var elapsedTime = 0f;

        while (elapsedTime < time)
        {
            if (Time.timeScale == 0)
            {
                yield return null;
                continue;
            }
            elapsedTime += GameModel.UnscaledDeltaTime;
            if (time - elapsedTime <= blinkDuration)
            {
                var blinkTime = (time - elapsedTime) % (visibleInterval * 2);
                var isVisible = blinkTime < visibleInterval;

                if (shieldRenderer != null)
                    shieldRenderer.enabled = isVisible;
            }

            if (UpLifeTime)
            {
                elapsedTime = 0;
                UpLifeTime = false;
            }
            yield return null;
        }

        // Убедимся, что щит виден перед уничтожением
        if (shieldRenderer != null)
            shieldRenderer.enabled = true;
        Die();
    }

    public void TakeDamage(int damage)
    {
        CurrentHp -= damage;
        if (floatingTextPrefab != null)
        {
            var textDamage = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
            textDamage.GetComponentInChildren<TextMeshPro>().text = damage.ToString();
            Destroy(textDamage, 1f);
        }
        if (CurrentHp <= 0)
            Die();
    }

    private void Die()
    {
        targetShield.isIndestructibleShield = false;
        fillBar.SetActive(false);
        gameObject.SetActive(false);
    }
}