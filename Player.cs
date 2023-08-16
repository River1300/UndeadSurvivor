using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// #. Player 클래스 : 플레이어의 행동과 외형을 담당한다.
public class Player : MonoBehaviour
{
    [Header("-----Active-----")]
    public Scanner scanner;
    public float speed;
    Vector2 inputVec;
    Rigidbody2D rigid;
    
    [Header("-----Skin-----")]
    public SpriteRenderer spriteRenderer;
    public Hand[] hands;
    public RuntimeAnimatorController[] animCon;
    Animator anim;

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
// #. 사용자가 GameStart 버튼으로 원하는 캐릭터 버튼을 클릭할 때 게임 매니저에 캐릭터 ID를 전달
//      => 플레이어의 애니메이션 컨트롤러에 해당 캐릭터 ID를 적용하여 출력
        anim.runtimeAnimatorController = animCon[GameManager.instance.playerID];
// #. 캐릭터의 특성에 따른 효과를 적용
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
// #. 플레이어가 죽게 된다면 플레이어의 외형( 그림자, 무기 )을 비활성화 한다.
//      => transform 컴포넌트를 통해 자식의 갯수만큼 반복하여 자식을 하나 하나 비활성화
            for(int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            anim.SetTrigger("Dead");
// #. 플레이어가 죽었으니 게임 매니저를 통해서 게임 오버를 호출
            GameManager.instance.GameOver();
        }
    }
}
