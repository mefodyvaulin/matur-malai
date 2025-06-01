using UnityEngine;

public abstract class EnemyGroupAbstract
{
    protected int countDrones;
    protected Vector3 spawnPosition;
    protected static readonly float minY = GameModel.PlayerMovement.trenchSizeDownLeft.y;
    protected static readonly float maxY = GameModel.PlayerMovement.trenchSizeUpRight.y;
    protected static readonly float minX = GameModel.PlayerMovement.trenchSizeDownLeft.x;
    protected static readonly float maxX = GameModel.PlayerMovement.trenchSizeUpRight.x;
    
    protected EnemyGroupAbstract(int countDrones, Vector3 spawnPosition)
    {
        this.countDrones = countDrones;
        this.spawnPosition = spawnPosition;
    }

    public abstract Vector3 TakePosition(int index);

    public abstract void MoveGroup(EnemyAbstract enemy);
}
