using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	public GameObject player;
	public GameObject proximityDetector;

	LevelManager level;
	StatsManager stats;

	public int health = 4;
	public int maxHealth = 4;
	public int score = 5;
	public Collider2D bodyCollider;
	public MeleeAttack playerAttack;

	private int playerAttackStrength;
	private float playerAttackInterval;
	private float lastAttackTime;

	void Start()
	{
		level = LevelManager.Instance;
		stats = StatsManager.Instance;

		health = maxHealth;
		lastAttackTime = Time.time;
    }

    void Update()
	{ 
		playerAttackStrength = player.GetComponent<MeleeAttack>().attackStrength;
		playerAttackInterval = 1.0f / player.GetComponent<MeleeAttack>().attackFrequency;

		// If no health left, edit stats in Game Manager and destroy current game object
		if (health <= 0)
        {
            level.DecrementEnemiesRemaining();
			stats.SetScore(score);
			Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
		// If the trigger is player and the player can attack, decrease health and reset player attack time
		if (collision.gameObject.CompareTag("PlayerWeapon") && Time.time - lastAttackTime >= playerAttackInterval)
		{
			lastAttackTime = Time.time;
            health -= playerAttackStrength;
        }
    }
}

