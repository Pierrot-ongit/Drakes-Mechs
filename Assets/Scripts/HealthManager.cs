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
    private bool isDead;

    private void Awake()
    {
        flashEffect = GetComponent<ColoredFlash>();
        gameManager = FindObjectOfType<GameManager>();
        isDead = false;

    }

    void Start()
    {
        currentHealth = maxHealth;
       // healthBar.SetMaxHealth();
    }

    public void TakeDamage(int damage)
    {

        if (isDead)
        {
            return;
        }

        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            isDead = true;
            Death();
            return;
        }


        if (flashEffect != null)
        {
            flashEffect.Flash("damage");
        }
        if (EnemyAIScript != null)
        {
            EnemyAIScript.setChasingTarget();
        }

    }

    // ENCAPSULATION
    public bool IsDead()
    {
        return isDead;
    }

    public void Death()
    {

        healthBar.gameObject.SetActive(false);
        if (shaderGraphScript != null)
        {
            shaderGraphScript.startDissolving();
        }
        Invoke("handleEndGame", deathDelay);
    }

    private void handleEndGame()
    {
        if (gameObject.name == "Character")
        {
            gameManager.GameOver();
        }
        else
        {
            gameManager.Victory();
        }
        Destroy(gameObject);
    }

    private IEnumerator DeathRoutine()
    {
        if (shaderGraphScript != null)
        {
            shaderGraphScript.startDissolving();
            Debug.Log("test");
            yield return new WaitForSeconds(deathDelay);
            Destroy(gameObject);
            if (gameObject.name == "Character")
            {
                gameManager.GameOver();
            }
            else
            {
                gameManager.Victory();
            }
        }
    }
}
