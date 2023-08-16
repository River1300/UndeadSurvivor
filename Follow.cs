using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// #. Follow 클래스 : 체력 슬라이드 바가 플레이어를 따라가는 기능
public class Follow : MonoBehaviour
{
    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        rect.position = Camera.main.WorldToScreenPoint(
            GameManager.instance.player.transform.position + Vector3.down * 0.8f);
    }
}
