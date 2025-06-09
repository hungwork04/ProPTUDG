using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
	[Header("Health")]
	[SerializeField] private float startingHealth = 5;
	public float currentHealth { get; private set; }
	private Animator anim;
	private bool dead;

	[Header("iFrames")]
	[SerializeField] private float iFramesDuration = 0.5f;
	[SerializeField] private int numberOfFlashes = 3;
	private SpriteRenderer spriteRend;

	[Header("Components")]
	[SerializeField] private Behaviour[] components;
	private bool invulnerable;

	private void Awake()
	{
		currentHealth = startingHealth;
		anim = GetComponent<Animator>();
		spriteRend = GetComponent<SpriteRenderer>();
	}

	public void TakeDamage(float _damage)
	{
		if (invulnerable || dead) return;

		currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

		if (currentHealth > 0)
		{
			anim.SetTrigger("hurt");
			StartCoroutine(Invunerability());
		}
		else
		{
			if (!dead)
			{
				dead = true;
				anim.SetTrigger("die");
				Debug.Log("Enemy chết - đang chuẩn bị biến mất");

				// Vô hiệu hoá các component (di chuyển, tấn công,...)
				foreach (Behaviour component in components)
					component.enabled = false;

				// Gọi hàm biến mất sau khi animation die phát xong
				Invoke(nameof(Disappear), 1.5f); // Điều chỉnh thời gian nếu animation dài hơn
			}
		}
	}

	public void AddHealth(float _value)
	{
		currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
	}

	private IEnumerator Invunerability()
	{
		invulnerable = true;
		Physics2D.IgnoreLayerCollision(10, 11, true); // Tuỳ vào layer của enemy/player

		for (int i = 0; i < numberOfFlashes; i++)
		{
			spriteRend.color = new Color(1, 0, 0, 0.5f);
			yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
			spriteRend.color = Color.white;
			yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
		}

		Physics2D.IgnoreLayerCollision(10, 11, false);
		invulnerable = false;
	}

	private void Disappear()
	{
		Debug.Log("Enemy đã biến mất khỏi scene.");
		// gameObject.SetActive(false); // Hoặc:
									 Destroy(gameObject);
	}
}
