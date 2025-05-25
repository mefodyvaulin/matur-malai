using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerShield : Shield, IFillBarProvider
{
    [SerializeField] private FillBar fillBar;
    private Renderer shieldRenderer;
    private static float LifeTime => 20f;

    public float MaxValue => MaxHp;
    public float CurrentValue => CurrentHp;
    
    private float blinkDuration = 3f; // время мигания в секундах
    private float visibleInterval = 0.3f; // сколько щит виден/невидим при мигании
    
    // ReSharper disable Unity.PerformanceAnalysis
    public override void Init(Collider targetCollider)
    {
        shieldRenderer = GetComponent<Renderer>();
        
        base.Init(targetCollider);

        if (fillBar) fillBar.enabled = true;
        StartCoroutine(DestroyAfterUnscaledTime(LifeTime));
    }

    private IEnumerator DestroyAfterUnscaledTime(float time)
    {
        var elapsedTime = 0f;

        while (elapsedTime < time)
        {
            elapsedTime += GameModel.UnscaledDeltaTime;
            if (time - elapsedTime <= blinkDuration)
            {
                var blinkTime = (time - elapsedTime) % (visibleInterval * 2);
                var isVisible = blinkTime < visibleInterval;

                if (shieldRenderer != null)
                    shieldRenderer.enabled = isVisible;
            }

            yield return null;
        }

        // Убедимся, что щит виден перед уничтожением
        if (shieldRenderer != null)
            shieldRenderer.enabled = true;

        Die();
    }

    protected override void Die()
    {
        if (fillBar) fillBar.enabled = false;
        base.Die();
    }
}