using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject weapon;
    public int attackStrength;
    public float attackFrequency;
    public float attackAngle;
    public LayerMask targetLayer;

    private bool isAttacking = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Attack whenever our weapon collides with the player
        if (CompareLayer(targetLayer, collision) && !isAttacking)
        {
            StartCoroutine(Attack(collision.gameObject));
        }
    }

    // Attack animation coroutine
    private IEnumerator Attack(GameObject player)
    {
        isAttacking = true;
        float currentAngle = 0f;

        // Offset weapon angle
        weapon.transform.up = player.transform.position - transform.position;
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

        player.GetComponent<Health>().Attacked(attackStrength);

        isAttacking = false;
    }

    private bool CompareLayer(LayerMask layer, Collider2D collider) {
        return (layer & 1 << collider.gameObject.layer) == 1 << collider.gameObject.layer;
    }
}