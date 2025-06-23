using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterHealth : MonoBehaviour
{
    HorizontalCameraFollow horizontalCamera;
    CamBoss camBoss;
    BossController bossController;
    private void Awake()
    {
        horizontalCamera=FindAnyObjectByType<HorizontalCameraFollow>();
        if(horizontalCamera!=null){
            horizontalCamera.player=this.transform;
        }
        camBoss=FindAnyObjectByType<CamBoss>();
        if(camBoss!=null){
            camBoss.player1=this.transform;
        }
        bossController=FindAnyObjectByType<BossController>();
        if(bossController!=null){
            bossController.playerPos=this.transform;
        }
    }
}
