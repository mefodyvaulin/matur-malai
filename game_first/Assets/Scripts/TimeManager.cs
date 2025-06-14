using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float timeSpeed = 1f; // для дебага
    public float UnscaledDeltaTime => Time.timeScale != 0 ? Time.unscaledDeltaTime : 0;
    public float UnscaledTime { get; private set; }

    private void Awake()
    {
        GameModel.SetTimeManager(this);
    }

    private void LateUpdate()
    {
        timeSpeed = Time.timeScale;
        UpdateUnscaledTime();
        UpTimeScale();
    }

    private void UpdateUnscaledTime()
    {
        UnscaledTime += UnscaledDeltaTime;
    }
    
    private static float maxTimeScale = 3.5f;
    private static float cooldownBoost = 1.5f; //2
    private static float boost = 0.01f;
    private static float updateCooldownBoost;
    // x - сколько минут потребуется, чтобы достичь значения value
    // value от [1, maxTimeScale]
    // 1 + x * 60 * boost / cooldownBoost = value
    private static void UpTimeScale() // должен быть в Update
    {
        if (Time.timeScale >= maxTimeScale) return;

        updateCooldownBoost -= GameModel.UnscaledDeltaTime;
        if (updateCooldownBoost > 0) return;

        Time.timeScale += boost;
        updateCooldownBoost = cooldownBoost;
    }
}
