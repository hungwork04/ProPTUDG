using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
	[Header("Patrol Points")]
	[SerializeField] private Transform leftEdge;
	[SerializeField] private Transform rightEdge;

	[Header("Enemy")]
	[SerializeField] private Transform enemy; // GameObject con chứa sprite enemy

	[Header("Movement Parameters")]
	[SerializeField] private float speed = 2f;
	private Vector3 initScale;
	private bool movingLeft;

	[Header("Idle Behaviour")]
	[SerializeField] private float idleDuration = 1f;
	private float idleTimer;

	[Header("Enemy Animator")]
	[SerializeField] private Animator anim;

	private int facingDirection = 1; // 1 = phải, -1 = trái

	private void Awake()
	{
		initScale = enemy.localScale;
	}

	private void OnDisable()
	{
		if (anim != null)
			anim.SetBool("moving", false);
	}

	private void Update()
	{
		if (movingLeft)
		{
			if (enemy.position.x >= leftEdge.position.x)
				MoveInDirection(-1);
			else
				DirectionChange();
		}
		else
		{
			if (enemy.position.x <= rightEdge.position.x)
				MoveInDirection(1);
			else
				DirectionChange();
		}
	}

	private void DirectionChange()
	{
		if (anim != null)
			anim.SetBool("moving", false);

		idleTimer += Time.deltaTime;
		if (idleTimer > idleDuration)
		{
			movingLeft = !movingLeft;
			idleTimer = 0;
		}
	}

	private void MoveInDirection(int _direction)
	{
		idleTimer = 0;

		if (anim != null)
			anim.SetBool("moving", true);

		// Cập nhật hướng hiện tại
		facingDirection = _direction;

		// Lật sprite enemy
		enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);

		// Di chuyển enemy
		enemy.position = new Vector3(
			enemy.position.x + Time.deltaTime * _direction * speed,
			enemy.position.y,
			enemy.position.z
		);
	}

	// Phương thức cho script khác gọi để biết enemy đang quay trái hay phải
	public int GetFacingDirection()
	{
		return facingDirection;
	}
}
