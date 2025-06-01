using UnityEngine;

[System.Serializable]
public class GameObjectWithWeight
{
    public GameObject item;
    [Min(0)]
    public int weight;
}

public class ItemWithWeight<T> where T : class
{
    public T item;
    [Min(0)]
    public int weight;
    
    public ItemWithWeight(T item, int weight)
    {
        this.item = item;
        this.weight = weight;
    }
}