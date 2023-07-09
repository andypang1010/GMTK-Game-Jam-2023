using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject W, A, S, D;

    public GameObject weapon;
    public int attackStrength, attackFrequency;
    public float attackRadius;
    public string opponentTag;

    private bool isAttacking = false;
    private Collider2D[] W_Colliders, A_Colliders, S_Colliders, D_Colliders;
    private List<GameObject> attackQueue = new List<GameObject>();

    private void Update()
    {
        W.transform.localScale = new Vector3(attackRadius, attackRadius);
        A.transform.localScale = new Vector3(attackRadius, attackRadius);
        S.transform.localScale = new Vector3(attackRadius, attackRadius);
        D.transform.localScale = new Vector3(attackRadius, attackRadius);

        //W_Colliders = Physics2D.OverlapBoxAll()

        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.GetComponentInParent<Transform>().transform.position, attackRadius);
    }

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

        //if (target.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        //{
        //    rb.AddForce(target.transform.position - gameObject.transform.position);
        //}

        //if (opponentTag == "Player")
        //{
        //    target.GetComponent<PlayerHealth>().Attacked(attackStrength);
        //}
        //else if (opponentTag == "Enemy")
        //{
        //    target.GetComponent<PlayerHealth>().Attacked(attackStrength);
        //}

        target.GetComponent<Health>().Attacked(attackStrength);

        isAttacking = false;
    }
}
