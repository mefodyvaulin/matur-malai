using System;
using System.Collections.Generic;
using System.Linq;

public class WeightedRandomStack<T>
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