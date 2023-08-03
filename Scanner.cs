using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
