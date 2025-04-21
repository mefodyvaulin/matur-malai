using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Trench : MonoBehaviour
{
    [SerializeField] private GameObject[] trenchSegments;
    [SerializeField] private GameObject randomSegment;
    [SerializeField] private Transform player;
    private List<GameObject> currentSegments;
    private float segmentHalfLength;
    private Vector3 initialSegmentPosition;
    public static float numberOfSegments;
    private float _lastSegmentVariant = 0;

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

        numberOfSegments = 5;

        currentSegments = new List<GameObject>();

        for (var i = -1; i < numberOfSegments; i++){
            var segment = Instantiate(trenchSegments[0],
                initialSegmentPosition + i * segmentHalfLength * Vector3.forward,
                Quaternion.identity);
            currentSegments.Add(segment);

        }
    }

    private void Update()
    {
        GenerateContinuationOfTrench();
    }

    private void GenerateContinuationOfTrench()
    {
        if (player.position.z - segmentHalfLength >= currentSegments[0].transform.position.z){

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
    }

    private int GetRandomSegmentVariant()
    {

        var randInt = Random.Range(0, 20);
        var prefabVariant = TrenchState.Default;

        if (randInt < 10)
            prefabVariant = TrenchState.Enemy;

        else if (randInt == 14)
            prefabVariant = TrenchState.Turret;

        OnGenerateContinuationOfTrench?.Invoke(prefabVariant);
        _lastSegmentVariant = (int)prefabVariant;
        return (int)prefabVariant;
    }
}