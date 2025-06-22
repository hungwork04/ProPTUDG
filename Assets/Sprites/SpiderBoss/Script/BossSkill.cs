using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill : MonoBehaviour
{
    BossController bossCtrl;
    // Start is called before the first frame update
    public GameObject lazerPrefab;
    public Transform shootPos;
    // Vị trí bắn ra
    public SpriteRenderer laserRenderer;     // Sprite laser
    public float maxLength = 100f;            // Chiều dài tối đa
    public LayerMask hitLayers;              // Lớp vật thể cần va chạm
    public bool isAttack = false;            // Kiểm soát bắn

    public float lazerOffset=3.5f;
    public Vector2 direction;
   // Thêm vào đầu class nếu chưa có
    public float sweepAngle = 60f;         // Tổng góc quét (VD: ±30°)
    public float sweepSpeed = 60f;         // Tốc độ quét (độ/giây)
    private float currentAngle = 0f;
    private bool sweepRight = true;
    public BossMovement bossMovement;
    public bool EndAttack=false;
    void Awake()
    {
        bossCtrl=GetComponent<BossController>();
        bossMovement= GetComponent<BossMovement>();
    }
    public enum Lazertype{
        none,
        type1,
        type2,
        type3
    }
    private Lazertype curlazerType =Lazertype.none;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K)){
           ThrowArcBullet(1);
        }
        // if (Input.GetKeyDown(KeyCode.L))
        // {
        //     DoShootLazerByAngle();
        // }

        if (isAttack && curlazerType==Lazertype.type2)
        {
            UpdateLaser();
        }
        bossCtrl.BossAni.SetBool("IsAttack",isAttack);
        bossCtrl.BossAni.SetBool("EndAtk",EndAttack);
    }

    public void DoShootLazerByAngle(){
        if(bossMovement.isJumping||bossMovement.isMoving||bossMovement.isRotating||EndAttack) return; //
            StartCoroutine(ShootLazerAngle());
    }
    public IEnumerator ShootTenLazer()
    {
        isAttack = true;
        curlazerType=Lazertype.type1;
        for (int i = 0; i < 10; i++)
        {
            ShootLazer();
            yield return new WaitForSeconds(0.9f);
        }
        EndAttack=true;
        yield return new WaitForSeconds(1f);
        EndAttack=false;
        isAttack = false;
        curlazerType=Lazertype.none;
    }
    public void ShootLazer(){
        if(bossCtrl.playerPos==null) return;
        if(shootPos!=null){
            // for(int i=0;i<10;i++){
            Vector2 direction = bossCtrl.playerPos.position - shootPos.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Bù thêm -90 độ nếu sprite đạn mặc định hướng lên
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            GameObject lazer = Instantiate(lazerPrefab, shootPos.position, rotation);
            lazer.GetComponent<BossLazer>().SetVelocity(direction);
            // }
        }
    }
    public IEnumerator ShootLazerAngle(){
        isAttack = true;
        yield return new WaitForSeconds(0.5f);
        laserRenderer.gameObject.SetActive(true);
         curlazerType=Lazertype.type2;
        yield return new WaitForSeconds(7f);
        EndAttack=true;
        yield return new WaitForSeconds(1f);
        isAttack = false;
        laserRenderer.gameObject.SetActive(false);
        EndAttack=false;
        curlazerType=Lazertype.none;
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }
    void UpdateLaser()
    {
        Vector2 origin = shootPos.position;

        float halfSweep = sweepAngle / 2f;

        // Tính góc xoay dần
        if (sweepRight)
        {
            currentAngle += sweepSpeed * Time.deltaTime;
            if (currentAngle >= halfSweep) sweepRight = false;
        }
        else
        {
            currentAngle -= sweepSpeed * Time.deltaTime;
            if (currentAngle <= -halfSweep) sweepRight = true;
        }

        // Xác định Boss đang quay mặt nào
        bool facingRight = GetComponent<BossMovement>().isFacingRight;
        float baseAngle = facingRight ? 0f : 180f; // 👉 0° nếu quay phải, 180° nếu quay trái
        
        // Cộng thêm góc quét hiện tại
        float totalAngle = baseAngle + currentAngle;

        // Tính hướng từ góc
        float angleRad = totalAngle * Mathf.Deg2Rad;
        direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)).normalized;

        // --- Raycast ---
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxLength, hitLayers);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
        }

        float laserLength = hit.collider ? hit.distance : maxLength;
        laserRenderer.flipX = facingRight;
        // 👉 Xoay sprite laser theo góc direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        laserRenderer.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Đặt vị trí laser tại điểm bắt đầu
        laserRenderer.transform.position = origin;

        // Scale theo chiều dài (trục X)
        laserRenderer.transform.localScale = new Vector3(laserLength * lazerOffset, 3, 1);

        // Debug Ray
        Debug.DrawRay(origin, direction * laserLength, Color.red);
    }

    [Header("ThrowSpider")]
    public GameObject SpiderConPrefab;
    public Transform headfirePos;
    public float launchAngle = 60;  // Góc ném cong (độ)
    public float gravity = 9.81f;    // Gravity mặc định Unity
    //public float arcHeightFactor = 1.0f; // Để điều chỉnh độ cao
    public IEnumerator throwSpiderTime(){
        int repeatCount = 4;
        float delay = 0.3f;
        curlazerType=Lazertype.type3;
        for (int i = 0; i < repeatCount; i++)
        {
            ThrowArcBullet(i+0.5f);
            yield return new WaitForSeconds(delay);
        }
        curlazerType=Lazertype.none;
    }
    public void ThrowArcBullet(float h)
    {
        if(bossCtrl.playerPos==null) return;
        Vector2 start = headfirePos.position;
        Vector2 end = bossCtrl.playerPos.position;
        float percentage = 0.7f;
        Vector2 dir = (end - start).normalized;
        float fullDistance = Vector2.Distance(start, end);
        end = start + dir * (fullDistance * percentage);

        float arcHeight = 3f;
        float gravity = 9.81f;

        float travelX = end.x - start.x;
        float travelY = end.y - start.y;

        // Đảm bảo đạn không bị bắn quá gần
        if (Mathf.Abs(travelX) < 0.01f)
            travelX = 0.01f * Mathf.Sign(travelX);

        float height = arcHeight+h;

        float timeToApex = Mathf.Sqrt(2 * height / gravity);
        float descendHeight = height - travelY;
        
        // Nếu descendHeight < 0 thì sqrt sẽ NaN → cần giới hạn tối thiểu
        if (descendHeight < 0.1f)
            descendHeight = 0.1f;

        float timeToDescend = Mathf.Sqrt(2 * descendHeight / gravity);
        float totalTime = timeToApex + timeToDescend;

        if (Mathf.Abs(totalTime) < 0.01f || float.IsNaN(totalTime))
        {
            Debug.LogWarning("Tính toán thời gian không hợp lệ.");
            return;
        }

        float vx = travelX / totalTime;
        float vy = gravity * timeToApex;

        GameObject bullet = Instantiate(SpiderConPrefab, start, Quaternion.identity);
        if(bossMovement.isFacingRight){
            bullet.transform.localScale= new Vector3(-bullet.transform.localScale.x,bullet.transform.localScale.y,bullet.transform.localScale.z);
        }else 
            bullet.transform.localScale= new Vector3(bullet.transform.localScale.x,bullet.transform.localScale.y,bullet.transform.localScale.z);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f;
        rb.velocity = new Vector2(vx, vy);

        SpiderConScript Spidercon = bullet.GetComponent<SpiderConScript>();
        Spidercon.SetTarget(end);
    }
}
