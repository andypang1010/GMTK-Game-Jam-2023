using System;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance { get; set; }

    [HideInInspector]
    public string formattedTime;

    [HideInInspector]
    public int health;
    public int score;

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
    }

    void Update()
    {
        // Timer increments by Time.deltaTime
        timer += Time.deltaTime;
        formattedTime = FormatTime(timer);

        if (health <= 0)
        {
            GameManager.Instance.SetGameStates(GameStates.Lost);
        }
    }

    // Format time from seconds to hh:mm:ss format
    string FormatTime(float t)
    {
        TimeSpan formatted = TimeSpan.FromSeconds(t);
        return formatted.ToString(@"hh\:mm\:ss");
    }

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int score)
    {
        this.score = score;
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int hp)
    {
        this.health = hp;
    }
}