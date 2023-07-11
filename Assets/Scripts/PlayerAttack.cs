using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerAttack : MonoBehaviour
{
    public GameObject W, A, S, D;

    public GameObject weapon;
    public int attackStrength, attackFrequency;
    public float attackRadius;
    public LayerMask enemyLayerMask;

    private bool isAttacking = false;
    private Collider2D[] targets = new Collider2D[100];

    private void Start()
    {
        W.SetActive(false);
        A.SetActive(false);
        S.SetActive(false);
        D.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)) {
            GameObject activeCollider = SetActiveCollider();
            activeCollider.GetComponentInChildren<BoxCollider2D>().size = new Vector2(attackRadius, attackRadius);

            // Use LayerMask to filter colliders
            ContactFilter2D enemyFilter = new ContactFilter2D
            {
                useLayerMask = true,
                layerMask = enemyLayerMask
            };

            // Find all colliders in the active collider 
            Physics2D.OverlapCollider(activeCollider.GetComponentInChildren<BoxCollider2D>(), enemyFilter, targets);

            AttackInDirection();
        }
    }

    // Attack animation coroutine
    private IEnumerator Attack(Vector3 attackDir)
    {
        isAttacking = true;
        float currentAngle = 0f;
        float attackAngle = 180;

        // Offset weapon angle
        weapon.transform.up = attackDir;
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

        isAttacking = false;

        foreach (Collider2D target in targets)
        {
            if (target.gameObject.TryGetComponent(out Health health))
            {
                health.Attacked(attackStrength);
            }
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

    // Set the active collider using W,A,S,D
    private GameObject SetActiveCollider() {
        W.SetActive(false);
        A.SetActive(false);
        S.SetActive(false);
        D.SetActive(false);

        if (Input.GetKey(KeyCode.W))
        {
            W.SetActive(true);
            return W;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            A.SetActive(true);
            return A;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            S.SetActive(true);
            return S;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            D.SetActive(true);
        }
        return D;
    }
}
