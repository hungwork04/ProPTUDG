using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] private float speed = 5f;
	private float direction;
	private bool hit;
	private float lifetime;

	private Animator anim;
	[SerializeField]private BoxCollider2D boxCollider;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		boxCollider = GetComponent<BoxCollider2D>();
	}

	private void Update()
	{
		if (hit) return;

		float movementSpeed = speed * Time.deltaTime * direction;
		transform.Translate(movementSpeed, 0f, 0f, Space.World);

		lifetime += Time.deltaTime;
		if (lifetime > 5f)
			gameObject.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (hit) return;

		hit = true;
		boxCollider.enabled = false;
		anim.SetTrigger("explode");

		if (collision.CompareTag("Enemy"))
		{
			Health enemyHealth = collision.GetComponent<Health>();
			if (enemyHealth != null)
				enemyHealth.TakeDamage(1);
		}
	}

	public void SetDirection(float _direction)
	{
		hit = false;
		lifetime = 0f;
		direction = _direction;

		gameObject.SetActive(true);
		boxCollider.enabled = true;

		// Flip sprite theo hướng bắn
		float localScaleX = Mathf.Abs(transform.localScale.x) * Mathf.Sign(_direction);
		transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
	}

	private void Deactivate()
	{
		gameObject.SetActive(false);
	}
}
