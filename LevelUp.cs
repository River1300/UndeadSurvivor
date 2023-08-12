using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{
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
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBgm(true);

        Next();
        rect.localScale = Vector3.one;

        GameManager.instance.Stop();
    }

    public void Hide()
    {
        AudioManager.instance.EffectBgm(false);

        rect.localScale = Vector3.zero;

        GameManager.instance.Resume();
    }

    public void Select(int index)
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);

        items[index].OnClick();
    }
}
