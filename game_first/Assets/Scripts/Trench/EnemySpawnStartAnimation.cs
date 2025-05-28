using System;
using UnityEngine;
using System.Collections;

public static class EnemySpawnStartAnimation
{
    private static readonly float moveForwardTime = 0.4f;
    private static readonly float departurTime = 2f;
    private static readonly float turningTime = 0.5f;
    
    public static IEnumerator MoveToPosition(EnemyAbstarct enemy, Vector3 toPosition, Action<EnemyAbstarct> moveGroup, int i,
        Vector3? controlPoint1 = null, Vector3? controlPoint2 = null, float flightTime = 0)
    {
        //yield return MoveForward(enemy, moveForwardTime);
        if (flightTime <= 0) flightTime = departurTime;
        var time = flightTime - i * 0.2f;
        
        var startPosition = enemy.transform.position;
        controlPoint1 ??= new Vector3(
            (GameModel.PlayerMovement.trenchSizeDownLeft.x + GameModel.PlayerMovement.trenchSizeUpRight.x) / 2,
            (GameModel.PlayerMovement.trenchSizeDownLeft.y + GameModel.PlayerMovement.trenchSizeUpRight.y) / 2,
            startPosition.z
        );
        controlPoint2 ??= new Vector3(
            toPosition.x,
            toPosition.y,
            toPosition.z + (startPosition.z - toPosition.z) * 0.5f
        );
        var cp1 = controlPoint1.Value;
        var cp2 = controlPoint2.Value;
        
        var elapsed = 0f;
        var previousPosition = startPosition;

        while (elapsed < time)
        {
            var t = elapsed / time;
            var curvedPosition = Bezier.Cubic(
                startPosition, cp1, cp2, toPosition, t);

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
        if (toPosition.z == cp2.z) toPosition.z -= 0.1f; // для правильного поворота
        var finalDir = (toPosition - cp2).normalized;
        var finalRot = Quaternion.LookRotation(finalDir);
        yield return SmoothRotate(enemy, finalRot, turningTime);
        
        enemy.movement.DefaultMove();
        enemy.movement.Move += moveGroup;
    }

    private static IEnumerator MoveForward(EnemyAbstarct enemy, float time)
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
    
    private static IEnumerator SmoothRotate(EnemyAbstarct enemy, Quaternion targetRotation, float time)
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
}
