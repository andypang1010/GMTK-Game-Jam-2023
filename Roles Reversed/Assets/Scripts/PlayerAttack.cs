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
    private float lastAttackTime;
    private Collider2D[] targets;

    private void Start()
    {
        W.transform.localScale = new Vector3(attackRadius, attackRadius);
        A.transform.localScale = new Vector3(attackRadius, attackRadius);
        S.transform.localScale = new Vector3(attackRadius, attackRadius);
        D.transform.localScale = new Vector3(attackRadius, attackRadius);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)) {
            lastAttackTime = Time.time;
            GameObject activeCollider = SetActiveCollider();

            // Use LayerMask to filter colliders
            ContactFilter2D enemyFilter = new ContactFilter2D();
            enemyFilter.useLayerMask = true;
            enemyFilter.layerMask = enemyLayerMask;

            // Find all colliders in the active collider 
            Physics2D.OverlapCollider(activeCollider.GetComponent<BoxCollider2D>(), enemyFilter, targets);

            foreach (Collider2D target in targets)
            {
                if (target.TryGetComponent<Health>(out Health health))
                {
                    health.Attacked(attackStrength);
                }
            }
        }

        //target.GetComponent<Health>().Attacked(attackStrength);

        //isAttacking = false;
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
