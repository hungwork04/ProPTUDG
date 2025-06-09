using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
	[Header("Attack Parameters")]
	[SerializeField] private float attackCooldown = 1f;
	[SerializeField] private float range = 5f;
	[SerializeField] private int damage = 1;

	[Header("Ranged Attack")]
	[SerializeField] private Transform firepoint;
	[SerializeField] private GameObject[] fireballs;

	[Header("Collider Parameters")]
	[SerializeField] private float colliderDistance = 0.3f;
	[SerializeField] private BoxCollider2D boxCollider;

	[Header("Player Layer")]
	[SerializeField] private LayerMask playerLayer;

	private float cooldownTimer = Mathf.Infinity;

	// References
	private Animator anim;
	private EnemyPatrol enemyPatrol;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		enemyPatrol = GetComponentInParent<EnemyPatrol>();
	}

	private void Update()
	{
		cooldownTimer += Time.deltaTime;

		if (PlayerInSight())
		{
			if (cooldownTimer >= attackCooldown)
			{
				cooldownTimer = 0;
				anim.SetTrigger("rangedAttack");
			}
		}

		if (enemyPatrol != null)
			enemyPatrol.enabled = !PlayerInSight();
	}

	// Gọi từ Animation Event
	private void RangedAttack()
	{
		int i = FindFireball();
		if (i < 0)
		{
			Debug.LogWarning("⚠️ Không có viên đạn nào khả dụng.");
			return;
		}

		GameObject fireball = fireballs[i];
		fireball.transform.position = firepoint.position;

		float direction = enemyPatrol != null ? Mathf.Sign(enemyPatrol.GetFacingDirection()) : 1f;
		fireball.GetComponent<EnemyProjectile>().ActivateProjectile(direction);
	}

	private int FindFireball()
	{
		for (int i = 0; i < fireballs.Length; i++)
		{
			if (!fireballs[i].activeInHierarchy)
				return i;
		}
		return -1;
	}

	private bool PlayerInSight()
	{
		RaycastHit2D hit = Physics2D.BoxCast(
			boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
			new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
			0, Vector2.zero, 0, playerLayer);

		return hit.collider != null;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(
			boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
			new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
	}
}
