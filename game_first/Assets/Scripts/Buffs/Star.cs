using System.Collections;

public class Star: AbstractBuff
{
    protected override IEnumerator DoBuff()
    {
        GameModel.playersMoney += 10;
        yield break;
    }
}
