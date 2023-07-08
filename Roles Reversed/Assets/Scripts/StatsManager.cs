using System;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; set; }

    private float timer;
    public int score;
    public int health;

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
        timer = 0f;
        score = 0;
        health = 3;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        // Timer increments by Time.deltaTime
        timer += Time.deltaTime;
        //print(FormatTime(timer));
        //print("Score: " + score);
    }

    // Format time from seconds to hh:mm:ss format
    string FormatTime(float t)
    {
        TimeSpan formatted = TimeSpan.FromSeconds(t);
        return formatted.ToString(@"hh\:mm\:ss");
    }

    void Attacked(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            print("GAME OVER!");
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int score)
    {
        this.score += score;
    }
}