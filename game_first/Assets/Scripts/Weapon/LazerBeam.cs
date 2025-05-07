using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] private float maxDistance = 100f;
    [SerializeField] private float width = 0.5f;
    [SerializeField] private int damagePerSecond = 1;
    [SerializeField] private float damageInterval = 0.2f;
    [SerializeField] private Transform beamVisual; // Ссылка на цилиндр лазера

    private float damageTimer;

    private void Awake()
    {
        if (beamVisual is null)
        {
            Debug.LogError("beamVisual не назначен!");
        }
    }

    private void Update()
    {
        var origin = transform.position;
        var direction = transform.forward;
        var beamLength = maxDistance;
        
        var radius = width * 0.5f;

        var offsets = CreateOffsets();
        
        var hits = new List<RaycastHit>();
        foreach (var offset in offsets)
        {
            var rayOrigin = origin + offset;
            if (Physics.Raycast(rayOrigin, direction, out var hit, maxDistance))
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
    
        damageTimer -= Time.deltaTime;
        if (damageTimer <= 0f)
        {
            foreach (var hit in hits)
            {
                var damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damagePerSecond);
                }
            }

            damageTimer = damageInterval;
        }

        UpdateBeamVisual(beamLength);
    }


    private Vector3[] CreateOffsets()
    {
        var iteration = (int)(width * 0.5f / 1f);
        if (iteration < 1)
        {
            var radius = width * 0.5f;
            return new[]
            {
                Vector3.zero, 
                transform.right * radius,
                -transform.right * radius,
                transform.up * radius,
                -transform.up * radius
            };
        }
        var result = new List<Vector3>(iteration * 8 + 1) { Vector3.zero };

        for (var i = 0; i < iteration; i++)
        {
            var radius = i * 1f;
            AppendOffsetFromRadius(result, radius);
        }
        
        return result.ToArray();
    }

    private void AppendOffsetFromRadius(List<Vector3> result, float radius)
    {
        for (var i = 0; i < 8; i++)
        {
            result.Add(transform.up * (2 * Mathf.PI * radius * i) / 8);
        }
    }

    private void UpdateBeamVisual(float length)
    {
        // Позиционируем лазер (вдоль Z, центр — в середине луча)
        beamVisual.localScale = new Vector3(width, length * 0.5f, width); // длина по Z
        beamVisual.localPosition = new Vector3(0, 0, length * 0.5f); // сдвигаем вперед
    }
}