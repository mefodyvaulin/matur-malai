using System;
using System.Collections.Generic;
using System.Linq;

public class WeightedRandomStack<T> where T : class
{
    private readonly int[] weights;
    private readonly T[] items;
    private Stack<int> indexStack;
    
    public int Count => indexStack.Count;

    public WeightedRandomStack(T[] items, int[] weights)
    {
        if (items == null || items.Length == 0)
            throw new ArgumentException("Items list cannot be null or empty.");
        if (weights.Any(w => w < 0))
            throw new ArgumentException("Weights must be non-negative.");
        if (weights.All(w => w == 0))
            throw new ArgumentException("At least one weight must be positive.");

        this.items = items;
        this.weights = weights;
        RefillStack();
    }
    
    public WeightedRandomStack(GameObjectWithWeight[] itemsWithWeight)
    {
        if (itemsWithWeight == null || itemsWithWeight.Length == 0)
            throw new ArgumentException("Items list cannot be null or empty.");

        var filtered = itemsWithWeight
            .Select(i => new { item = i.item as T, i.weight }) 
            .Where(i => i.item != null)                        
            .ToArray();

        if (filtered.Length == 0)
            throw new ArgumentException($"No items could be cast to {typeof(T)}.");

        if (filtered.All(i => i.weight == 0))
            throw new ArgumentException("At least one weight must be positive.");
        
        items = filtered.Select(i => i.item).ToArray();
        weights = filtered.Select(i => i.weight).ToArray();
    
        RefillStack();
    }
    
    public WeightedRandomStack(ItemWithWeight<T>[] itemsWithWeight)
    {
        if (itemsWithWeight == null || itemsWithWeight.Length == 0)
            throw new ArgumentException("Items list cannot be null or empty.");

        if (itemsWithWeight.All(i => i.weight == 0))
            throw new ArgumentException("At least one weight must be positive.");

        items = itemsWithWeight.Select(i => i.item).ToArray();
        weights = itemsWithWeight.Select(i => i.weight).ToArray();

        RefillStack();
    }

    /// <summary>
    /// Возвращает следующий случайный элемент на основе весов.
    /// Автоматически перезаполняет стек, если он пуст.
    /// </summary>
    public T Pop()
    {
        if (indexStack.Count == 0)
            RefillStack();

        return items[indexStack.Pop()];
    }

    private void RefillStack()
    {
        var distribution = RandomDistributions.CreateDistributionArray(weights);
        RandomDistributions.ShuffleArray(distribution);
        indexStack = new Stack<int>(distribution);
    }
}