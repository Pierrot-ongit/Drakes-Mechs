using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private float attackRange;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerLayers;
    [SerializeField] private GameObject fireBreathEffect;
    private GameObject areaDamageActivated;
    private ParticleSystem particleSystemActivated;
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
            float animationLength = enemyAnimator.GetCurrentAnimatorStateInfo(0).length; // TODO A CORRIGER.
            Debug.Log(enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("FireBreath"));
            Debug.Log(enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Flying"));
            Debug.Log(enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            Debug.Log(animationLength);
            switch (attackName)
            {
                case "Bite":
                    // AttackInAnimation se charge d'appliquer les degats au bon moment de l'animation.
                    StartCoroutine(DelayAnimation(animationLength));
                    break;
                case "FireBreath":
                    if (fireBreathEffect != null)
                    {
                        particleSystemActivated = fireBreathEffect.GetComponent<ParticleSystem>();
                        particleSystemActivated.Play();
                        areaDamageActivated = fireBreathEffect.transform.Find("AreaDamage").gameObject;
                        if (areaDamageActivated != null)
                        {
                            // Un script AreaDamage dans ce GameObject se charge d'infler les degats.
                            areaDamageActivated.SetActive(true);
                        }
                    }
                    break;
            }
        }
    }

    IEnumerator DelayAnimation(float _delay = 0)
    {
        yield return new WaitForSeconds(_delay);
    }

    private void AttackInAnimation(int damage)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
        foreach (Collider2D hit in hits)
        {
            hit.gameObject.GetComponent<HealthManager>().TakeDamage(damage);
        }
    }

    private void StopAttackInAnimation()
    {
        if (particleSystemActivated != null)
        {
            particleSystemActivated.Stop();
        }
        if (areaDamageActivated != null)
        {
            areaDamageActivated.SetActive(false);
        }

    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
