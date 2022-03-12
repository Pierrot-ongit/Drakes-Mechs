using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    [SerializeField] int damage = 40;
    [SerializeField] float tickRate = 2f;
    [SerializeField] bool damageOverTime = true;
    [SerializeField] private LayerMask damageableLayers;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer == damageableLayers)
        {
            if (damageOverTime)
            {
                collision.gameObject.GetComponent<HealthManager>().TakeDamage(damage);
                StartCoroutine(DelayDamage(tickRate));
            }
            else
            {
                collision.gameObject.GetComponent<HealthManager>().TakeDamage(damage);
            }
        }
    }

    IEnumerator DelayDamage(float _delay = 0)
    {
        yield return new WaitForSeconds(_delay);
    }
}
