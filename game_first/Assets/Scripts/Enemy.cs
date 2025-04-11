using UnityEngine;

public abstract class Enemy: MonoBehaviour
{
    protected virtual void Shoot()
    {

    }
    protected abstract void Move();
}