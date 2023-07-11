using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    public GameObject healthPopup;
    public AudioClip hitAudio;

    public bool isPlayer;
    public int health;
    public int maxHealth;
    public int score;

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

    public void Attacked(int damage)
    {

        // Inflict damage
        health -= damage;

        ShowPopup();

        // If the character is the player, update the health on stats manager
        if (isPlayer)
        {
            StatsManager.Instance.SetHealth(health);
        }
        else
        {
            // Play audio
            source.PlayOneShot(hitAudio);
        }
    }

    private void ShowPopup()
    {
        // If popup is not null, show a popup at the character's position
        if (healthPopup)
        {
            var ins = Instantiate(healthPopup, transform.position, Quaternion.identity, transform);

            // Set the text to the remaining health of the character if health is positive
            if (health > 0) {
                ins.GetComponent<TextMeshPro>().SetText(health.ToString());
            }
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
