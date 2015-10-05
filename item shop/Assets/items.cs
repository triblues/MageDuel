using UnityEngine;
using System.Collections;


[System.Serializable]
public class Item
{

    public string itemName;
    public int itemID;
    public string itemDesc;
    public Texture2D itemIcon;
    public int itemPower;
    public int itemSpeed;
    public ItemType itemType;
    public int itemCash;

    public enum ItemType
    {
        Weapon,
        Consumable,
        Quest


    }

    public Item(string name, int id, string desc, int power, int speed, ItemType type, int cash)
    {
        itemName = name;
        itemID = id;
        itemDesc = desc;
        itemIcon = Resources.Load<Texture2D>("icons/" + name);
        itemPower = power;
        itemSpeed = speed;
        itemType = type;
        itemCash = cash;

    }

    public Item()
    {


    }
}
