using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderConScript : MonoBehaviour
{
    private Vector2 target;
    private bool hasActivated = false;
    public float triggerDistance = 0.3f; // khoảng cách kích hoạt
    public bool canMove=false;
    public Rigidbody2D rb;
    public float moveInput;
    void Awake()
    {
        if(rb==null)
        rb=GetComponent<Rigidbody2D>();
    }
    public void SetTarget(Vector2 end)
    {
        target = end;
    }
    void Start()
    {
        moveInput=transform.localScale.x;
        StartCoroutine(SelfDestruct());
    }
    void Update()
    {
        if (!hasActivated && Vector2.Distance(transform.position, target) < triggerDistance)
        {
            hasActivated = true;
            Active(); // Gọi hàm khi đến nơi
        }
        if(canMove){
            rb.velocity = new Vector2(-moveInput * 15, rb.velocity.y);
        }
    }
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(5f);

        // (Tùy chọn) Thêm hiệu ứng nổ, âm thanh trước khi hủy
        // Ví dụ: Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
    void Active()
    {
        Debug.Log("Bullet đã tới đích và thực hiện Active()");
        GetComponent<Collider2D>().isTrigger=false;
        canMove=true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player"){
            StopAllCoroutines();
            Destroy(gameObject);
            collision.GetComponent<HealthPlayer>().TakeDamage(2);
            Debug.Log("-2");
        }
    }
}
