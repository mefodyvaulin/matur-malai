using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Trench : MonoBehaviour
{
    [SerializeField] private GameObject[] trenchSegments;
    private readonly int[] weightsTrench = {1, 2, 2, 4};
    private WeightedRandomStack<GameObject> randomTrench;
    
    [SerializeField] private GameObject[] buffPrefabs;
    private readonly int[] weightsBuff = {3, 2};
    private WeightedRandomStack<GameObject> randomBuffs;
    
    [SerializeField] private Renderer referenceRenderer;
    private float segmentHalfLength;
    private static Vector3 initialSegmentPosition;
    
    private Queue<GameObject> currentSegments;
    private static float countSegments;
    
    public static event Action OnGenerateContinuationOfTrench; // вызвать до создания туннеля
    
    private void Start()
    {
        segmentHalfLength = referenceRenderer.bounds.size.z;
        initialSegmentPosition = new Vector3(2.52f,
            12,
            -29.6f + segmentHalfLength);

        countSegments = 3;

        currentSegments = new Queue<GameObject>();

        for (var i = -1; i < countSegments; i++){
            var segment = Instantiate(trenchSegments[0],
                initialSegmentPosition + i * segmentHalfLength * Vector3.forward,
                Quaternion.identity);
            currentSegments.Enqueue(segment);
        }

        randomTrench = new WeightedRandomStack<GameObject>(trenchSegments, weightsTrench);
        randomBuffs = new WeightedRandomStack<GameObject>(buffPrefabs, weightsBuff);
    }

    private void Update()
    {
        GenerateContinuationOfTrench();
    }

    private void GenerateContinuationOfTrench()
    {
        if (!(GameModel.PlayerPosition.z - segmentHalfLength >= currentSegments.Peek().transform.position.z)) return;
        
        var firstSegment = currentSegments.Dequeue();
        Destroy(firstSegment);
        
        OnGenerateContinuationOfTrench?.Invoke();
        var newSegment = Instantiate(randomTrench.Pop(),
            initialSegmentPosition + countSegments * segmentHalfLength * Vector3.forward,
            Quaternion.identity);
        currentSegments.Enqueue(newSegment);
        
        TrySpawnBuffWeapon(newSegment);
        countSegments++;
    }
    
    private void TrySpawnBuffWeapon(GameObject segment)
    {
        if (!(Random.value <= 0.7f)) return;

        var center = segment.transform.position;

        var randomX = Random.Range(GameModel.PlayerMovement.trenchSizeDownLeft.x, GameModel.PlayerMovement.trenchSizeUpRight.x);
        var randomY = Random.Range(GameModel.PlayerMovement.trenchSizeDownLeft.y, GameModel.PlayerMovement.trenchSizeUpRight.y);
        var randomZ = Random.Range(center.z - segmentHalfLength / 2, center.z + segmentHalfLength / 2);

        var randomPosition = new Vector3(randomX, randomY, randomZ);
        
        Instantiate(randomBuffs.Pop(), randomPosition, Quaternion.identity);
    }
}