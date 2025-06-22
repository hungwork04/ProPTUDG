using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public float walkDistance=12f;
    public float jumpDistance=30f;
    public float atkDistance1=15f;
    public float atkDistance2=25f;
    public float timeOffset=2f;
    BossMovement bossMove;
    BossController bossCtrl;
    BossSkill bossSkill;
    [Header("Move time")]
    public float timer = 0f;
    public float interval = 10f;
    public float atkTimer=10f;
    public float atkinterval = 10f;
    void Awake()
    {
        bossMove= GetComponent<BossMovement>();
        bossCtrl=GetComponent<BossController>();
        bossSkill=GetComponent<BossSkill>();
    }
    void OnDisable()
    {
        StopAllCoroutines();

            // Lấy tất cả Transform con (bao gồm cả chính nó), rồi tắt các GameObject con
            foreach (Transform child in GetComponentsInChildren<Transform>())
            {
                if (child != transform) // Không tắt chính object này
                {
                    child.gameObject.SetActive(false);
                }
            }
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
             // reset đếm
            movebyDistance();
        }
        atkTimer += Time.deltaTime;
        if (atkTimer >= atkinterval)
        {
            atkbyDistance();
        }

    }
    //delay đạn lại 1f
    //delay sau 1 mỗi 1 hành động 
    public void movebyDistance(){
        if (bossSkill.isAttack || bossMove.isRotating || bossMove.isJumping || bossSkill.EndAttack) return;
        if(bossCtrl.playerPos==null) return;
        timer=0f;
        float distance = Vector2.Distance(bossCtrl.playerPos.position, bossCtrl.BossRb.position);    
        if (distance > jumpDistance)
        {
            bossMove.JumpTo();
        }
        else if (distance > walkDistance)
        {
            bossMove.StartMoveToPlayer(); // <-- Chỉ gọi 1 lần
        }
    }
    public enum CurSkill{
        none,
        tenLazer,
        LazerAngle,
        ThrowSkill
    }
    public CurSkill curSkill=CurSkill.none;
    public void atkbyDistance(){
        if(bossSkill.isAttack||bossMove.isMoving||bossMove.isRotating||bossMove.isJumping||bossSkill.EndAttack) return;
        atkTimer=0f;
        if(curSkill == CurSkill.none){
            int index = Random.Range(1, 3);
            if(index % 2 == 0){
                Debug.Log("ban laze");
                curSkill = CurSkill.LazerAngle;
                bossSkill.DoShootLazerByAngle();
            } else {
                curSkill = CurSkill.tenLazer;
                bossSkill.StartCoroutine(bossSkill.ShootTenLazer());
                Debug.Log("ban 10 laze");
            }
        }
        else if(curSkill == CurSkill.LazerAngle){
            int index2 = Random.Range(1, 3);
            if(index2 % 2 == 0){
                Debug.Log("ban laze");
                curSkill = CurSkill.ThrowSkill;
                bossSkill.StartCoroutine(bossSkill.throwSpiderTime());
            } else {
                curSkill = CurSkill.tenLazer;
                bossSkill.StartCoroutine(bossSkill.ShootTenLazer());
                Debug.Log("ban 10 laze");
            }
        }
        else if(curSkill == CurSkill.tenLazer){
            int index2 = Random.Range(1, 3);
            if(index2 % 2 == 0){
                Debug.Log("ban laze");
                curSkill = CurSkill.ThrowSkill;
                bossSkill.StartCoroutine(bossSkill.throwSpiderTime());
            } else {
                Debug.Log("ban laze");
                curSkill = CurSkill.LazerAngle;
                bossSkill.DoShootLazerByAngle();
            }
        }else if(curSkill == CurSkill.ThrowSkill){
            int index2 = Random.Range(1, 3);
            if(index2 % 2 == 0){
                Debug.Log("ban laze");
                curSkill = CurSkill.LazerAngle;
                bossSkill.DoShootLazerByAngle();
            } else {
                curSkill = CurSkill.tenLazer;
                bossSkill.StartCoroutine(bossSkill.ShootTenLazer());
                Debug.Log("ban 10 laze");
            }
        }

        // if(Vector2.Distance(bossCtrl.playerPos.position, bossCtrl.BossRb.position)>atkDistance1){
        //     bossSkill.StartCoroutine(bossSkill.ShootTenLazer());
        //         Debug.Log("ban 10 laze");

        // }else if(Vector2.Distance(bossCtrl.playerPos.position, bossCtrl.BossRb.position)>atkDistance2){
        //    bossSkill.DoShootLazerByAngle();
        //         Debug.Log("ban laze");

        // }else {
        //     int index=Random.Range(1,3);
        //     if(index%2==0){
        //         Debug.Log("ban laze");
        //         bossSkill.DoShootLazerByAngle();

        //     }else 
        //         bossSkill.StartCoroutine(bossSkill.ShootTenLazer());
        //         Debug.Log("ban 10 laze");

        // }
    }
}
