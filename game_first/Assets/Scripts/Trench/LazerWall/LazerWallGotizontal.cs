using System.Collections;
using UnityEngine;

public class LazerWallGotizontal: LazerWallAbstract
{
    protected override int Damage => 10;
    private static int Speed => 10;

    protected override IEnumerator Move()
    {
        var direction = Vector3.up;
        while (true)
        {
            if (transform.position.y < GameModel.Player.trenchSizeDownLeft.y)
                direction = Vector3.up;
            else if (transform.position.y + 6f > GameModel.Player.trenchSizeUpRight.y)
                direction = Vector3.down;
            transform.position += direction * (GameModel.UnscaledDeltaTime * Speed);
            yield return null;
        }
    }
}
