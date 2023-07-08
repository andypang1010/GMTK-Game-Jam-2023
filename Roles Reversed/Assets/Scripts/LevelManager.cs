using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; set; }

    public GameObject Player;
    public GameObject Enemy;

    public int wave;
    public int numEnemiesRemaining;

    [SerializeField]
    private int minNumEnemies = 3;
    [SerializeField]
    private int maxNumEnemies = 7;
    [SerializeField]
    private int enemySpawnRadius = 15;
    [SerializeField]
    private int minGroupSpawnInterval = 3;
    [SerializeField]
    private int maxGroupSpawnInterval = 10;

    private int numEnemiesInWave;
    private int numEnemiesInGroup;
    private int numEnemiesSpawned;
    private int nextSpawnInterval;
    private float lastSpawnTime;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Initialize wave and last spawn time
        wave = 0;
        lastSpawnTime = Time.time;
    }

    void Update()
    {
        // Start a new wave if no enemies left
        if (numEnemiesRemaining <= 0)
        {
            NewWave();
        }

        // Spawn groups of enemies at a random time interval if number of enemies spawned < total number of enemies in wave
        else
        {
            if (Time.time - lastSpawnTime >= nextSpawnInterval
                && numEnemiesSpawned < numEnemiesInWave)
            {
                SpawnEnemies();
                lastSpawnTime = Time.time;
                nextSpawnInterval = UnityEngine.Random.Range(minGroupSpawnInterval, maxGroupSpawnInterval + 1);
            }
        }
    }


    // Create a new wave of enemies
    void NewWave()
    {
        wave++;
        numEnemiesInWave = UnityEngine.Random.Range(minNumEnemies * wave, (maxNumEnemies + 1) * wave);
        numEnemiesRemaining = numEnemiesInWave;
        numEnemiesSpawned = 0;
    }

    // Spawn enemies at spawn radius
    void SpawnEnemies()
    {
        numEnemiesInGroup = UnityEngine.Random.Range(1, numEnemiesRemaining / 10);
        for (int i = 0; i < numEnemiesInGroup; i++)
        {
            int spawnAngle = UnityEngine.Random.Range(0, 360);
            Vector2 spawnPosition = Player.transform.position + new Vector3(enemySpawnRadius * Mathf.Cos(spawnAngle), enemySpawnRadius * Mathf.Sin(spawnAngle), 0);
            Instantiate(Enemy, spawnPosition, new Quaternion(0, 0, 0, 0));
        }
        numEnemiesSpawned += numEnemiesInGroup;
    }

    public int GetNumEnemiesRemaining()
    {
        return numEnemiesRemaining;
    }

    public void DecrementEnemiesRemaining()
    {
        numEnemiesRemaining--;
    }
}
