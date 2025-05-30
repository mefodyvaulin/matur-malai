using System.Collections;

public class Star: AbstractBuff
{
    protected override IEnumerator DoBuff()
    {
        GameModel.playerSessionMoney += 10;
        yield break;
    }
}
