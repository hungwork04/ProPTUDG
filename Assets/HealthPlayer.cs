using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthPlayer : MonoBehaviour
{
	[Header("Health")]
	[SerializeField] private float startingHealth = 5;
	public float currentHealth { get; private set; }
	//private Animator anim;
	private bool dead;

	[Header("iFrames")]
	[SerializeField] private float iFramesDuration = 0.5f;
	[SerializeField] private int numberOfFlashes = 3;
	//private SpriteRenderer spriteRend;
	public SpriteRenderer[] spriteRend;


	[Header("Components")]
	[SerializeField] private Behaviour[] components;
	private bool invulnerable;

	[Header("UI")]
	[SerializeField] private Image healthBar; // Kéo HealthFill vào đây

	private void Awake()
	{
		currentHealth = startingHealth;
		//anim = GetComponent<Animator>();
		//spriteRend = GetComponents<SpriteRenderer>();
		UpdateHealthUI();
	}

	public void TakeDamage(float _damage)
	{
		if (invulnerable || dead) return;

		currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
		UpdateHealthUI();

		if (currentHealth > 0)
		{
			//anim.SetTrigger("hurt");
			StartCoroutine(Invunerability());
		}
		else
		{
			if (!dead)
			{
				dead = true;
				//anim.SetTrigger("die");
				Debug.Log("🛑 Player chết");

				foreach (Behaviour component in components)
					component.enabled = false;

				Invoke(nameof(Disappear), 1.5f);
			}
		}
	}

	public void AddHealth(float _value)
	{
		currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
		UpdateHealthUI();
	}

	private void UpdateHealthUI()
	{
		if (healthBar != null)
			healthBar.fillAmount = currentHealth / startingHealth;
	}

	private IEnumerator Invunerability()
	{
		invulnerable = true;
		Physics2D.IgnoreLayerCollision(10, 11, true); // Cập nhật layer nếu cần

		for (int i = 0; i < numberOfFlashes; i++)
		{
			foreach(var item in spriteRend){
				item.color = new Color(1, 0, 0, 0.5f);
				yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
			}
			foreach(var item in spriteRend){
				item.color = Color.white;
				yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
			}
		}

		Physics2D.IgnoreLayerCollision(10, 11, false);
		invulnerable = false;
	}

	private void Disappear()
	{
		Debug.Log("🧍 Player biến mất khỏi scene");
		Destroy(gameObject);
	}
}
