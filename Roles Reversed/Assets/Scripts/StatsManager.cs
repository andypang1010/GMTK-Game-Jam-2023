using System;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; set; }

    public string formattedTime;
    public int score;
    public int health;

    private float timer;

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
        formattedTime = FormatTime(timer);

        if (health <= 0)
        {
            GameManager.Instance.SetGameStates(GameStates.Lost);
            //print("GAME OVER!");
        }
    }

    // Format time from seconds to hh:mm:ss format
    string FormatTime(float t)
    {
        TimeSpan formatted = TimeSpan.FromSeconds(t);
        return formatted.ToString(@"hh\:mm\:ss");
    }

    //void Attacked(int damage)
    //{
    //    health -= damage;

    //}

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int score)
    {
        this.score += score;
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int hp)
    {
        this.score += hp;
    }
}