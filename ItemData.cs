using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// #. ItemData
//  게임에 등장하는 아이템을 관리하기 위한 클래스
//  해당하는 아이템의 데미지 || 공격 속도 || 관통력 || 갯수 || 이동 속도를 속성으로 갖는다.
//  이 클래스를 이용하여 속성을 지정하면서 아이템을 추가한다.
//  이 클래스를 이용하여 플레이어의 아이템 레벨없에 따라 속성의 변화를 준다.
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }
    [Header("----- Main Info -----")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    public string itemDesc;
    public Sprite itemIcon;
    
    [Header("----- Level Data -----")]
    public float baseDamage;
    public int baseCount;
    public float[] damages;
    public int[] counts;

    [Header("----- Weapon -----")]
    public GameObject projectile;
}
