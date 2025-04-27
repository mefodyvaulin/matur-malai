using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Trench : MonoBehaviour
{
    [SerializeField] private GameObject[] trenchSegments;
    [SerializeField] private GameObject randomSegment;
    private List<GameObject> currentSegments;
    private float segmentHalfLength;
    public static Vector3 initialSegmentPosition;
    public static float numberOfSegments;
    private readonly int[] weights = {100, 25, 8};
    private int[] randomSegmentVariants;
    private int variantIndex;

    public static event Action<TrenchState> OnGenerateContinuationOfTrench;
    public enum TrenchState // Enum для индексации объектов в TrenchSegment
    {
        Default = 0,
        Enemy = 1,
        Turret = 2
    }

    private void Start()
    {
        segmentHalfLength = 82f;

        initialSegmentPosition = new Vector3(2.52f,
            12,
            -29.6f + segmentHalfLength);

        numberOfSegments = 3;

        currentSegments = new List<GameObject>();

        for (var i = -1; i < numberOfSegments; i++){
            var segment = Instantiate(trenchSegments[0],
                initialSegmentPosition + i * segmentHalfLength * Vector3.forward,
                Quaternion.identity);
            currentSegments.Add(segment);

        }
        
        GenerateNewRandomSequence();
    }

    private void Update()
    {
        GenerateContinuationOfTrench();
    }

    private void GenerateContinuationOfTrench()
    {
        if (!(GameModel.PlayerPosition.z - segmentHalfLength >= currentSegments[0].transform.position.z)) return;
        
        var firstSegment = currentSegments[0];
        currentSegments.RemoveAt(0);
        Destroy(firstSegment);

        var prefabVariant = GetRandomSegmentVariant();

        firstSegment = Instantiate(trenchSegments[prefabVariant],
            initialSegmentPosition + numberOfSegments * segmentHalfLength * Vector3.forward,
            Quaternion.identity);
        currentSegments.Add(firstSegment);

        numberOfSegments++;
    }

    private int GetRandomSegmentVariant()
    {
        var randInt = Random.Range(0, 20);
        
        var prefabVariant = (TrenchState)randomSegmentVariants[randInt];
        variantIndex++;
        if (variantIndex == randomSegmentVariants.Length)
            GenerateNewRandomSequence();

        OnGenerateContinuationOfTrench?.Invoke(prefabVariant);
        return (int)prefabVariant;
    }

    private void GenerateNewRandomSequence()
    {
        variantIndex = 0;
        randomSegmentVariants = RandomDistributions.CreateDistributionArray(weights);
        RandomDistributions.ShuffleArray(randomSegmentVariants);
    }
}