using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	[SerializeField] float speed = 20f;
	[SerializeField] int damage = 40;
	[SerializeField] float lifespan = 5f;
	[SerializeField] GameObject impactEffect;
	[SerializeField] bool damageEnemy = true; // Damage the player or the enemy.

	private Rigidbody2D rb;
	private Vector3 shootDir;

	public void Setup(Vector3 shootDir)
    {
		this.shootDir = shootDir;
		transform.eulerAngles = new Vector3(0, 0 , GetAngleFromVectorFloat(shootDir));
    }

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		Destroy(gameObject, lifespan);
	}

    private void Update()
    {
        transform.position += shootDir * speed * Time.deltaTime;
    }


     void OnCollisionEnter2D(Collision2D collision)
    {
		if (damageEnemy)
        {
			if (collision.gameObject.CompareTag("Enemy"))
			{
				collision.gameObject.GetComponent<HealthManager>().TakeDamage(damage);
			}
		}
		else
        {
			if (collision.gameObject.CompareTag("Player"))
			{
				collision.gameObject.GetComponent<HealthManager>().TakeDamage(damage);
			}
		}
		// Impact againt something else than a projectile. Destroy the projectile.
		if (!collision.gameObject.CompareTag("ProjectilePlayer") && !collision.gameObject.CompareTag("ProjectileEnemy"))
        {
			Destroy(gameObject);
        }

	}

	// ABSTRACTION
	private float GetAngleFromVectorFloat(Vector3 dir)
	{
		dir = dir.normalized;
		float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		if (n < 0) n += 360;

		return n;
	}

}
