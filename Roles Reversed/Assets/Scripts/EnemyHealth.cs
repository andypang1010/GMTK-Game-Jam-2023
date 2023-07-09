using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public GameObject player;

    //public GameObject proximityDetector;

    public int health = 4;
    public int maxHealth = 4;
    public int score = 5;
    public Collider2D bodyCollider;
    public MeleeAttack playerAttack;

    private float playerAttackInterval;
    private float lastAttackTime;

    void Start()
    {
        health = maxHealth;
        lastAttackTime = Time.time;
    }

    void Update()
    {
        playerAttackInterval = 1.0f / playerAttack.attackFrequency;

        // If no health left, edit stats in Game Manager and destroy current game object
        if (health <= 0)
        {
            LevelManager.Instance.DecrementEnemiesRemaining();
            StatsManager.Instance.SetScore(score);
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        // If the trigger is player and the player can attack, decrease health and reset player attack time
        if (
            collision.gameObject.CompareTag("PlayerWeapon")
            && Time.time - lastAttackTime >= playerAttackInterval
        )
        {

            MeleeAttack playerAttack = collidedObject.GetComponentInParent<MeleeAttack>();

            //playerAttack

            lastAttackTime = Time.time;
            health -= playerAttack.attackStrength;
        }
    }
}
