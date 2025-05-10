using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public static class GameModel
{
    public static PlayerMovement Player => _player ?? throw new System.Exception("Player not set!");
    private static PlayerMovement _player;
    
    public static Vector3 PlayerPosition => Player.transform.position;
    
    public static readonly Dictionary<Enemy, int> Enemies = new();
    public static int CountEnemies => Enemies.Count;
    
    public static float UnscaledDeltaTime => Time.timeScale != 0 ? Time.unscaledDeltaTime : 0;
    public static float UnscaledTime => Time.timeScale != 0 ? Time.unscaledTime : 0;

    public static void SetPlayerMovement(PlayerMovement player)
    {
        if ( _player is not null ) return;
        _player = player;
    }

    public static void AddEnemy(Enemy enemy)
    {
        Enemies.Add(enemy, Enemies.Count + 1);
    }
    
    public static void RemoveEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy);
    }
    
    public static void ResetModel()
    {
        _player = null;
        Enemies.Clear();
    }
}
