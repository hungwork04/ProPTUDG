using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform playerPos;
    public Animator BossAni;
    public Rigidbody2D BossRb;
    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        BossAni=GetComponent<Animator>();
        BossRb=GetComponent<Rigidbody2D>();
        spriteRenderer=GetComponent<SpriteRenderer>();
    }

}
