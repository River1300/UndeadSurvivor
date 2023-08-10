using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;

    Vector2 inputVec;

    Rigidbody2D rigid;
    Animator anim;
    public SpriteRenderer spriteRenderer;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    void OnEnable()
    {
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerID];
        speed *= Character.Speed;
    }

    void FixedUpdate()
    {
        if(GameManager.instance.isLive)
        {
            Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;

            rigid.MovePosition(rigid.position + nextVec);
        }
    }

    void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);

        if(inputVec.x != 0)
        {
            spriteRenderer.flipX = inputVec.x < 0;
        }
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if(!GameManager.instance.isLive) return;

        GameManager.instance.health -= Time.deltaTime * 10;

        if(GameManager.instance.health <= 0)
        {
            for(int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            anim.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }
}
