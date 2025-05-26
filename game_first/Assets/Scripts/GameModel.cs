using UnityEngine;
using System.Collections.Generic;

public static class GameModel
{
    public static PlayerMovement Player => _player ?? throw new System.Exception("Player not set!");
    private static PlayerMovement _player;
    public static Vector3 PlayerPosition => playerTransform.position;

    private static Transform playerTransform;

    public static int playersMoney = 15000;
    
    public static Texture currentTexture;
    public static Texture selectedTexture;
    public static int selectedTextureCost;
    public static List<Texture> playersTextures = new ();
    
    public static WeaponSwitcher WeaponSwitcher => _weaponSwitcher ?? throw new System.Exception("WeaponSwitcher not set!");
    private static WeaponSwitcher _weaponSwitcher;
    
    public static readonly Dictionary<Enemy, int> Enemies = new();
    public static int CountEnemies => Enemies.Count;
    public static int Score;

    private static float _unscaledTime;
    public static float UnscaledDeltaTime => Time.timeScale != 0 ? Time.unscaledDeltaTime : 0;
    public static float UnscaledTime
    {
        get
        {
            if (Time.timeScale == 0)
                return _unscaledTime;
            _unscaledTime = Time.time / Time.timeScale;
            return _unscaledTime;
        }
    }
    
    public static void SetPlayerTransform(Transform transform)
    {
        playerTransform = transform;
    }

    public static void SetPlayerMovement(PlayerMovement player)
    {
        if ( _player is not null ) return;
        _player = player;
    }
    
    public static void SetWeaponSwitcher(WeaponSwitcher weaponSwitcher)
    {
        if ( _weaponSwitcher is not null ) return;
        _weaponSwitcher = weaponSwitcher;
    }

    public static void AddEnemy(Enemy enemy)
    {
        Enemies.Add(enemy, Enemies.Count + 1);
    }
    
    public static void RemoveEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy);
        Score += 1;
    }

    
    public static void ResetModel()
    {
        _player = null;
        _weaponSwitcher = null;
        Enemies.Clear();
    }

    private static float maxTimeScale = 9f;
    private static float cooldownBoost = 2f;
    private static float boost = 0.02f;
    private static float updateCooldownBoost;
    // x - сколько минут потребуется, чтобы достичь значения value
    // value от [1, maxTimeScale]
    // 1 + x * 60 * boost / cooldownBoost = value
    private static void UpTimeScale() // должен быть в Update
    {
        if (Time.timeScale >= maxTimeScale) return;

        updateCooldownBoost -= UnscaledDeltaTime;
        if (updateCooldownBoost > 0) return;

        Time.timeScale += boost;
        updateCooldownBoost = cooldownBoost;
    }
}
