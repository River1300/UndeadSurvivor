using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("----- Enemy Info -----")]
    public float maxHealth;
    public float health;
    public float speed;
    bool isLive;
    
    [Header("----- Game Object -----")]
    public Rigidbody2D target;

    [Header("----- Component -----")]
    public RuntimeAnimatorController[] animCon;
    Rigidbody2D rigid;
    Animator anim;
    Collider2D coll;
    SpriteRenderer spriteRenderer;
    WaitForFixedUpdate wait;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        wait = new WaitForFixedUpdate();
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();

        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriteRenderer.sortingOrder = 2;
        anim.SetBool("Dead", false);

        health = maxHealth;
    }

    void FixedUpdate()
    {
        if(GameManager.instance.isLive)
        {
            if(!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;

            Vector2 dirVec = target.position - rigid.position;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero;
        }
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

    void Dead()
    {
        gameObject.SetActive(false);
    }

    IEnumerator KnockBack()
    {
        yield return wait;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Bullet")) return;
        if(!isLive) return;

        health -= other.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if(health > 0)
        {
            anim.SetTrigger("Hit");
        }
        else
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriteRenderer.sortingOrder = 1;
            anim.SetBool("Dead", true);

            if(GameManager.instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            
            Invoke("Dead", 1.0f);

            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
    }
}