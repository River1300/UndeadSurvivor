using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// #. Reposition 클래스 : 맵과 몬스터에 부착되어 플레이어의 이동에 따라 위치를 재조정해주는 역할
public class Reposition : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D other)
    {
// #. 플레이어의 자식으로 있는 Area 콜라이더를 벗어났을 때 위치 재조정을 실행
        if(!other.CompareTag("Area")) return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        switch(transform.tag)
        {
            case "Ground":
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);
                transform.Translate(diffX < diffY ? 
                    Vector3.up * dirY * 60 :
                    Vector3.right * dirX * 60
                    );
                break;

            case "Enemy":
                if(coll.enabled)
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(
                        Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f), 0.0f
                    );
                    transform.Translate(ran + dist * 3);
                }
                break;
        }
    }
}
