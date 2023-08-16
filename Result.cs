using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// #. Result 클래스 : 게임이 종료되는 2가지 경우를 관리
public class Result : MonoBehaviour
{
    public GameObject[] titles;

    public void Win()
    {
        titles[0].SetActive(true);
        titles[1].SetActive(false);
    }

    public void Lose()
    {
        titles[0].SetActive(false);
        titles[1].SetActive(true);
    }
}
