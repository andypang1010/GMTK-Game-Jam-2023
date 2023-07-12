using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int attackStrength, attackFrequency;
    public float attackRadius;
    public LayerMask enemyLayerMask;

    private bool isAttacking = false;
    private GameObject parent;
    private List<GameObject> targets = new();

    private void Start()
    {
        parent = transform.parent.gameObject;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)) {
            AttackInDirection();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player is attacking, the collider is an enemy, and the enemy is not already a target
        if (isAttacking && CompareLayer(enemyLayerMask, collision) && !targets.Contains(collision.gameObject)) {
            targets.Add(collision.gameObject);
        }
    }

    private void AttackInDirection()
    {
        Vector3 dir;
        if (Input.GetKey(KeyCode.W))
        {
            dir = Vector3.up;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            dir = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir = Vector3.down;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir = Vector3.right;
        }
        else
        {
            return;
        }
        if (Input.GetKey(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(Attack(dir));
        }
    }

    // Attack animation coroutine
    private IEnumerator Attack(Vector3 attackDir)
    {
        float currentAngle = 0f;
        float attackAngle = 180;

        // Offset weapon angle
        parent.transform.up = attackDir;
        parent.transform.rotation =
            parent.transform.rotation * Quaternion.Euler(0, 0, attackAngle / 2);

        float attackInterval = 1.0f / attackFrequency;
        isAttacking = true;

        // Rotate the weapon while the sword is not at the target angle
        for (float timer = 0; timer < attackInterval; timer += Time.deltaTime)
        {
            float targetAngle = Mathf.Lerp(
                0,
                -attackAngle,
                Mathf.Pow(timer / attackInterval, 1.5f)
            );
            parent.transform.rotation =
                parent.transform.rotation * Quaternion.Euler(0, 0, targetAngle - currentAngle);
            currentAngle = targetAngle;
            yield return null;
        }


        foreach (GameObject target in targets)
        {
            if (target.TryGetComponent(out Health health))
            {
                health.Attacked(attackStrength);
            }
        }

        targets.Clear();
        isAttacking = false;
    }


    private bool CompareLayer(LayerMask layer, Collider2D collider)
    {
        return (layer & 1 << collider.gameObject.layer) == 1 << collider.gameObject.layer;
    }

}
