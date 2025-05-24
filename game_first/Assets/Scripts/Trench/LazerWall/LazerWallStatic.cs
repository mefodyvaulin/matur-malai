using System.Collections;

public class LazerWallStatic: LazerWallAbstract
{
    protected override int Damage => 10;
    protected override IEnumerator Move()
    {
        yield return null;
    }
}
