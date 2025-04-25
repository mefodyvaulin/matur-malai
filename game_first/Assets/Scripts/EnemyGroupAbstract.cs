using UnityEngine;

public abstract class EnemyGroupAbstract
{
    protected int countDrones;
    protected Vector3 spawnPosition;
    
    protected EnemyGroupAbstract(int countDrones, Vector3 spawnPosition)
    {
        this.countDrones = countDrones;
        this.spawnPosition = spawnPosition;
    }

    public abstract Vector3 TakePosition(int index);

    public abstract void MoveGroup(Enemy enemy);
}
