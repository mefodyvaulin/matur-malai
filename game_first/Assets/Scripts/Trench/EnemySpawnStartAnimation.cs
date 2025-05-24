using System;
using UnityEngine;
using System.Collections;

public static class EnemySpawnStartAnimation
{
    private static readonly float moveForwardTime = 0.2f;
    private static readonly float departurTime = 1.8f;
    private static readonly float turningTime = 0.5f;
    
    public static IEnumerator MoveToPosition(Enemy enemy, Vector3 toPosition, Action<Enemy> moveGroup)
    {
        //yield return MoveForward(enemy, moveForwardTime);
        var startPosition = enemy.transform.position;
        //var nearlyWall = NearlyWall(toPosition);
        var controlPoint1 = new Vector3(
            (startPosition.x + toPosition.x) * 0.75f,
            2 * startPosition.y - toPosition.y,
            startPosition.z
        );

        var controlPoint2 = new Vector3(
            toPosition.x,
            toPosition.y,
            toPosition.z + (startPosition.z - toPosition.z) * 0.5f
        );

        var elapsed = 0f;
        var previousPosition = startPosition;

        while (elapsed < departurTime)
        {
            var t = elapsed / departurTime;
            var curvedPosition = Bezier.Cubic(
                startPosition, controlPoint1, controlPoint2, toPosition, t);

            enemy.transform.position = curvedPosition;

            // Направление между текущей и предыдущей позицией
            var direction = (curvedPosition - previousPosition).normalized;

            if (direction != Vector3.zero)
            {
                var targetRotation = Quaternion.LookRotation(direction);
                enemy.transform.rotation =
                    Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            previousPosition = curvedPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }

        enemy.transform.position = toPosition;
        
        // Плавный поворот в нужную сторону
        if (toPosition.z == controlPoint2.z) toPosition.z -= 0.1f; // для правильного поворота
        var finalDir = (toPosition - controlPoint2).normalized;
        var finalRot = Quaternion.LookRotation(finalDir);
        yield return SmoothRotate(enemy, finalRot, turningTime);
        
        enemy.movement.DefaultMove();
        enemy.movement.Move += moveGroup;
    }

    private static IEnumerator MoveForward(Enemy enemy, float time)
    {
        var originalPosition = enemy.transform.position;
        var offsetPosition = originalPosition + 4 * enemy.transform.forward.normalized;
  
        var elapsedOffset = 0f;

        while (elapsedOffset < time)
        {
            var t = elapsedOffset / time;
            enemy.transform.position = Vector3.Lerp(originalPosition, offsetPosition, t);
            elapsedOffset += Time.deltaTime;
            yield return null;
        }
        enemy.transform.position = offsetPosition;
    }
    
    private static IEnumerator SmoothRotate(Enemy enemy, Quaternion targetRotation, float time)
    {
        var startRotation = enemy.transform.rotation;
        var elapsed = 0f;

        while (elapsed < time)
        {
            enemy.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / time);
            elapsed += Time.deltaTime;
            yield return null;
        }

        enemy.transform.rotation = targetRotation;
    }

    private static float NearlyWall(Vector3 toPosition)
    {
        if (Mathf.Abs(toPosition.x - GameModel.PlayerMovement.trenchSizeDownLeft.x) <= 1f
            || Mathf.Abs(toPosition.x - GameModel.PlayerMovement.trenchSizeUpRight.x) <= 1f)
        {
            return toPosition.x + 10f;
        }
        return toPosition.x;
    }
}
