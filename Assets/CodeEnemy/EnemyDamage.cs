﻿using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
	[SerializeField] protected float damage;

	protected virtual void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log(collision.gameObject.name);
		if (collision.CompareTag("Player"))
		{
			PlayerCharacterMovement hp = collision.GetComponent<PlayerCharacterMovement>();
			HealthPlayer hpPlayer2=collision.GetComponent<HealthPlayer>();
			if (hp != null)
			{
				hp.TakeDamage(damage);
				Debug.Log("💥 Player bị trúng đạn! Gây sát thương: " + damage);
			}else if(hpPlayer2!=null){
				hpPlayer2.TakeDamage(damage);
				Debug.Log("💥 Player bị trúng đạn! Gây sát thương: " + damage);
			}
			else
			{
				Debug.LogWarning("⚠️ Không tìm thấy HealthPlayer trên Player!");
			}
		}
	}
}
