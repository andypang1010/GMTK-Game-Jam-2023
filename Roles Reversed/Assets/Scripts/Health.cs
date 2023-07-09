using UnityEngine;

public class Health : MonoBehaviour
{
    public bool isPlayer;
    public int health;
    public int maxHealth;
    public int score;

    void Start()
    {
        health = maxHealth;
        if (isPlayer) {
            StatsManager.Instance.SetHealth(maxHealth);
        }
    }

    void Update()
    {
        if (!isPlayer && health <= 0) {
            Dead();
        }
    }

    public void Attacked(int damage) {
        health -= damage;
        if (isPlayer) {
            StatsManager.Instance.SetHealth(health);
        }
    }

    public void Healed(int amount) {
        if (health + amount < maxHealth) {
            health += amount;
        }
        else {
            health = maxHealth;
        }

        if (isPlayer) {
            StatsManager.Instance.SetHealth(health);
        }
    }

    void Dead() {
        LevelManager.Instance.DecrementEnemiesRemaining();
        StatsManager.Instance.SetScore(StatsManager.Instance.GetScore() + score);
        Destroy(gameObject);
    }
}
