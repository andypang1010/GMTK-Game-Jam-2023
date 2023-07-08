using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float timer;

    [Header("Waves")]
    public int wave;
    [SerializeField]
    private int minNumberOfEnemies;
    [SerializeField]
    private int maxNumberOfEnemies;
    [SerializeField]
    private int enemySpawnRadius;
    [SerializeField]
    private int totalNumberOfEnemies;
    [SerializeField]
    private int numEnemiesPerGroup;
    [SerializeField]
    private int groupSpawnInterval;
    [SerializeField]
    private int numberOfEnemiesRemaining;

    [Header("Stats")]
    public int score;
    public int health;


    void Start()
    {
        // Initialize timer, wave, and score
        timer = 0f;
        wave = 1;
        score = 0;
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
        if (numberOfEnemiesRemaining <= 0)
        {
            NewWave();
            print("NEW WAVE!");
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
        totalNumberOfEnemies = UnityEngine.Random.Range(minNumberOfEnemies * wave, (maxNumberOfEnemies + 1) * wave);
        numberOfEnemiesRemaining = totalNumberOfEnemies;
    }

    void SpawnEnemies()
    {

    }
}
