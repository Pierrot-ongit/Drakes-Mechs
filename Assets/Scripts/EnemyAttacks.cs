using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private float attackRange;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerLayers;
  //  [SerializeField] private Animator enemyAnimator;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack("FireBreath");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Attack("Bite");
        }
    }


    void Attack(string attackName)
    {
        if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Flying"))
        {
            Debug.Log(attackName);
            enemyAnimator.SetTrigger(attackName);
            switch (attackName)
            {
                case "Bite":
                    Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
                    foreach (Collider2D hit in hits)
                    {
                        hit.gameObject.GetComponent<HealthManager>().TakeDamage(10);
                    }
                    break;
                case "FireBreath":
                    // TODO AREA DAMAGE.
                    break;
            }
            
            StartCoroutine(DelayAnimation(enemyAnimator.GetCurrentAnimatorStateInfo(0).length));

        }
    }

    void FireBreath()
    {
      //  if (enemyAnimator.GetCurrentAnimatorStateInfo(0).)
        enemyAnimator.SetTrigger("FireBreath");
        Debug.Log("FireBreath");
        StartCoroutine(DelayAnimation(enemyAnimator.GetCurrentAnimatorStateInfo(0).length));
    }


    IEnumerator DelayAnimation(float _delay = 0)
    {
        yield return new WaitForSeconds(_delay);
    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
