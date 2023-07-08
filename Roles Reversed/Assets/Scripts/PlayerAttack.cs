using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public CircleCollider2D proximityDetector;
    public GameObject sword;
    public float attackFrequency = 5;
    public float attackAngle = 45;

    private bool attacking = false;
    private List<GameObject> attackQueue = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Add enemy to attack queue when entering attack radius
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            attackQueue.Add(collision.gameObject);
        }
    }

    // Remove enemy from attack queue when exiting attack radius or becomes dead
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (attackQueue.Contains(collision.gameObject))
        {
            attackQueue.Remove(collision.gameObject);
        }
    }

    // Attack whenever an enemy is within radius
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !attacking)
        {
            StartCoroutine(Attack(attackQueue[0]));
        }
    }

    // Attack animation coroutine
    private IEnumerator Attack(GameObject target)
    {
        attacking = true;
        float curAngle = 0f;

        // Offset sword angle
        sword.transform.up = target.transform.position - transform.position;
        sword.transform.rotation = sword.transform.rotation * Quaternion.Euler(0, 0, attackAngle/2);

        float attackTime = 1 / attackFrequency;

        for (float timer = 0; timer < attackTime; timer += Time.deltaTime)
        {
            float targetAngle = Mathf.Lerp(0, -attackAngle, Mathf.Pow(timer / attackTime, 1.5f));
            sword.transform.rotation = sword.transform.rotation * Quaternion.Euler(0, 0, targetAngle - curAngle);
            curAngle = targetAngle;
            yield return null;
        }
        attacking = false;
    }
}
