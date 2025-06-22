using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    public float maxHealth=200f;
    public float currentHealth;
    // Start is called before the first frame update
    BossMovement bossMovement;
    BossAI bossAI;
    void Awake()
    {
        bossMovement=GetComponent<BossMovement>();
        bossAI=GetComponent<BossAI>();
    }
    public bool isDead=false;
    [SerializeField] private Image healthBar; // Kéo HealthFill vào đây
    void Start()
    {
        currentHealth=maxHealth;
    }
    void Update()
    {
        if(currentHealth<=0&&isDead==false){
            isDead=true;
            Debug.Log("BossDie");
            bossMovement.enabled=false;
            bossAI.enabled=false;
            GetComponent<Animator>().Play("Spider_Death");
        }
        

    }
    public void takeDame(float dame){
        currentHealth-=dame;
        UpdateHealthUI();
        //UpdateUI
    }
    private void UpdateHealthUI()
	{
		if (healthBar != null)
			healthBar.fillAmount = currentHealth / maxHealth;
    }
}
