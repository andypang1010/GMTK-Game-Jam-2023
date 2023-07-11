using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject weapon;
    public int attackStrength;
    public float attackFrequency;
    public float attackAngle;
    public LayerMask playerLayer;

    private bool isAttacking = false;
    private List<GameObject> attackQueue = new List<GameObject>();

    // Add opponent to attack queue when entering attack radius
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CompareLayer(playerLayer, collision)) {
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
        if (CompareLayer(playerLayer, collision) && !isAttacking)
        {
            //print(attackQueue[0].ToString());
            StartCoroutine(Attack(attackQueue[0]));
        }
    }

    // Attack animation coroutine
    private IEnumerator Attack(GameObject target)
    {
        //print(target.ToString());
        isAttacking = true;
        float currentAngle = 0f;

        // Offset weapon angle
        weapon.transform.up = target.transform.position - transform.position;
        weapon.transform.rotation =
            weapon.transform.rotation * Quaternion.Euler(0, 0, attackAngle / 2);

        float attackInterval = 1.0f / attackFrequency;

        // Rotate the weapon while the sword is not at the target angle
        for (float timer = 0; timer < attackInterval; timer += Time.deltaTime)
        {
            float targetAngle = Mathf.Lerp(
                0,
                -attackAngle,
                Mathf.Pow(timer / attackInterval, 1.5f)
            );
            weapon.transform.rotation =
                weapon.transform.rotation * Quaternion.Euler(0, 0, targetAngle - currentAngle);
            currentAngle = targetAngle;
            yield return null;
        }

        target.GetComponent<Health>().Attacked(attackStrength);

        isAttacking = false;
    }

    private bool CompareLayer(LayerMask layer, Collider2D collider) {
        return (layer & 1 << collider.gameObject.layer) == 1 << collider.gameObject.layer;
    }
}