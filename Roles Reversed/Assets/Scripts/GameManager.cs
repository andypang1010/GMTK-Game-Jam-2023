using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float timer;
    private int numEnemiesInWave;
    private int numEnemiesInGroup;
    private int numEnemiesSpawned;
    private int numEnemiesRemaining;
    private int nextSpawnInterval;
    private float lastSpawnTime;

    [Header("Waves")]
    [SerializeField]
    private int wave = 0;
    [SerializeField]
    private int minNumberOfEnemies = 3;
    [SerializeField]
    private int maxNumberOfEnemies = 7;
    [SerializeField]
    private int enemySpawnRadius = 15;
    [SerializeField]
    private int minGroupSpawnInterval = 3;
    [SerializeField]
    private int maxGroupSpawnInterval = 10;

    [Header("Stats")]
    public int score;
    public int health = 3;


    void Start()
    {
        // Initialize timer, wave, and score
        timer = 0f;
        wave = 0;
        score = 0;
        lastSpawnTime = Time.time;
    }

    void Update()
    {
        // Timer increments by Time.deltaTime
        timer += Time.deltaTime;
        //print(FormatTime(timer));

        // Game ends if no health left
        if (health <= 0)
        {
            print("GAME OVER!");
            Application.Quit();
        }

        // Start a new wave if no enemies left
        if (numEnemiesRemaining <= 0)
        {
            NewWave();
            print("NEW WAVE!");
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
                print("Next Spawn Interval: " + nextSpawnInterval);
            }
        }
    }

    // Format time from seconds to hh:mm:ss format
    string FormatTime(float t)
    {
        TimeSpan formatted = TimeSpan.FromSeconds(t);
        return formatted.ToString(@"hh\:mm\:ss");
    }

    // Create a new wave of enemies
    void NewWave()
    {
        wave++;
        print("Current Wave #: " + wave);
        numEnemiesInWave = UnityEngine.Random.Range(minNumberOfEnemies * wave, (maxNumberOfEnemies + 1) * wave);
        print(numEnemiesInWave + " enemies");
        numEnemiesRemaining = numEnemiesInWave;
        numEnemiesSpawned = 0;
    }

    // Spawn enemies at spawn radius
    void SpawnEnemies()
    {
        numEnemiesInGroup = UnityEngine.Random.Range(1, numEnemiesRemaining / 10);
        numEnemiesSpawned += numEnemiesInGroup;
        print("Spawned " + numEnemiesInGroup + " enemies at: " + Time.time);
    }
}
