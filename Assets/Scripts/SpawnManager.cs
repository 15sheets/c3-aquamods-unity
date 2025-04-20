using UnityEngine;
using UnityEngine.Rendering;

public class SpawnManager : MonoBehaviour
{
    public float spawnInterval; // how long to wait between spawning more fish
    public float spawnIntervalDecrease;
    public float spawnIntervalDecreaseMeters;
    private float minSpawnInterval = 0.5f;
    private float lastSpawnIntervalDecrease = 0;
    
    public int spawnsPerSpawn; // number of spawnPoints fish should spawn from every spawn interval
    public int spawnsIncrease;
    public int spawnsIncreaseMeters;
    private float lastSpawnIncrease = 0;

    public Vector2 obstacleInterval;
    public int obstaclesPerSpawn;
    public int obstaclesIncrease;
    public int obstaclesIncreaseMeters;
    private float lastObstacleIncrease=0;

    public int fishVarieties;
    public GameObject[] fishPrefabs;
    public int[] numFishToSpawn;
    public float[] fishWeights;

    public GameObject[] obstaclePrefabs;

    //private int[] randomizeFish;
    
    private Transform[] spawnPoints;
    private int[] randomizeSpawn;
    private float spawnTimer;
    private float obstacleTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastSpawnIntervalDecrease = 0;
        lastSpawnIncrease = 0;
        lastObstacleIncrease = 0;

        spawnPoints = new Transform[transform.childCount];
        randomizeSpawn = new int[transform.childCount];

        int i = 0;
        foreach (Transform child in transform)
        {
            spawnPoints[i] = child;
            randomizeSpawn[i] = i;
            i++;
        }

        spawnTimer = -1;
        obstacleTimer = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // adjust difficulty over time
        float depth = -StatMan.sm.submods.transform.position.y;

        if (depth - lastSpawnIntervalDecrease > spawnIntervalDecreaseMeters)
        {
            lastSpawnIntervalDecrease = depth;
            spawnInterval = (spawnInterval - spawnIntervalDecrease < minSpawnInterval) ? minSpawnInterval : spawnInterval - spawnIntervalDecrease;
        }
        if (depth - lastSpawnIncrease > spawnsIncreaseMeters)
        {
            lastSpawnIncrease = depth;
            spawnsPerSpawn = (spawnsPerSpawn + spawnsIncrease > transform.childCount) ? spawnsPerSpawn : spawnsPerSpawn + spawnsIncrease;
        }
        if (depth - lastObstacleIncrease > obstaclesIncreaseMeters)
        {
            lastObstacleIncrease = depth;
            obstaclesPerSpawn = (obstaclesPerSpawn + obstaclesIncrease > transform.childCount) ? obstaclesPerSpawn : obstaclesPerSpawn + obstaclesIncrease;
        }


        // spawn enemies
        if (spawnTimer < 0)
        {
            spawnRandomFish(spawnsPerSpawn);
            spawnTimer = spawnInterval;
        }
        else
        {
            spawnTimer -= Time.deltaTime;
        }

        // spawn obstacles
        if (obstacleTimer < 0)
        {
            spawnObstacles(obstaclesPerSpawn);
            obstacleTimer = Random.Range(obstacleInterval[0], obstacleInterval[1]);
        }
        else
        {
            obstacleTimer -= Time.deltaTime;
        }
    }

    public void updateFishSettings(int _fishVarieties, GameObject[] _fishPrefabs, int[] _numFishToSpawn, float[] _fishWeights)
    {
        fishVarieties = _fishVarieties;
        fishPrefabs = _fishPrefabs;
        numFishToSpawn = _numFishToSpawn;
        fishWeights = _fishWeights;

        /*
        randomizeFish = new int[_fishVarieties];
        for (int i = 0; i < fishVarieties; i++)
        {
            randomizeFish[i] = i;
        }
        */
    }

    public void updateSpawnSettings(float _spawnInterval, int _spawnsPerSpawn)
    {
        spawnInterval = _spawnInterval;
        spawnsPerSpawn = _spawnsPerSpawn;
    }

    public void spawnRandomFish(int num)
    {
        // fill in...
        // shuffle spawnpoint randomize array
        shuffleSpawnIdx();

        // spawn (num fish to spawn) amount of fish at (num) amount of random spawn points
        for (int i = 0; i < num; i++) // number of spawnpoints to spawn at
        {
            // pick a random fish type
            int fishidx = Random.Range(0, fishVarieties);

            // spawn (num fish to spawn) instances at random sp
            // hardcode a 0.53 y offset for now shrug
            for (int j = 0; j < numFishToSpawn[fishidx]; j++)
            {
                Instantiate(fishPrefabs[fishidx], spawnPoints[randomizeSpawn[i]].position + j * new Vector3(0, -0.53f, 0), spawnPoints[randomizeSpawn[i]].rotation);
            }

        }
    }

    public void spawnObstacles(int num)
    {
        shuffleSpawnIdx();

        // spawn (num fish to spawn) amount of obstacles at (num) amount of random spawn points
        for (int i = 0; i < num; i++) // number of spawnpoints to spawn at
        {
            // pick a random obstacle type
            int fishidx = Random.Range(0, obstaclePrefabs.Length);

            Instantiate(obstaclePrefabs[fishidx], spawnPoints[randomizeSpawn[i]].position, spawnPoints[randomizeSpawn[i]].rotation);

        }
    }

    private void shuffleSpawnIdx()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int tmp = randomizeSpawn[i];
            int idx = Random.Range(i, transform.childCount);
            randomizeSpawn[i] = randomizeSpawn[idx];
            randomizeSpawn[idx] = tmp;
        }
    }
}
