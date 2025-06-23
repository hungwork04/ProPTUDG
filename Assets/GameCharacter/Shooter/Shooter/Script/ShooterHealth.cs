using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterHealth : MonoBehaviour
{
    HorizontalCameraFollow horizontalCamera;
    private void Awake()
    {
        horizontalCamera=FindAnyObjectByType<HorizontalCameraFollow>();
        if(horizontalCamera!=null){
            horizontalCamera.player=this.transform;
        }
    }
}
