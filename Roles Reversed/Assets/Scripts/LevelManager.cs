using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; set; }

    [Header("Characters")]
    public GameObject Player;
    public int minEnemyScore;
    public List<GameObject> Enemies;

    [Header("Wave")]
    public int wave;
    public int numEnemiesRemaining;
    public int minNumEnemies;
    public int maxNumEnemies;
    public int enemySpawnRadius;
    public int minGroupSpawnInterval;
    public int maxGroupSpawnInterval;

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

            // Randomly select enemies that can be spawned based on their score
            int randomEnemy;
            do
            {
                randomEnemy = Random.Range(0, Enemies.Count);
            } while (Enemies[randomEnemy].GetComponent<Health>().score / minEnemyScore > LevelManager.Instance.GetWave());
            Instantiate(Enemies[randomEnemy], spawnPosition, new Quaternion(0, 0, 0, 0));
        }
        numEnemiesSpawned += numEnemiesInGroup;
    }

    public int GetWave()
    {
        return wave;
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
