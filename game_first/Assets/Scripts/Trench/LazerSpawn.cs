using UnityEngine;
using Random = UnityEngine.Random;

public class LazerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] lazersPrefab;
    private int spawnedAfter;
    private GameObject lazer;


    private void Awake()
    {
        Trench.OnGenerateContinuationOfTrench += CountFragmentsToSpawn;
    }

    private void OnDestroy()
    {
        Destroy(lazer);
        Trench.OnGenerateContinuationOfTrench -= CountFragmentsToSpawn;
    }

    private void CountFragmentsToSpawn(Trench.TrenchState state)
    {
        spawnedAfter++;
        if (spawnedAfter == 2)
        {
            SpawnLazer();
        }
    }

    private void SpawnLazer()
    {
        var (index, vector) = GetRandomPosition();
        lazer = Instantiate(lazersPrefab[index], transform.position + vector, Quaternion.identity);
    }

    private (int, Vector3) GetRandomPosition()
    {
        var index = Random.Range(0, lazersPrefab.Length);
        return index switch
        {
            0 => (0, new Vector3(Random.Range(-1, 2) * 10, 0, Random.Range(-1, 2) * 24)),
            1 => (1, new Vector3(-16, Random.Range(0, 3) * 9 + 4, Random.Range(-1, 2) * 24)),
            _ => (0, Vector3.zero)
        };
    }
}
