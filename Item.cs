using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// #. Item
//  ItemData를 받아서 플레이어가 착용한 아이템의 상태를 HUD로 표시하는 역할
//  ItemData를 받아서 플레이어가 획득한 아이템을 Weapon || Gear 클래스에게 알려주는 역할
//  ItemData를 받아서 플레이어가 착용한 아이템의 레벨업을 Weapon || Gear 클래스에게 알려주는 역할
//  ItemData를 받아서 아이템이 레벨업 할 때 속성의 변화 값을 정해주는 역할
public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;
        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
    }

    void OnEnable()
    {
        textLevel.text = "Lv." + (level + 1);

        switch(data.itemType)
        {
        case ItemData.ItemType.Melee:
        case ItemData.ItemType.Range:
            if(data.itemId >= 5)
            {
                textDesc.text = string.Format(data.itemDesc);
            }
            else
            {
                textDesc.text = string.Format(
                    data.itemDesc, data.damages[level] * 100, data.counts[level]);
            }
            break;

        case ItemData.ItemType.Glove:
        case ItemData.ItemType.Shoe:
            textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
            break;
        
        case ItemData.ItemType.Heal:
            textDesc.text = string.Format(data.itemDesc);
            break;
        }
    }

    public void OnClick()
    {
        switch(data.itemType)
        {
        case ItemData.ItemType.Melee:
        case ItemData.ItemType.Range:
            if(level == 0)
            {
                GameObject newWeapon = new GameObject();
                weapon = newWeapon.AddComponent<Weapon>();
                weapon.Init(data);
            }
            else
            {
                float nextDamage = data.baseDamage;
                int nextCount = 0;

                nextDamage += data.baseDamage * data.damages[level];
                nextCount += data.counts[level];

                weapon.LevelUp(nextDamage, nextCount);
            }
            level++;
            break;

        case ItemData.ItemType.Glove:
        case ItemData.ItemType.Shoe:
            if(level == 0)
            {
                GameObject newGear = new GameObject();
                gear = newGear.AddComponent<Gear>();
                gear.Init(data);
            }
            else
            {
                float nextRate = data.damages[level];
                gear.LevelUp(nextRate);
            }
            level++;
            break;

        case ItemData.ItemType.Heal:
            GameManager.instance.health = GameManager.instance.maxHealth;
            break;
        }

        if(level == data.damages.Length) GetComponent<Button>().interactable = false;
    }
}
