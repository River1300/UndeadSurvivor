using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// #. Bullet 클래스 : 충돌의 주체, 생성과 소멸을 담당
public class Bullet : MonoBehaviour
{
    public int per;
    public float damage;

    Rigidbody2D rigid;

    void Awake()
    {
        if(gameObject.GetComponent<Rigidbody2D>())
            rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.per = per;
        this.damage = damage;

        if(per >= 0)
        {
            rigid.velocity = dir * 10.0f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Enemy") || per == -100) return;

        per--;
        if(per < 0)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(!other.CompareTag("Area") || per == -100) return;

        gameObject.SetActive(false);
    }
}
