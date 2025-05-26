using System.Collections;
using UnityEngine;

public class LazerWallVertical: LazerWallAbstract
{
    protected override int Damage => 10;
    private static int Speed => 10;
    protected override IEnumerator Move()
    {
        var direction = Vector3.right;
        while (true)
        {
            if (transform.position.x < GameModel.PlayerMovement.trenchSizeDownLeft.x)
                direction = Vector3.right;
            else if (transform.position.x + 2f > GameModel.PlayerMovement.trenchSizeUpRight.x)
                direction = Vector3.left;
            transform.position += direction * (GameModel.UnscaledDeltaTime * Speed);
            yield return null;
        }
    }
}
