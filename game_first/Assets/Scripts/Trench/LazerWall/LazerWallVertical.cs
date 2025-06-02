using System;
using System.Collections;
using UnityEngine;

public class LazerWallVertical : LazerWallAbstract
{
    protected override int Damage => 10;
    private static int Speed => 10;

    private bool isGorizontal;

    protected void Start()
    {
        // Правильная проверка направления по Z углу
        float angle = transform.rotation.eulerAngles.z;
        isGorizontal = Mathf.Approximately(angle, 90) || Mathf.Approximately(angle, 270);
        StartCoroutine(Move());
    }

    protected override IEnumerator Move()
    {
        Vector3 direction = isGorizontal ? Vector3.up : Vector3.right;

        while (true)
        {
            if (Time.timeScale == 0)
            {
                yield return null;
                continue;
            }
            if (isGorizontal)
            {
                if (transform.position.y < GameModel.PlayerMovement.trenchSizeDownLeft.y)
                    direction = Vector3.up;
                else if (transform.position.y + 3f > GameModel.PlayerMovement.trenchSizeUpRight.y)
                    direction = Vector3.down;
            }
            else
            {
                if (transform.position.x < GameModel.PlayerMovement.trenchSizeDownLeft.x)
                    direction = Vector3.right;
                else if (transform.position.x + 3f > GameModel.PlayerMovement.trenchSizeUpRight.x)
                    direction = Vector3.left;
            }

            transform.position += direction * (GameModel.UnscaledDeltaTime * Speed);
            yield return null;
        }
    }
}