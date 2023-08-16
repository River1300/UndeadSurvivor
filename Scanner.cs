using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// #. Scanner 클래스 : 플레이어에 부착되어 플레이어를 중심으로 지정한 반지름 크기 만큼의 원형 Ray를 발사
//      => 가장 가까이에 있는 적을 찾아낸다.
public class Scanner : MonoBehaviour
{
    public float scanRange;

    public Transform nearestTarget;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;

    void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
    
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float dis = 1000.0f;

        foreach(RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;

            float curDiff = Vector3.Distance(myPos, targetPos);

            if(curDiff < dis)
            {
                dis = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
