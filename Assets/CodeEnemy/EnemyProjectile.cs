using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
	[SerializeField] private float speed = 5f;
	[SerializeField] private float resetTime = 3f;

	private float lifetime;
	private Animator anim;
	private BoxCollider2D coll;
	private bool hit;
	private float direction = 1f;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		coll = GetComponent<BoxCollider2D>();
	}

	public void ActivateProjectile(float dir)
	{
		hit = false;
		lifetime = 0;
		gameObject.SetActive(true);
		coll.enabled = true;

		direction = dir;

		// ✅ Flip sprite đúng hướng bay
		float scaleX = Mathf.Abs(transform.localScale.x) * Mathf.Sign(direction);
		transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);

		Debug.Log("🚀 Projectile kích hoạt! Hướng: " + direction);
	}

	private void Update()
	{
		if (hit)
		{
			return;
		}

		// ✅ Di chuyển theo hướng
		transform.Translate(Vector3.right * direction * speed * Time.deltaTime, Space.World);

		lifetime += Time.deltaTime;
		if (lifetime > resetTime)
		{
			gameObject.SetActive(false);
		}
	}

	protected override void OnTriggerEnter2D(Collider2D collision)
	{
		if (hit) return;
		hit = true;
		base.OnTriggerEnter2D(collision);
		coll.enabled = false;

		if (anim != null)
			anim.SetTrigger("explode");
		else
			gameObject.SetActive(false);
	}

	// Gọi từ Animation `explode` nếu có
	private void Deactivate()
	{
		gameObject.SetActive(false);
	}
}
