using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class LazerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] lazersPrefab;
    private int spawnedAfter;
    private GameObject lazerPrefab;


    private void Awake()
    {
        Trench.OnGenerateContinuationOfTrench += CountFragmentsToSpawn;
    }

    private void OnDestroy()
    {
        Destroy(lazerPrefab);
        Trench.OnGenerateContinuationOfTrench -= CountFragmentsToSpawn;
    }

    private void CountFragmentsToSpawn()
    {
        spawnedAfter++;
        if (spawnedAfter == 15)
        {
            SpawnLazer();
        }
    }

    private void SpawnLazer()
    {
        var (index, vector) = GetRandomPosition();
        lazerPrefab = Instantiate(lazersPrefab[index], transform.position + vector, Quaternion.identity);
    }


    private (int, Vector3) GetRandomPosition()
    {
        var index = Random.Range(0, lazersPrefab.Length);
        return index switch
        {
            0 => (0, new Vector3(Random.Range(-1, 2) * 10, 0, Random.Range(-1, 2) * 24)),
            1 => (1, new Vector3(-16, Random.Range(0, 3) * 9 + 4, Random.Range(-1, 2) * 24)),
            2 => (2, new Vector3(Random.Range(-1, 2) * 10, 0, Random.Range(-1, 2) * 24)),
            3 => (3, new Vector3(-16, Random.Range(0, 3) * 9 + 4, Random.Range(-1, 2) * 24)),
        };
    }
}
