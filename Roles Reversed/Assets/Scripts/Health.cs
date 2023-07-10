using UnityEngine;

public class Health : MonoBehaviour
{
    public bool isPlayer;
    public int health;
    public int maxHealth;
    public int score;

    public AudioClip hit;

    private AudioSource source;

    void Start()
    {
        health = maxHealth;
        if (isPlayer) {
            StatsManager.Instance.SetHealth(maxHealth);
        }
        else {
            source = GetComponent<AudioSource>();
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
        else {
            source.PlayOneShot(hit);
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
