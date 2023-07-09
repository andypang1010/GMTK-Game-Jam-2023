using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public bool isTypeEnemy;
    public int health;
    public int maxHealth;
    public int score;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        if (health <= 0) {
            Dead();
        }
    }

    public void Attacked(int damage) {
        health -= damage;
        if (!isTypeEnemy) {
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

        if (!isTypeEnemy) {
            StatsManager.Instance.SetHealth(health);
        }
    }

    void Dead() {
        if (isTypeEnemy) {
            LevelManager.Instance.DecrementEnemiesRemaining();
            StatsManager.Instance.SetScore(score);
            Destroy(gameObject);
        }
    }
}
