using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    BossController bossCtrl;
    [Header("Jump Ref")]
    public float jumpDuration = 0.5f;
    public AnimationCurve jumpCurve;
    public bool isJumping = false;
	public bool isPreJump=false;

    [Header("Move Ref")]
    public float moveSpeed=10f;
    public bool isFacingRight=false;
    public float maxDistance=20f;
    public bool isMoving=false;
    public bool isRotating=false;
    public bool isHitted=false;
    public BossSkill bossSkill;
    void Awake()
    {
        bossCtrl=GetComponent<BossController>();
        bossSkill=GetComponent<BossSkill>();
    }
    public float movingTimer;
    public float RotateTimer;

    private void Update()
    {
        //Kiểm tra bên trái phải
        LookAtPlayer();
        bossCtrl.BossAni.SetBool("IsMoving",isMoving);
        if(isMoving){
            Physics2D.IgnoreLayerCollision(6, 10, true);
            bossCtrl.BossRb.gravityScale=0;
        }else{
            
            Physics2D.IgnoreLayerCollision(6, 10, false);
            bossCtrl.BossRb.gravityScale=4;

        }
        if (isMoving)
        {
            movingTimer += Time.deltaTime;

            if (movingTimer >= 5)
            {
                isMoving = false;
                movingTimer = 0f;
                // bossCtrl.BossAni.Play("Spider_Idle");
                Debug.Log("Tự động tắt isMoving sau 5s");
            }
        }
        else
        {
            // Nếu không di chuyển thì reset timer luôn
            movingTimer = 0f;
        }


        if (isRotating)
        {
            RotateTimer += Time.deltaTime;

            if (RotateTimer >= 3)
            {
                // isJumping = false;
                // isPreJump=false;
                isRotating=false;
                RotateTimer = 0f;
                bossCtrl.BossAni.Play("Spider_Idle");
                Debug.Log("Tự động tắt rotate sau 5s");
            }
        }
        else
        {
            // Nếu không di chuyển thì reset timer luôn
            RotateTimer = 0f;
        }
    }
    public void makeHitted(){
        if(isRotating || isJumping ||bossSkill.isAttack||bossSkill.EndAttack||isHitted){
            return;
        }
        bossCtrl.BossAni.SetTrigger("Hitted");
        isHitted=true;
    }

    public void LookAtPlayer(){
        //Chỉ xoay khi k tấn công hay nhảy
        if(isJumping||isMoving||bossSkill.isAttack||bossCtrl.playerPos==null) return; //
        if (bossCtrl.playerPos.position.x < this.transform.position.x && isFacingRight)
        {
            // Player đang ở bên trái boss
            Flip();
        }
        else if(bossCtrl.playerPos.position.x > this.transform.position.x && !isFacingRight)
        {
            // Player đang ở bên phải boss
            Flip();
        }
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }
    // Hàm gọi cú nhảy tới vị trí được truyền vào

    public void StartMoveToPlayer()
    {
        if(bossCtrl.playerPos==null) return;
            isMoving = true;
            Debug.Log("start");
            Vector2 dir = (bossCtrl.playerPos.position - bossCtrl.BossRb.transform.position).normalized;
            Vector2 targetPos = (Vector2)bossCtrl.playerPos.position - dir * maxDistance;
            targetPos=new Vector2(targetPos.x,bossCtrl.BossRb.transform.position.y);
            bossCtrl.StartCoroutine(MoveToTargetOnce(targetPos));

    }
    private IEnumerator MoveToTargetOnce(Vector2 targetPos)
    {
        // bossCtrl.BossAni.Play("Spider_Walk");
        while (Vector2.Distance(bossCtrl.BossRb.position, targetPos) > 0.05f) // Cho phép sai số nhỏ
        {
            Vector2 newPos = Vector2.MoveTowards(bossCtrl.BossRb.position, targetPos, moveSpeed * Time.deltaTime);
            bossCtrl.BossRb.MovePosition(newPos);
            yield return null;
        }
        Debug.Log("end");
        isMoving = false;
        // bossCtrl.BossAni.Play("Spider_Idle");
    }

    public void exitHittedAni(){
        isHitted=false;
    }
    public void JumpTo()
    {
        if(bossCtrl.playerPos==null) return;
        if(isRotating||isMoving) return;
        if (!isJumping) 
            StartCoroutine(JumpRoutine(bossCtrl.playerPos.position)); 
    }
    public Collider2D JumpDameCol;
    private IEnumerator JumpRoutine(Vector2 target)
    {
        bossCtrl.BossAni.SetTrigger("PreJump");
        isJumping = true; // Đánh dấu boss đang nhảy
        JumpDameCol.gameObject.SetActive(true);
        Vector2 startPos = transform.position; // Lưu vị trí bắt đầu
        float elapsed = 0f; // Biến đếm thời gian trôi qua

        while (elapsed < jumpDuration) // Lặp đến khi hết thời gian nhảy
        {
            elapsed += Time.deltaTime; // Tăng thời gian trôi qua mỗi frame
            float t = elapsed / jumpDuration; // Tính tỉ lệ tiến trình từ 0 -> 1

            // Lerp để tìm vị trí hiện tại theo tỉ lệ t
            Vector2 pos = Vector2.Lerp(startPos, target, t);

            // Tính độ cao (giả lập) tại thời điểm t từ đường cong
            float height = jumpCurve.Evaluate(t);

            // Cộng chiều cao giả vào trục Y để tạo hiệu ứng nhảy
            transform.position = pos + Vector2.up * height;
            bossCtrl.BossAni.SetFloat("yVelocity",t);
            bossCtrl.BossAni.SetBool("IsJumping",isJumping);

            // Đợi đến frame tiếp theo mới tiếp tục vòng lặp
            yield return null;
        }

        // Đảm bảo vị trí kết thúc chính xác
        transform.position = target;
        // Đánh dấu boss đã nhảy xong
        isJumping = false;
        JumpDameCol.gameObject.SetActive(false);
        bossCtrl.BossAni.SetBool("IsJumping",isJumping);

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            collision.GetComponent<HealthPlayer>().TakeDamage(5);
        }
    }
    public void Flip(){
        if(isRotating) return;
        bossCtrl.BossAni.Play("Spider_Flip");
        isRotating=true;
    }
    public void afterFlip(){
        // Đảo hướng
        isFacingRight = !isFacingRight;
        Debug.Log(isFacingRight);
        // bossCtrl.spriteRenderer.flipX = !bossCtrl.spriteRenderer.flipX;
        transform.localScale=new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
        // Quay lại animation idle
        bossCtrl.BossAni.Play("Spider_Idle", 0, 0);
        isRotating=false;
    }
}
