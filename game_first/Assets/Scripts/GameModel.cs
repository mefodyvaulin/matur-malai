using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public static class GameModel
{
    public static PlayerMovement Player => _player ?? throw new System.Exception("Player not set!");
    private static PlayerMovement _player;
    
    public static Vector3 PlayerPosition => Player.transform.position;
    
    private static List<Enemy> enemies = new();
    public static int CountEnemies => enemies.Count;

    public static void SetPlayerMovement(PlayerMovement player)
    {
        if ( _player != null ) return;
        _player = player;
    }

    public static void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }
    
    public static void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    
    public static void ResetModel()
    {
        _player = null;
        enemies.Clear();
    }
}
