using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LazerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] lazersPrefab;
    private int spawnedAfter = 2;
    private bool spawned = false;
    private GameObject lazerPrefab;
    [SerializeField] private bool isShip;
    private static float lifeTime = 10f;

    private void Awake()
    {
        Trench.OnGenerateContinuationOfTrench += CountFragmentsToSpawn;
    }
    
    private void OnDestroy()
    {
        Destroy(lazerPrefab);
        Trench.OnGenerateContinuationOfTrench -= CountFragmentsToSpawn;
    }

    private void CountFragmentsToSpawn(float segmentLenght)
    {
        if (!spawned 
            && transform.position.z - GameModel.PlayerPosition.z <= spawnedAfter * segmentLenght 
            && transform.position.z - GameModel.PlayerPosition.z > spawnedAfter * segmentLenght - 0.96f)
        {
            spawned = true;
            SpawnLazer();
        }
    }

    private void SpawnLazer()
    {
        var lazerWalls = GetRandomPosition();
        foreach (var lazerWall in lazerWalls)
        {
            lazerPrefab = Instantiate(lazersPrefab[0], transform.position + lazerWall.Position, lazerWall.Rotation);
            lazerPrefab.transform.localScale = lazerWall.Scale;
        }
    }

    private IEnumerable<LazerWallIndetifity> GetRandomPosition()
    {
        var count = isShip ? 10 : Random.Range(1, 4);
        for (var i = 0; i < count; i++)
        {
            if (Time.timeScale == 0)
            {
                yield return null;
                continue;
            }
            yield return new LazerWallIndetifity(isShip? 2 : Random.Range(0, 2), isShip? 1 : 4);
        }
    }

}

class LazerWallIndetifity
{
    public Vector3 Position;
    public Vector3 Scale;
    public Quaternion Rotation;
    private Dictionary<int, (Edge<Vector3> Pos, Edge<Vector3> Scale, Edge<Quaternion> Rotation)> dict = new()
    {
        {
            0,
            (
                new Edge<Vector3>(new (-10, 0, -24), new (10, 0, 24)), 
                new Edge<Vector3>(new(1,2,2), new(1,2,2)),
                new Edge<Quaternion>(Quaternion.Euler(0,0,0), Quaternion.Euler(0,0,0))
            )
        },
        {
            1,
            (
                new Edge<Vector3>(new(-15, 4.5f, -24), new(-15, 22, 24)),
                new Edge<Vector3>(new(1,1,2), new(1,1,2)),
                new Edge<Quaternion>(Quaternion.Euler(0,0,-90), Quaternion.Euler(0,0,-90))
            )
        },
        {2, GenerateRandomLazerShip()}

    };

    private static (Edge<Vector3> Pos, Edge<Vector3> Scale, Edge<Quaternion> Rotation) GenerateRandomLazerShip()
    {
        var rng = Random.Range(0, 3);
        switch (rng)
        {
            case 0:
                return (
                    new Edge<Vector3>(new(13, 4, -43) , new(13, 22, 43)),
                    new Edge<Vector3>(new(0.2f,0.6f, 0.8f), new(1,2,2)),
                    new Edge<Quaternion>(Quaternion.Euler(0,0,0), Quaternion.Euler(0,30f,0))
                );
            case 1:
                return (
                    new Edge<Vector3>(new(-13, 4, -43) , new(-13, 22, 43)),
                    new Edge<Vector3>(new(0.2f,0.6f, 0.8f), new(1,2,2)),
                    new Edge<Quaternion>(Quaternion.Euler(0,-180f,0), Quaternion.Euler(0,-150f,0))
                );
            case 2:
                return (
                    new Edge<Vector3>(new(-13, 0, -43) , new(13, 0, 43)),
                    new Edge<Vector3>(new(0.2f,0.6f, 0.8f), new(1,2,2)),
                    new Edge<Quaternion>(Quaternion.Euler(0,0,-120f), Quaternion.Euler(0,0,-90f))
                );
        }
        return (null, null, null);
    }

    public LazerWallIndetifity(int index, float step)
    {
        var edge = dict[index];
        Position = GetRandomBetweenWithStep(edge.Pos.Down, edge.Pos.Up, step);
        Scale = GetRandomBetween(edge.Scale.Down, edge.Scale.Up);
        Scale.y = Scale.x;
        Rotation = GetRandomBetween(edge.Rotation.Down, edge.Rotation.Up);

    }

    private static Vector3 GetRandomBetweenWithStep(Vector3 min, Vector3 max, float step)
    {
        var randomX = Random.Range(min.x / step, max.x / step);
        var randomY = Random.Range(min.y / step, max.y / step);
        var randomZ = Random.Range(min.z / step, max.z / step);
        return new Vector3(randomX, randomY, randomZ) * step;
    }

    private static Vector3 GetRandomBetween(Vector3 min, Vector3 max)
    {
        var randomX = Random.Range(min.x, max.x);
        var randomY = Random.Range(min.y, max.y);
        var randomZ = Random.Range(min.z, max.z);
        return new Vector3(randomX, randomY, randomZ);
    }

    private static Quaternion GetRandomBetween(Quaternion min, Quaternion max)
    {
        var minEuler = min.eulerAngles;
        var maxEuler = max.eulerAngles;
        var randomX = Random.Range(minEuler.x, maxEuler.x);
        var randomY = Random.Range(minEuler.y, maxEuler.y);
        var randomZ = Random.Range(minEuler.z, maxEuler.z);
        return Quaternion.Euler(randomX, randomY, randomZ);
    }

    private class Edge<T>
    {
        public T Up, Down;

        public Edge(T down, T up)
        {
            Up = up;
            Down = down;
        }
    }
}