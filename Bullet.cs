using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void LateUpdate()
    {
        if(per >= 0)
            Invoke("DeActivated", 3.0f);
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

    void DeActivated()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Enemy") || per == -100) return;

        per--;
        if(per < 0)
        {
            rigid.velocity = Vector2.zero;
            CancelInvoke("DeActivated");
            gameObject.SetActive(false);
        }
    }
}
