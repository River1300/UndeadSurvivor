using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// #. Gear 클래스 : 보조 아이템 능력 정의
public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
// #. 사용자가 보조 아이템을 선택했을 때 보조 아이템의 속성을 받아와서 배정
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        type = data.itemType;
        rate = data.damages[0];
// #. 보조 아이템 타입에 맞게 능력치 보정
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch(type)
        {
        case ItemData.ItemType.Glove:
            RateUp();
            break;

        case ItemData.ItemType.Shoe:
            SpeedUp();
            break;
        }
    }

    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach(Weapon weapon in weapons)
        {
            switch(weapon.id)
            {
            case 0:
                {
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;
                }
            case 1:
                {
                    float speed = 0.5f * Character.WeaponRate;
                    weapon.speed = speed * (1.0f - rate);
                    break;
                }
            default:
                break;
            }
        }
    }

    void SpeedUp()
    {
        float speed = 3.0f * Character.Speed;

        GameManager.instance.player.speed = speed + speed * rate;
    }
}
