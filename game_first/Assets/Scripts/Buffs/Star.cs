using System.Collections;

public class Star: AbstractBuff
{
    protected override IEnumerator DoBuff()
    {
        Statistic.playerSessionMoney += 10;
        yield break;
    }
}
