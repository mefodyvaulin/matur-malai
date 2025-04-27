using UnityEngine;

public static class RandomDistributions
{
    /// <summary>
    /// Генерирует массив, заполненный числами от 0 до n-1, с учетом заданных вероятностей.
    /// </summary>
    /// <param name="counts">Массив, содержащий количество вхождений для каждого числа.</param>
    /// <returns>Массив, содержащий сгенерированные числа.</returns>
    public static int[] CreateDistributionArray(int[] counts)
    {
        var totalSize = 0;
        foreach (var count in counts)
            totalSize += count;
        var array = new int[totalSize];
        var index = 0;
        for (int i = 0; i < counts.Length; i++)
        for (int j = 0; j < counts[i]; j++)
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
        for (int k = 0; k < 3 * array.Length; k++)
        for (int i = array.Length - 1; i > 3; i--)
        {
            var j = Random.Range(0, i - 2);
            var temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}
