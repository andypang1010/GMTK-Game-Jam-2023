using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public GameObject sword;
    public int attackStrength = 2;
    public int attackFrequency = 5;
    public float attackAngle = 45;
    public float attackRadius = 1.5f;
    public string opponentTag;
    private bool isAttacking = false;

    private List<GameObject> attackQueue = new List<GameObject>();

    //private void Update()
    //{
    //    Collider2D[] colliders = Physics2D.OverlapCircleAll()
    //}

    // Add opponent to attack queue when entering attack radius
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(opponentTag))
        {
            attackQueue.Add(collision.gameObject);
        }
    }

    // Remove opponent from attack queue when exiting attack radius or becomes dead
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
        if (collision.CompareTag(opponentTag) && !isAttacking)
        {
            StartCoroutine(Attack(attackQueue[0]));
        }
    }

    // Attack animation coroutine
    private IEnumerator Attack(GameObject target)
    {
        isAttacking = true;
        float currentAngle = 0f;

        // Offset weapon angle
        sword.transform.up = target.transform.position - transform.position;
        sword.transform.rotation =
            sword.transform.rotation * Quaternion.Euler(0, 0, attackAngle / 2);

        float attackInterval = 1.0f / attackFrequency;

        // Rotate the weapon while the sword is not at the target angle
        for (float timer = 0; timer < attackInterval; timer += Time.deltaTime)
        {
            float targetAngle = Mathf.Lerp(
                0,
                -attackAngle,
                Mathf.Pow(timer / attackInterval, 1.5f)
            );
            sword.transform.rotation =
                sword.transform.rotation * Quaternion.Euler(0, 0, targetAngle - currentAngle);
            currentAngle = targetAngle;
            yield return null;
        }

        //if (target.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        //{
        //    rb.AddForce(target.transform.position - gameObject.transform.position);
        //}

        target.GetComponent<Health>().Attacked(attackStrength);

        isAttacking = false;
    }
}
