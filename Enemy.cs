using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    public float maxHealth;
    public float health;
    public float speed;

    bool isLive;

    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        isLive = true;
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();

        isLive = true;
        health = maxHealth;
    }

    void FixedUpdate()
    {
        if(!isLive) return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if(!isLive) return;
        
        spriteRenderer.flipX = target.position.x < rigid.position.x;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }
}