using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float speed = 6f;
	public float lifeTime = 3f;
	public int damage = 1;

	private Vector2 direction;

	public void Initialize(Vector2 dir)
	{
		direction = dir.normalized;
		Destroy(gameObject, lifeTime);
	}

	void Update()
	{
		transform.Translate(direction * speed * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			// Gây sát thương nếu có script Health/PlayerManager
			// other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
			Destroy(gameObject);
		}
		else if (!other.CompareTag("Enemy"))
		{
			Destroy(gameObject);
		}
	}
}
