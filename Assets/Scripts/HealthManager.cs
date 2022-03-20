using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;
    private ColoredFlash flashEffect;

    public HealthBar healthBar;
    [SerializeField] private float deathDelay = 5f;
    [SerializeField] private ShaderGraphUnit shaderGraphScript;
    [SerializeField] private EnemyAI EnemyAIScript;
    private GameManager gameManager;

    private void Awake()
    {
        flashEffect = GetComponent<ColoredFlash>();
        gameManager = FindObjectOfType<GameManager>();

    }

    void Start()
    {
        currentHealth = maxHealth;
       // healthBar.SetMaxHealth();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (flashEffect != null)
        {
            flashEffect.Flash("damage");
        }
        if (EnemyAIScript != null)
        {
            EnemyAIScript.setChasingTarget();
        }


        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        
        var deathRoutine = StartCoroutine(DeathRoutine());
       // Invoke("Death", 2f);
        if (gameObject.name == "Character")
        {
            gameManager.GameOver();
        }
        else
        {
            gameManager.Victory();
        }
    }

    private IEnumerator DeathRoutine()
    {
        if (shaderGraphScript != null)
        {
            shaderGraphScript.startDissolving();
            yield return new WaitForSeconds(deathDelay);
            Destroy(gameObject);
        }
    }
}
