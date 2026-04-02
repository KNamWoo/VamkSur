using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType {
        Melee, // 근접
        Range, // 원거리
        Glove, // 장갑 (보조 아이템)
        Shoe, // 신발 (보조 아이템)
        Heal
    }
    
    [Header("# Main Info")]
    public ItemType itemType; // 아이템 종류
    public int itemID; // 아이템 번호
    public string itemName; // 아이템 이름
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon; // 아이템 아이콘
    
    [Header("# Level Data")]
    public float baseDamage; // 아이템의 기초 공격력
    public int baseCount; // 무기의 관통력
    public float[] damages; // 레벨에 따른 추가공격력
    public int[] counts; // 레벨에 따른 관통력

    [Header("# Weapon")]
    public GameObject projectile;
    public Sprite hand;
}
