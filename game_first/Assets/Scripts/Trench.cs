using System.Collections.Generic;
using UnityEngine;

public class Trench : MonoBehaviour
{
    [SerializeField] private GameObject[] trenchSegments;
    [SerializeField] private GameObject randomSegment;
    [SerializeField] private Transform player;
    private List<GameObject> currentSegments;
    private float segmentHalfLength;
    private Vector3 initialSegmentPosition;
    private float numberOfSegments;
    private float lastSegmentVariant = 0;

    private void Start(){
        segmentHalfLength = 1.5f;

        initialSegmentPosition = new Vector3(2.52f, 
            15 - segmentHalfLength, 
            -29.6f + segmentHalfLength);

        numberOfSegments = 20;

        currentSegments = new List<GameObject>();

        for (var i = 0; i < numberOfSegments; i++){
            var segment = Instantiate(trenchSegments[0], 
                initialSegmentPosition + i * segmentHalfLength * Vector3.forward, 
                Quaternion.identity);
            currentSegments.Add(segment);

        }
    }

    private void Update(){
        GenerateContinuationOfTrench();
    }

    private void GenerateContinuationOfTrench(){
        if (player.position.z >= currentSegments[0].transform.position.z){

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

    private int GetRandomSegmentVariant(){

        var randInt = Random.Range(0, 20);
        int prefabVariant = 0;

        if (randInt == 9)
            prefabVariant = 1;
        else if (randInt == 14)
            prefabVariant = 2;

        if (lastSegmentVariant != 0)
            prefabVariant = 0;
        lastSegmentVariant = prefabVariant;

        return prefabVariant;
    }
}