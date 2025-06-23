using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLazer : ShooterBullet
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(collision.gameObject.CompareTag("Player")){
                if(collision.GetComponent<HealthPlayer>()!=null)
                    collision.GetComponent<HealthPlayer>().TakeDamage(2);
                if(collision.GetComponent<PlayerCharacterMovement>()!=null){
                    collision.GetComponent<PlayerCharacterMovement>().TakeDamage(2);
                }
                Debug.Log("-1");
            }
            Destroy(gameObject);
            Debug.Log(gameObject.name);
        }
    }
}
