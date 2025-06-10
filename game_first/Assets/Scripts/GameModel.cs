using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Buffs;

public static class GameModel
{
    public static PlayerMovement PlayerMovement => _playerMovement ?? throw new System.Exception("PlayerMovement not set!");
    private static PlayerMovement _playerMovement;
    public static Vector3 PlayerPosition => playerTransform?.position ?? throw new System.Exception("PlayerTransform not set!");

    private static Transform playerTransform;

    public static int Harder = 0;
    
    public static WeaponSwitcher WeaponSwitcher => _weaponSwitcher ?? throw new System.Exception("WeaponSwitcher not set!");
    private static WeaponSwitcher _weaponSwitcher;
    
    public static PlayerHitPoint PlayerHitPoint => _playerHitPoint ?? throw new System.Exception("PlayerHitPoint not set!");
    private static PlayerHitPoint _playerHitPoint;
    public static Collider PlayerCollider => _playerCollider ?? throw new System.Exception("PlayerHitPoint not set!");
    private static Collider _playerCollider;

    public static CameraFollow CameraFollow => _cameraFollow ?? throw new System.Exception("CameraFollow not set!");
    private static CameraFollow _cameraFollow;
    
    public static Trench GenerateTrench => _generateTrench ?? throw new System.Exception("GenerateTrench not set!");
    private static Trench _generateTrench;

    public static PlayerShield Shield => _shield ?? throw new System.Exception("Shied not set!");
    private static PlayerShield _shield;
    
    public static SpeedBuff SpeedBuff;

    public static readonly Dictionary<EnemyAbstract, int> Enemies = new();
    public static int CountEnemies => Enemies.Count;

    public static bool isEducation = false;

    private static TimeManager _timeManager;
    public static float UnscaledDeltaTime => _timeManager.UnscaledDeltaTime;
    public static float UnscaledTime => _timeManager.UnscaledTime;

    public static bool BossIsAlive { get; set; }

    public static void SetPlayerTransform(Transform transform)
    {
        playerTransform = transform;
    }

    public static void SetPlayerShied(PlayerShield shield)
    {
        _shield = shield;
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
    
    public static void SetGenerateTrench(Trench generateTrench)
    {
        if ( _generateTrench is not null ) return;
        _generateTrench = generateTrench;
    }
    
    public static void SetSpeedBuff(SpeedBuff speedBuff)
    {
        if ( SpeedBuff is not null ) return;
        SpeedBuff = speedBuff;
    }

    public static void AddEnemy(EnemyAbstract enemy)
    {
        Enemies.Add(enemy, Enemies.Count + 1);
    }
    
    public static void RemoveEnemy(EnemyAbstract enemy)
    {
        Enemies.Remove(enemy);
        _weaponSwitcher?.PourInUlta(1);
    }

    
    public static void ResetModel()
    {
        _playerMovement = null;
        _weaponSwitcher = null;
        _cameraFollow = null;
        _playerHitPoint = null;
        _playerCollider = null;
        _timeManager = null;
        _generateTrench = null;
        SpeedBuff = null;
        isEducation = false;
        Helper.helperAlive = false;
        playerTransform = null;
        BossIsAlive = false;
        EnemySpawn.CanSpawn = true;
        Harder = 0;
        
        Enemies.Clear();
    }

    public static void ResetEducate()
    {
        isEducation = false;
        Helper.helperAlive = false;
    }
}
