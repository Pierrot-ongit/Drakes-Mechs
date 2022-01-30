using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float speed = 20f;
	public int damage = 40;
	public Rigidbody2D rb;
	public GameObject impactEffect;

	// Use this for initialization
	void Start () {
		if (rb == null) {
			rb = GetComponent<Rigidbody2D>();
		}
		//rb.velocity = transform.right * speed;
		Destroy(gameObject, 5);
	}

	void OnTriggerEnter2D (Collider2D hitInfo)
	{
		Debug.Log("Projectile Collision with " + hitInfo.gameObject);

		Enemy enemy = hitInfo.GetComponent<Enemy>();
		if (enemy != null)
		{
			enemy.TakeDamage(damage);
		}
		Destroy(gameObject);

		//Instantiate(impactEffect, transform.position, transform.rotation);

	}
	
}
