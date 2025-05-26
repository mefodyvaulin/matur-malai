using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Trench : MonoBehaviour
{
    [SerializeField] private GameObject[] trenchSegments;
    [SerializeField] private GameObject randomSegment;
    [SerializeField] private Renderer referenceRenderer;
    private List<GameObject> currentSegments;
    private float segmentHalfLength;
    public static Vector3 initialSegmentPosition;
    private static float numberOfSegments;
    private readonly int[] weights = {1, 5, 5, 1};
    private int[] randomSegmentVariants;
    private int variantIndex;
    
    [SerializeField] private GameObject buffWeaponPrefab;


    public static event Action<TrenchState> OnGenerateContinuationOfTrench;
    public enum TrenchState // Enum для индексации объектов в TrenchSegment
    {
        Default = 0,
        Enemy = 1,
        EnemyMirror = 2,
        Turret = 3
    }

    private void Start()
    {
        segmentHalfLength = referenceRenderer.bounds.size.z;
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
        
        TrySpawnBuffWeapon(firstSegment);

        numberOfSegments++;
    }

    private int GetRandomSegmentVariant()
    {
        var prefabVariant = (TrenchState)randomSegmentVariants[variantIndex];
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
    
    private void TrySpawnBuffWeapon(GameObject segment)
    {
        if (!(Random.value <= 30f)) return;

        var center = segment.transform.position;

        var randomX = Random.Range(GameModel.Player.trenchSizeDownLeft.x, GameModel.Player.trenchSizeUpRight.x);
        var randomY = Random.Range(GameModel.Player.trenchSizeDownLeft.y, GameModel.Player.trenchSizeUpRight.y);
        var randomZ = Random.Range(center.z - segmentHalfLength / 2, center.z + segmentHalfLength / 2);

        var randomPosition = new Vector3(randomX, randomY, randomZ);
        Instantiate(buffWeaponPrefab, randomPosition, Quaternion.identity);
    }
}