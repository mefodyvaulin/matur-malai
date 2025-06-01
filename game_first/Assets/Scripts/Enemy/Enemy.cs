using System;
using System.Collections;
using UnityEngine;

public class Enemy : EnemyAbstract
{
    private bool isStartFollow;
    protected override void Update()
    {
        UpdateAnimation();
        base.Update();
        if (GameModel.CountEnemies != 1) return;
        if (isStartFollow) return;
        
        StartCoroutine(StartFollow());
    }

    private void UpdateAnimation()
    {
        movement.animator.speed = Time.timeScale == 0 ? 0 : 1;
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