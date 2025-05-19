using UnityEngine;

public class BackgroundFightersController : MonoBehaviour
{
    [SerializeField] private GameObject[] fighterPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    public float spawnRate = 0.1f;
    private float nextSpawnTime = 0f;
    public float yRandomLeftBorderAngle = -60f;
    public float yRandomRightBorderAngle = -35f;

    private void Start()
    {
        nextSpawnTime =  Random.Range(0f, 1f / spawnRate);
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(GameModel.PlayerPosition.x, 
            36, 
            GameModel.PlayerPosition.z);
        if (Time.time >= nextSpawnTime)
        {
            SpawnFighter();
            nextSpawnTime = Time.time + 1f / spawnRate;
        }
    }

    private void SpawnFighter()
    {

        var randomYRotation = Random.Range(yRandomLeftBorderAngle, yRandomRightBorderAngle);
        var randomSpawnPosition = Random.Range(0, spawnPoints.Length);


        var randomRotation = spawnPoints[randomSpawnPosition].rotation * Quaternion.Euler(0, randomYRotation, 0);

        Instantiate(fighterPrefabs[Random.Range(0, 2)], spawnPoints[randomSpawnPosition].position, randomRotation);
    }
}
