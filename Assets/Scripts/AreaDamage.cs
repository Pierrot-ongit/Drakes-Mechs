using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    [SerializeField] int damage = 40;
    [SerializeField] float tickRate = 2f;
    float timestamp = 0f;
    [SerializeField] bool damageOverTime = true;
    [SerializeField] private LayerMask damageableLayers;


    private void OnTriggerStay2D(Collider2D collision)
    {

        if (!damageOverTime)
            return;

        if ((damageableLayers & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            if (Time.time > timestamp + tickRate)
            {
                collision.gameObject.GetComponent<HealthManager>().TakeDamage(damage);
                timestamp = Time.time;
            }
        }
    }

}
