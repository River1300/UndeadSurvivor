using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// #. PoolManager 클래스 : 공장( 몬스터, 무기 )과 만들어진 인스턴스를 담을 컨테이너 관리
public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    List<GameObject>[] pools;

    void Awake()
    {
// #. 2차원 배열의 형태로 공장ID 별로 각각의 인스턴스 리스트를 할당
        pools = new List<GameObject>[prefabs.Length];
        for(int index = 0; index < prefabs.Length; index++)
            pools[index] = new List<GameObject>();
    }

    public GameObject Get(int index)
    {
// #. 외부에서 ID를 전달하여 인스턴스를 요청한다.
//      => 전달 받은 ID를 공장ID로 활용하여 해당 공장의 인스턴스를 활성화 하여 반환
        GameObject select = null;

        foreach(GameObject item in pools[index])
        {
            if(!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if(!select)
        {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
            select.SetActive(true);
        }

        return select;
    }
}
