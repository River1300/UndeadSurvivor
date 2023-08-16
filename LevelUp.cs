using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// #. LevelUp 클래스 : 업그레이드 항목을 나열하고 선택된 항목을 Item 클래스에게 전달하는 역할
public class LevelUp : MonoBehaviour
{
    public GameObject healthBar;
    RectTransform rect;
    Item[] items;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    void Next()
    {
        int[] ran = new int[3];

        foreach(Item item in items)
        {   // 0 ~ 7 까지, 8개의 Item 스크립트를 부착한 ui 오브젝트를 비활성화
            item.gameObject.SetActive(false);
        }

        while(true)
        {   // 0 ~ 7 까지의 인덱스 중 랜덤 수 를 구하고 해당 인덱스의 ui 오브젝트를 활성화
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if(ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2]) break;
        }

        for(int i = 0; i < ran.Length; i++)
        {
            Item ranItem = items[ran[i]];
            // 단, 만렙에 달성한 아이템이 있을 경우 다른 아이템을 활성화
            if(ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }
    }

    public void Show()
    {
// #. GameManager 클래스에서 레벨업을 할 때 호출
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBgm(true);

        Next();
        healthBar.SetActive(false);
        rect.localScale = Vector3.one;
// #. 업그레이드 항목을 선택 중에는 게임을 정지
        GameManager.instance.Stop();
    }

    public void Hide()
    {
        AudioManager.instance.EffectBgm(false);

        healthBar.SetActive(true);
        rect.localScale = Vector3.zero;
// #. 선택을 완료 했다면 게임을 진행
        GameManager.instance.Resume();
    }

    public void Select(int index)
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);

        items[index].OnClick();
    }
}
