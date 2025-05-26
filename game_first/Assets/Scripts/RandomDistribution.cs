using System.Linq;
using UnityEngine;

public static class RandomDistributions
{
    private static readonly System.Random rand = new();
    /// <summary>
    /// Генерирует массив, заполненный числами от 0 до n-1, с учетом заданных вероятностей.
    /// </summary>
    /// <param name="counts">Массив, содержащий количество вхождений для каждого числа.</param>
    /// <returns>Массив, содержащий сгенерированные числа.</returns>
    public static int[] CreateDistributionArray(int[] counts)
    {
        var totalSize = counts.Sum();
        var array = new int[totalSize];
        var index = 0;
        for (var i = 0; i < counts.Length; i++)
        for (var j = 0; j < counts[i]; j++)
            array[index++] = i;
        return array;
    }
    
    /// <summary>
    /// Случайным образом перемешивает массив чисел.
    /// </summary>
    /// <param name="array">Массив, содержащий заданное количество каждого из чисел от 0 до n-1.</param>
    /// <returns>Массив со случайно разбросанными величинами, количество каждого из чисел прежнее.</returns>
    public static void ShuffleArray(int[] array)
    {
        for (var i = array.Length - 1; i > 0; i--)
        {
            var j = rand.Next(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}
