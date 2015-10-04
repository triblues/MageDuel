using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ItemDatabase : MonoBehaviour
{

    public List<Item> items = new List<Item>();


    void Start()
    {


        items.Add(new Item("HpPotion", 1, "heal Hitpoints", 2, 0, Item.ItemType.Consumable, 100));
        items.Add(new Item("Mana_Potion", 2, "heal mana", 2, 0, Item.ItemType.Consumable, 100));
        //items.Add(new Item("powerpotion", 3, "powerup", 3, 0, Item.ItemType.Consumable, 100));
    }


    void onGUI()
    {



    }




}
