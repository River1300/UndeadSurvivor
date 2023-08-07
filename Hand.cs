using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public bool isLeft;

    Vector3 rightPos = new Vector3(0.34f, -0.15f, 0.0f);
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0.0f);
    Quaternion leftRot = Quaternion.Euler(0, 0, -15);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -165);

    public SpriteRenderer spriteRenderer;
    SpriteRenderer player;

    void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    void LateUpdate()
    {
        bool isReverse = player.flipX;

        if(isLeft)
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriteRenderer.flipY = isReverse;
            spriteRenderer.sortingOrder = isReverse ? 4 : 6;
        }
        else
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriteRenderer.flipX = isReverse;
            spriteRenderer.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
