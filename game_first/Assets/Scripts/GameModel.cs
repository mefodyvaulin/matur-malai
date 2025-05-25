using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public static class GameModel
{
    public static PlayerMovement PlayerMovement => _playerMovement ?? throw new System.Exception("PlayerMovement not set!");
    private static PlayerMovement _playerMovement;
    public static Vector3 PlayerPosition => PlayerMovement.transform.position;
    
    public static WeaponSwitcher WeaponSwitcher => _weaponSwitcher ?? throw new System.Exception("WeaponSwitcher not set!");
    private static WeaponSwitcher _weaponSwitcher;
    
    public static PlayerHitPoint PlayerHitPoint => _playerHitPoint ?? throw new System.Exception("PlayerHitPoint not set!");
    private static PlayerHitPoint _playerHitPoint;
    public static Collider PlayerCollider => _playerCollider ?? throw new System.Exception("PlayerHitPoint not set!");
    private static Collider _playerCollider;
    
    public static CameraFollow CameraFollow => _cameraFollow ?? throw new System.Exception("CameraFollow not set!");
    private static CameraFollow _cameraFollow;
    
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

    public static void SetPlayerMovement(PlayerMovement player)
    {
        if ( _playerMovement is not null ) return;
        _playerMovement = player;
    }
    
    public static void SetWeaponSwitcher(WeaponSwitcher weaponSwitcher)
    {
        if ( _weaponSwitcher is not null ) return;
        _weaponSwitcher = weaponSwitcher;
    }
    
    public static void SetPlayerHitPoint(PlayerHitPoint playerHitPoint)
    {
        if ( _playerHitPoint is not null ) return;
        _playerHitPoint = playerHitPoint;
        _playerCollider = playerHitPoint.GetComponent<Collider>();
    }
    
    public static void SetCameraFollow(CameraFollow cameraFollow)
    {
        if ( _cameraFollow is not null ) return;
        _cameraFollow = cameraFollow;
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
        _playerMovement = null;
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
