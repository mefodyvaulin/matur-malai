using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public static class GameModel
{
    public static PlayerMovement PlayerMovement => _playerMovement ?? throw new System.Exception("PlayerMovement not set!");
    private static PlayerMovement _playerMovement;
    public static Vector3 PlayerPosition => playerTransform?.position ?? throw new System.Exception("PlayerTransform not set!");

    private static Transform playerTransform;

    public static int playersMoney = 15000;

    public static Texture currentTexture;
    public static Texture selectedTexture;
    public static int selectedTextureCost;
    public static List<Texture> playersTextures = new ();
    
    public static WeaponSwitcher WeaponSwitcher => _weaponSwitcher ?? throw new System.Exception("WeaponSwitcher not set!");
    private static WeaponSwitcher _weaponSwitcher;
    
    public static PlayerHitPoint PlayerHitPoint => _playerHitPoint ?? throw new System.Exception("PlayerHitPoint not set!");
    private static PlayerHitPoint _playerHitPoint;
    public static Collider PlayerCollider => _playerCollider ?? throw new System.Exception("PlayerHitPoint not set!");
    private static Collider _playerCollider;

    public static CameraFollow CameraFollow => _cameraFollow ?? throw new System.Exception("CameraFollow not set!");
    private static CameraFollow _cameraFollow;

    public static readonly Dictionary<EnemyAbstract, int> Enemies = new();
    public static int CountEnemies => Enemies.Count;
    public static int Score;

    private static TimeManager _timeManager;
    public static float UnscaledDeltaTime => _timeManager.UnscaledDeltaTime;
    public static float UnscaledTime => _timeManager.UnscaledTime; 

    public static void SetPlayerTransform(Transform transform)
    {
        playerTransform = transform;
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
    
    public static void SetTimeManager(TimeManager timeManager)
    {
        if ( _timeManager is not null ) return;
        _timeManager = timeManager;
    }

    public static void AddEnemy(EnemyAbstract enemy)
    {
        Enemies.Add(enemy, Enemies.Count + 1);
    }
    
    public static void RemoveEnemy(EnemyAbstract enemy)
    {
        Enemies.Remove(enemy);
        WeaponSwitcher.PourInUlta(1);
        Score += 1;
    }

    
    public static void ResetModel()
    {
        _playerMovement = null;
        _weaponSwitcher = null;
        _cameraFollow = null;
        _playerHitPoint = null;
        _playerCollider = null;
        _timeManager = null;
        Score = 0;
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
