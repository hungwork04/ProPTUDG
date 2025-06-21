using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
	[SerializeField] protected float damage;

	protected virtual void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			HealthPlayer hp = collision.GetComponent<HealthPlayer>();
			if (hp != null)
			{
				hp.TakeDamage(damage);
				Debug.Log("💥 Player bị trúng đạn! Gây sát thương: " + damage);
			}
			else
			{
				Debug.LogWarning("⚠️ Không tìm thấy HealthPlayer trên Player!");
			}
		}
	}
}
