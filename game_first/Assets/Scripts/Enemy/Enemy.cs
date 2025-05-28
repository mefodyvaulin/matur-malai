using System;
using System.Collections;
using UnityEngine;

public class Enemy : EnemyAbstarct
{
    private bool isStartFollow;

    protected override void Update()
    {
        base.Update();
        if (GameModel.CountEnemies != 1) return;
        if (isStartFollow) return;
        
        StartCoroutine(StartFollow());
    }

    private IEnumerator StartFollow()
    {
        yield return new WaitForSeconds(0.5f);
        if (GameModel.CountEnemies != 1) yield break;
        
        movement.ClearMove();
        movement.Move = movement.MoveFollowerPlayer;
        isStartFollow = true;
    }
}