using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public int count;
    public float damage;
    public float speed;

    Player player;
    Vector3 initPos;

    float timer;
    float moveDistance = 3.0f;
    float curDistance;
    bool moveU;

    float thrustDistance = 2.0f;
    float thrustSpeed = 20.0f;
    float returnSpeed = 5.0f;
    bool isThrust;
    bool isFlipX;

    void Awake()
    {
        player = GameManager.instance.player;

        moveU = true;
    }

    void Update()
    {
        if(!GameManager.instance.isLive) return;

        switch(id)
        {
        case 0:
            transform.Rotate(Vector3.back * speed * Time.deltaTime);
            break;

        case 1:
            timer += Time.deltaTime;
            if(timer > speed)
            {
                timer = 0;
                Fire();
            }
            break;

        case 5:
            if (moveU)
            {
                curDistance += speed * Time.deltaTime;
                if (curDistance >= moveDistance) moveU = false;
            }
            else
            {
                curDistance -= speed * Time.deltaTime;
                if (curDistance <= -moveDistance) moveU = true;
            }
            // 무기의 회전 값을 매 프레임마다 지정하여 회전 시킨다.
            transform.rotation = Quaternion.Euler(0, 0, 500 * Time.time);
            transform.position = 
                player.transform.position + Vector3.up * curDistance;
            break;

        case 6:
            isFlipX = player.spriteRenderer.flipX;
            FlipRotate();
            initPos = player.transform.position + 
                (isFlipX ? Vector3.left : Vector3.right) * 0.01f;
            timer += Time.deltaTime;
            if(timer > speed && !isThrust)
            {
                timer = 0;
                isThrust = true;
                Vector3 targetPosition = transform.position + 
                    (isFlipX ? Vector3.left : Vector3.right) * thrustDistance;
                StartCoroutine(ThrustAndReturn(targetPosition));
            }
            break;

        case 7:
            timer += Time.deltaTime;
            if(timer > speed)
            {
                timer = 0;
                UltraFire();
            }
            break;
        }
    }

    public void Init(ItemData data) // #1. 초기화
    {
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero; // 1. 플레이어 오브젝트의 자식으로 무기를 등록

        id = data.itemId;   // 2. 무기의 id를 등록
        damage = data.baseDamage + Character.WeaponDamage;
        count = data.baseCount; // 3. 이 무기의 기본 데미지와 (갯수 || 관통력) 등록

        for(int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
        {
            if(data.projectile == GameManager.instance.pool.prefabs[i])
            {
                prefabId = i;   // 4. 인스턴스할 프리팹ID 등록
                break;
            }
        }

        switch(id)  // 5. 무기의 id에 따라 (회전 속도 || 연사 속도) 를 지정
        {
        case 0:
            speed = 150.0f * Character.WeaponSpeed;
            BatchOne();
            break;

        case 1:
            speed = 0.5f * Character.WeaponRate;
            break;

        case 5:
            speed = 3.5f;
            BatchTwo();
            break;

        case 6:
            speed = 2.0f;
            BatchThree();
            break;

        case 7:
            speed = 20.0f;
            break;
        }

        Hand hand = player.hands[(int)data.itemType];
        hand.spriteRenderer.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void LevelUp(float damage, int count)    // #4. 업그레이드
    {
        this.damage = damage + Character.WeaponDamage;
        this.count += count;    // 1. Item 클래스가 전달해준 값으로 데미지와 (갯수 || 관통력) 을 증가

        if(id == 0)
        {
            BatchOne();
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void FlipRotate()
    {
        if (isFlipX)
        {
            // FlipX가 참일 때, 무기를 180도 회전
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            // FlipX가 거짓일 때, 무기를 원래 방향으로 회전
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void BatchOne()    // #2. 근접 무기 생성
    {
        for(int i = 0; i < count; i++)  // 1. 속성의 갯수 만큼 만들어 배치한다.
        {
            Transform bullet;

            if(i < transform.childCount)    // 2. 이미 만들어져 있는 근접 무기가 있다면 그걸 사용
            {
                bullet = transform.GetChild(i);
            }
            else    // 3. 프리팹ID 를 이용하여 근접 무기를 인스턴스화
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }
            
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity; // 4. 근접 무기를 회전 시키기 전에 위치와 회전 값을 초기화

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);    // 5. 무기 회전

            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);
        }
    }

    void BatchTwo()
    {
        Transform bullet;

        bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.parent = transform;
        
        bullet.localPosition = Vector3.zero;
        bullet.localRotation = Quaternion.identity;

        bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);
    }

    void BatchThree()
    {
        Transform bullet;

        bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.parent = transform;
        
        bullet.localPosition = Vector3.zero;
        bullet.localRotation = Quaternion.identity;

        bullet.Rotate(Vector3.back * 90);
        bullet.Translate(bullet.up * 0.5f, Space.World);

        bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero);
    }

    void Fire() // #3. 원거리 무기 생성
    {
        if(!player.scanner.nearestTarget) return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;   // 1. 적을 향하는 방향 저장

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;   // 2. 프리팹ID 를 이용하여 총알 인스턴스화
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);   // 3. 적을 향해 발사
        
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);

        bullet.GetComponent<Bullet>().Init(damage, count, dir);
    }

    void UltraFire()
    {
        int roundNum = 50;

        for(int i = 0; i < roundNum; i++)
        {
            Vector3 dir = new Vector3(
                Mathf.Cos(Mathf.PI * 2 * i / roundNum), 
                Mathf.Sin(Mathf.PI * 2 * i / roundNum),
                0
            );
            dir = dir.normalized;

            Transform bullet = GameManager.instance.pool.Get(prefabId).transform;

            bullet.position = transform.position;
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bullet.GetComponent<Bullet>().Init(damage, count, dir);
        }
    }

    IEnumerator ThrustAndReturn(Vector3 targetPosition)
    {
        // 찌르기
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, thrustSpeed * Time.deltaTime);
            yield return null;
        }

        // 다시 제자리로 돌아오기
        while (Vector3.Distance(transform.position, initPos) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initPos, returnSpeed * Time.deltaTime);
            yield return null;
        }

        // 찌르기 완료 후 초기화
        isThrust = false;
    }
}
