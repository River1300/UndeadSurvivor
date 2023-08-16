using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// #. Enemy 클래스 : 몬스터의 행동과 외형을 담당
public class Enemy : MonoBehaviour
{
    [Header("----- Enemy Active -----")]
    public float maxHealth;
    public float health;
    public float speed;
    public Rigidbody2D target;
    Rigidbody2D rigid;
    WaitForFixedUpdate wait;
    bool isLive;

    [Header("----- Enemy Skin -----")]
    public RuntimeAnimatorController[] animCon;
    Animator anim;
    Collider2D coll;
    SpriteRenderer spriteRenderer;
    
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
// #. 몬스터가 활성화될 때 게임 매니저를 통해서 추적 대상( 플레이어 )을 받는다.
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
// #. 외부 Spawner 클래스로 부터 몬스터 소환 데이터를 받는다.
//      => 소환 데이터를 현재 활성화된 몬스터의 속성에 배정
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
            
            Invoke("Dead", 0.5f);

// #. 몬스터가 죽을 때 게임 매니저에게 자신의 죽음을 알려준다.
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
    }
}