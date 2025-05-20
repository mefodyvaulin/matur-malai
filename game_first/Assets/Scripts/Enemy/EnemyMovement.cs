using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    
    public int direction;

    [SerializeField] private float distanceToEnemy;

    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float maxRotationAngle = 15f;
    private Animator animator;
    private float? previousZ = null;

    public Action<Enemy> Move;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        direction = Random.Range(0, 2) == 1 ? 1 : -1;
        distanceToEnemy = Random.Range(25f, 35f);
    }

    public void MoveFollowerPlayer(Enemy enemy)
    {

        var player = GameModel.PlayerPosition;
        if (Mathf.Abs(player.x - GameModel.Player.trenchSizeUpRight.x) < 0.7f ||
            Mathf.Abs(player.x - GameModel.Player.trenchSizeDownLeft.x) < 0.7f )
            StartCoroutine(WaitForAnimationToEnd(1));
        else
        {
            animator.SetLayerWeight(1, 1);
        }
        transform.position = Vector3.Lerp(enemy.transform.position,
            new Vector3(player.x,
                        player.y,
                        player.z + distanceToEnemy),
            speed * GameModel.UnscaledDeltaTime * 0.3f);

        enemy.shooting.UpdateShooting(0.3f);
    }

    private IEnumerator WaitForAnimationToEnd(int layerIndex)
    {
        // Проверяем, есть ли активная анимация на слое
        while (animator.GetCurrentAnimatorClipInfo(layerIndex).Length > 0)
        {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);

            if (stateInfo.normalizedTime >= 1f)
                break;

            yield return null;
        }

        animator.SetLayerWeight(layerIndex, 0f);
    }

    public void DefaultMove()
    {
        Move += MoveBack;
    }
    
    private void MoveBack(Enemy enemy)
    {
        var currentZ = GameModel.PlayerPosition.z;

        if (!previousZ.HasValue)
        {
            previousZ = currentZ;
            return;
        }

        var deltaZ = currentZ - previousZ.Value;
        previousZ = currentZ;

        transform.Translate(Vector3.back * deltaZ);
    }

    public void ClearMove()
    {
        Move = null;
    }
}