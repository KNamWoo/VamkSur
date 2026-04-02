using System;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;   // 아이템의 데이터 스크립트
    public int      level;  // 플레이어의 레벨
    public Weapon   weapon; // 무기 스크립트
    public Gear     gear; // 장비 스크립트

    Image icon;      // 아이템의 아이콘
    Text  textLevel; // 아이템의 레벨
    Text  textName; // 아이템의 이름
    Text  textDesc; // 아이템의 설명

    void Awake()
    {
        icon        = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;
        
        Text[] texts = GetComponentsInChildren<Text>();
        textLevel     = texts[0];
        textName      = texts[1];
        textDesc      = texts[2];
        textName.text = data.itemName;
    }

    void OnEnable()
    {
        textLevel.text = "Lv."+level;

        switch(data.itemType)
        {
            case ItemData.ItemType.Melee :
            case ItemData.ItemType.Range :
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Glove :
            case ItemData.ItemType.Shoe :
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                textDesc.text = string.Format(data.itemDesc);
                break;
        }
    }

    public void OnClick()
    {
        switch(data.itemType)
        {
            case ItemData.ItemType.Melee :
            case ItemData.ItemType.Range :
                if(level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                } else
                {
                    float nextDamage = data.baseDamage;
                    int   nextCount  = 0;
                    
                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount  += data.counts[level];
                    
                    weapon.LevelUp(nextDamage, nextCount);
                }
                level++;
                break;
            case ItemData.ItemType.Glove :
            case ItemData.ItemType.Shoe :
                if(level == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                } else
                {
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                level++;
                break;
            case ItemData.ItemType.Heal :
                GameManager.instance.HP = GameManager.instance.maxHP;
                break;
        }
        
        if(level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
