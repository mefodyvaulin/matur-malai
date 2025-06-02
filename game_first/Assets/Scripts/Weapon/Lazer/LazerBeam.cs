using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] protected float maxDistance = 150f;
    [SerializeField] public float width = 0.5f;
    [SerializeField] public int damagePerSecond = 5;
    [SerializeField] public float damageInterval = 0.2f;
    protected float step = 0.5f; // шаг расположения лучей (их частота)
    [SerializeField] protected Transform beamVisual; // Ссылка на цилиндр лазера
    private float damageTimer;
    protected LayerMask hitLayers;

    protected virtual void Awake()
    {
        if (step < width) step = width;
        if (beamVisual is null)
        {
            Debug.LogError("beamVisual не назначен!");
        }
    }

    public void SetLayerMask(LayerMask layerMask)
    {
        hitLayers = layerMask;
    }

    private void Update()
    {
        var origin = transform.position;
        var direction = transform.forward;

        var hits = PerformRaycasts(origin, direction, out var beamLength);
        ApplyDamage(hits);
        UpdateBeamVisual(beamLength);
    }

    private List<RaycastHit> PerformRaycasts(Vector3 origin, Vector3 direction, out float beamLength)
    {
        beamLength = maxDistance;
        var offsets = CreateOffsets();
        var hits = new List<RaycastHit>();

        foreach (var offset in offsets)
        {
            var rayOrigin = origin + offset;
            if (Physics.Raycast(rayOrigin, direction, out var hit, maxDistance, layerMask: hitLayers))
            {
                if (beamLength > hit.distance)
                {
                    hits.Clear();
                    hits.Add(hit);
                    beamLength = hit.distance;
                }
                else if (Mathf.Abs(hit.distance - beamLength) < 0.25f)
                {
                    hits.Add(hit);
                }
            }
        }

        return hits;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void ApplyDamage(List<RaycastHit> hits)
    {
        damageTimer -= GameModel.UnscaledDeltaTime;
        if (damageTimer > 0f) return;

        var isShoot = false;
        foreach (var hit in hits)
        {
            var damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable is null) continue;
            
            damageable.TakeDamage(damagePerSecond);
            isShoot = true;
        }
    
        if (isShoot) damageTimer = damageInterval;
    }



    private Vector3[] CreateOffsets()
    {
        var radius = width * 0.5f;
        var rings = Mathf.CeilToInt(radius / step);

        var result = new List<Vector3> { Vector3.zero };

        for (var i = 1; i <= rings; i++)
        {
            var currentRadius = i * step < radius ? i * step : radius;
            var segments = Mathf.RoundToInt(2 * Mathf.PI * currentRadius / step);

            for (var j = 0; j < segments; j++)
            {
                var angle = j * Mathf.PI * 2 / segments;
                var offset = transform.right * (Mathf.Cos(angle) * currentRadius) +
                             transform.up * (Mathf.Sin(angle) * currentRadius);
                result.Add(offset);
            }
        }

        return result.ToArray();
    }



    protected virtual void UpdateBeamVisual(float length)
    {
        // Позиционируем лазер (вдоль Z, центр — в середине луча)
        beamVisual.localScale = new Vector3(width, length * 0.5f, width); // длина по Z
        beamVisual.localPosition = new Vector3(0, 0, length * 0.5f); // сдвигаем вперед
    }
}