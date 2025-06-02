using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Trench : MonoBehaviour
{
    [SerializeField] private GameObject star;
    private bool starCanSpawn = true;
    
    [SerializeField] private GameObject emptyStartSegment;
    [Min(5)] [SerializeField] private int totalNumberOfFloodedTrenches = 50;
    [SerializeField] public GameObject bossBar;
    private int skipStartTrenches = 2;
    
    [SerializeField] private GameObject bossTrenchSegment;
    [SerializeField] private Location[] locations;
    [Min(5)] [SerializeField] private int locationSegmentsCount = 50;
    private int locationIndex = 0;
    private int locationSegmentIndex = 0;
    public bool IsBossLocation => locationIndex == locations.Length - 1;
    private WeightedRandomStack<GameObject> randomTrench;
    
    
    [SerializeField] private GameObject[] buffPrefabs;
    private readonly int[] weightsBuff = {0, 0, 1, 0, 0}; // {6, 3, 1, 3, 2};
    private WeightedRandomStack<GameObject> randomBuffs;
    
    [SerializeField] private Renderer referenceRenderer;
    private float segmentLength;
    private static Vector3 initialSegmentPosition;
    
    private Queue<(GameObject trench, GameObject buff)> currentSegments;
    private int countSegments;

    public bool BossTrenchExists { get; set; }
    private Queue<float> startSegmentLocations;
    public float BossLocationSegmentPosition => bossLocationSegment * segmentLength;
    private int bossLocationSegment;
    
    private const int maxSegmentsAhead = 4;

    private bool isInstantiateSegments;
    public bool speedStop;
    
    public static event Action<float> OnGenerateContinuationOfTrench; // вызвать до создания туннеля
    
    private bool isTraining => Helper.isEducation;
    
    public void Awake()
    {
        GameModel.SetGenerateTrench(this);
    }

    private void Start()
    {
        startSpawnStars = new Vector2(
            Random.Range(GameModel.PlayerMovement.trenchSizeDownLeft.x, GameModel.PlayerMovement.trenchSizeUpRight.x),
            Random.Range(GameModel.PlayerMovement.trenchSizeDownLeft.y, GameModel.PlayerMovement.trenchSizeUpRight.y));
        segmentLength = referenceRenderer.bounds.size.z;
        initialSegmentPosition = new Vector3(2.52f,
            12,
            -29.6f + segmentLength);

        startSegmentLocations = new Queue<float>();
        currentSegments = new Queue<(GameObject trench, GameObject buff)>();
        
        locationSegmentIndex = skipStartTrenches;
        countSegments = skipStartTrenches;
        for (var i = -1; i < skipStartTrenches; i++)
        {
            var segment = Instantiate(emptyStartSegment,
                initialSegmentPosition + i * segmentLength * Vector3.forward,
                Quaternion.identity);
            currentSegments.Enqueue((segment, null));
        }
        randomTrench = new WeightedRandomStack<GameObject>(locations[locationIndex].trenches);
        randomBuffs = new WeightedRandomStack<GameObject>(buffPrefabs, weightsBuff);
        InstantiateSegments(totalNumberOfFloodedTrenches - skipStartTrenches);
    }

    private void Update()
    {
        speedStop = false;
        OnGenerateContinuationOfTrench?.Invoke(segmentLength);
        UpdateLocation();
        GenerateContinuationOfTrench();
        if (GameModel.GenerateTrench.startSegmentLocations.Count > 0
            && GameModel.PlayerPosition.z >= startSegmentLocations.Peek() * segmentLength)
        {
            speedStop = true;
            startSegmentLocations.Dequeue();
        }
    }

    private void UpdateLocation()
    {
        if (isTraining)
        {
            locationSegmentIndex = 0;
            locationIndex = 0;
            return;
        }
        if (locationSegmentIndex < locationSegmentsCount 
            || (IsBossLocation && BossTrenchExists)
            ) return;
        
        var lastLocation = IsBossLocation;
        locationSegmentIndex = 0;
        startSegmentLocations.Enqueue(countSegments);
        locationIndex = locationIndex == locations.Length - 1 ? 0 : locationIndex + 1;
        randomTrench = new WeightedRandomStack<GameObject>(locations[locationIndex].trenches);
        
        if (IsBossLocation)
        {
            bossLocationSegment = countSegments;
            starCanSpawn = false;
        }
        if (lastLocation)
        {
            starCanSpawn = true;
            ReloadSegments();
        }
    }

    private void GenerateContinuationOfTrench()
    {
        if (!(GameModel.PlayerPosition.z - segmentLength >= currentSegments.Peek().trench.transform.position.z)) return;
        
        var firstSegment = currentSegments.Dequeue();
        Destroy(firstSegment.trench);
        
        var newSegment = Instantiate(TakeSpecialOrDefaultSegment(),
            initialSegmentPosition + countSegments * segmentLength * Vector3.forward,
            Quaternion.identity);
        var buff = TrySpawnBuff(newSegment);
        currentSegments.Enqueue((newSegment, buff));
        locationSegmentIndex++;
        countSegments++;
        SpawnStar();
    }

    private GameObject TakeSpecialOrDefaultSegment()
    {
        if (isTraining)
        {
            return emptyStartSegment;
        }
        if (IsBossLocation
            && locationSegmentIndex >= 3 
            && !BossTrenchExists 
            )
        {
            BossTrenchExists = true;
            return bossTrenchSegment;
        }
        return randomTrench.Pop();
    }
    
    private GameObject TrySpawnBuff(GameObject segment)
    {
        if (!(Random.value <= 0.7f) || isTraining) return null;

        var center = segment.transform.position;

        var randomX = Random.Range(GameModel.PlayerMovement.trenchSizeDownLeft.x, GameModel.PlayerMovement.trenchSizeUpRight.x);
        var randomY = Random.Range(GameModel.PlayerMovement.trenchSizeDownLeft.y, GameModel.PlayerMovement.trenchSizeUpRight.y);
        var randomZ = Random.Range(center.z - segmentLength / 2, center.z + segmentLength / 2);

        var randomPosition = new Vector3(randomX, randomY, randomZ);
        
        return Instantiate(randomBuffs.Pop(), randomPosition, Quaternion.identity);
    }

    public void ReloadSegments()
    {
        startSegmentLocations.Clear();
        InstantiateSegments(CleanNewTrenchSegments());
    }
    
    private int CleanNewTrenchSegments()
    {
        var playerZ = GameModel.PlayerPosition.z;
        var maxZ = playerZ + maxSegmentsAhead * segmentLength;

        var removedCount = 0;
        var filteredSegments = new Queue<(GameObject trench, GameObject buff)>();
        while (currentSegments.Count > 0)
        {
            var segment = currentSegments.Dequeue();

            if (segment.trench != null && segment.trench.transform.position.z <= maxZ)
            {
                filteredSegments.Enqueue(segment);
            }
            else
            {
                Destroy(segment.trench);
                Destroy(segment.buff);
                removedCount++;
            }
        }
        
        currentSegments = filteredSegments;
        countSegments -= removedCount;
        return removedCount;
    }
    
    private void InstantiateSegments(int count)
    {
        isInstantiateSegments = true;
        for (var i = 0; i < count; i++)
        {
            UpdateLocation();
            var segment = Instantiate(TakeSpecialOrDefaultSegment(),
                initialSegmentPosition + countSegments * segmentLength * Vector3.forward,
                Quaternion.identity);
            var buff = TrySpawnBuff(segment);
            currentSegments.Enqueue((segment, buff));
            locationSegmentIndex++;
            countSegments++;
            SpawnStar();
        }
        isInstantiateSegments = false;
    }
    
    private Vector2 startSpawnStars, endSpawnStars;
    private void SpawnStar()
    {
        if (!(Random.value <= 0.5f) || !starCanSpawn || isTraining) return;
        
        endSpawnStars = new Vector2(
            Random.Range(GameModel.PlayerMovement.trenchSizeDownLeft.x, GameModel.PlayerMovement.trenchSizeUpRight.x),
            Random.Range(GameModel.PlayerMovement.trenchSizeDownLeft.y, GameModel.PlayerMovement.trenchSizeUpRight.y)
        );

        var startZ = countSegments * segmentLength;

        var steps = (int)segmentLength / 10;
        for (var i = 0; i < steps; i++)
        {

            var t = (float)i / steps;
            var lerpedPos = Vector2.Lerp(startSpawnStars, endSpawnStars, t);


            var currentZ = startZ + i * 10f;
            Instantiate(star, new Vector3(lerpedPos.x, lerpedPos.y, currentZ), Quaternion.identity);
        }


        startSpawnStars = endSpawnStars;
    }
}

[Serializable]
public class Location
{
    public GameObjectWithWeight[] trenches;
}