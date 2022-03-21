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
    private List<string> attackList;
    public bool isAttacking;

    void Start()
    {
        attackList = new List<string>();
        attackList.Add("Bite");
        attackList.Add("FireBreath");

        isAttacking = false;
    }


    public void RandomAttack()
    {
        string attackSelected = attackList[Random.Range(0, attackList.Count)];
        Attack(attackSelected);
    }


    void Attack(string attackName)
    {
        if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Flying"))
        {
            enemyAnimator.SetTrigger(attackName);
            isAttacking = true;
            float animationLength = enemyAnimator.GetCurrentAnimatorStateInfo(0).length; // TODO A CORRIGER.
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

        isAttacking = false;
    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
