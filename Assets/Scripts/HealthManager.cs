using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    private ColoredFlash flashEffect;

    public HealthBar healthBar;

    private void Awake()
    {
        flashEffect = GetComponent<ColoredFlash>();
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (flashEffect != null)
        {
            flashEffect.Flash("damage");
        }

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        Destroy(gameObject);
        // TODO EFFECT DEATH.
    }
}
