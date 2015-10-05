//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class Itemshop : MonoBehaviour
//{
//    public int SlotsX, SlotsY;
//    public GUISkin skin;
//    public List<Item> inventory = new List<Item>();
//    public ItemDatabase database;
//    public List<Item> slots = new List<Item>();
//    public bool ItemShop;
//    private bool showTooltip;
//    private string tooltip;

//    private bool draggingItem;
//    private Item draggedItem;
//    private int prevIndex;
//    public Inventory playerStore;


//    // Use this for initialization
//    void Start()
//    {
//        for (int i = 0; i < (SlotsX * SlotsY); i++)
//        {
//            slots.Add(new Item());
//            inventory.Add(new Item());
//        }


//        database = GameObject.FindGameObjectWithTag("Item Database").GetComponent<ItemDatabase>();
//        inventory[0] = database.items[0];
//        inventory[1] = database.items[1];
//        inventory[2] = database.items[2];

//        //AddItem(1);
//        //AddItem(0);
//        //AddItem(2);
//        // RemoveItem(0);
//        //print(InventoryContains(8));

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetButtonDown("ItemShop"))
//        {
//            ItemShop = !ItemShop;

//        }
       
//    }

//    void OnGUI()
//    {

//        if (GUI.Button(new Rect(40, 400, 100, 40), "Save"))
//        {
//            SaveInventory();

//        }

//        if (GUI.Button(new Rect(40, 450, 1000, 400), "Load"))
//        {
//            LoadInventory();

//        }


//        tooltip = " ";
//        GUI.skin = skin;
//        if (ItemShop)
//        {
//            DrawInventory();
//            if (showTooltip)
//            {
//                GUI.Box(new Rect(Event.current.mousePosition.x + 15f, Event.current.mousePosition.y, 200, 200), tooltip);

//            }
//            if (draggingItem)
//            {
//                GUI.DrawTexture(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 50, 50), draggedItem.itemIcon);

//            }

//        }



//        for (int i = 0; i < inventory.Count; i++)
//        {
//            GUI.Label(new Rect(10, i * 10, 200, 50), inventory[i].itemName);

//        }

//    }




//    void DrawInventory()
//    {

//        Event e = Event.current;
//        int i = 0;

//        for (int y = 0; y < SlotsY; y++)
//        {
//            for (int x = 0; x < SlotsX; x++)
//            {
//                Rect slotRect = new Rect(x * 60, y * 160, 50, 50);
//                GUI.Box(slotRect, "", skin.GetStyle("Slots"));
//                slots[i] = inventory[i];
//                Item item = slots[i];
//                if (slots[i].itemName != null)
//                {
//                    GUI.DrawTexture(slotRect, slots[i].itemIcon);
//                    if (slotRect.Contains(e.mousePosition))
//                    {
//                        tooltip = CreateTooltip(slots[i]);
//                        showTooltip = true;
//                        if (e.button == 0 && e.type == EventType.MouseDrag && !draggingItem)
//                        {


//                            draggingItem = true;
//                            draggedItem = slots[i];
//                            inventory[i] = new Item();
//                            prevIndex = i;



//                        }

//                        if (e.type == EventType.MouseUp && draggingItem)
//                        {
//                            inventory[prevIndex] = inventory[i];
//                            inventory[i] = draggedItem;
//                            draggingItem = false;
//                            draggedItem = null;
//                        }

//                        if (e.isMouse && e.type == EventType.mouseDown && e.button == 1)
//                        {
//                            if (slots[i].itemType == Item.ItemType.Consumable)
//                            {
//                                UsedConsumable(slots[i], i, true);

//                            }

//                        }


//                    }

//                    else
//                    {
//                        if (slotRect.Contains(e.mousePosition))
//                        {
//                            if (e.isMouse && e.type == EventType.mouseUp && draggingItem)

//                            {
//                                inventory[i] = draggedItem;
//                                draggingItem = false;
//                                draggedItem = null;
//                            }
//                        }

//                    }


//                }
//                if (tooltip == " ")
//                {

//                    showTooltip = false;

//                }




//                i++;

//            }

//        }

//    }


//    string CreateTooltip(Item item)
//    {
//        tooltip = item.itemName + "\n" + "Description:" + item.itemDesc + "\n" + "Cash:" + item.itemCash;
//        return tooltip;

//    }

//    private void UsedConsumable(Item item, int slot, bool deleteItem)
//    {
//        switch (item.itemID)
//        {

//            case 0:
//                {
//                    print("Used consumable" + item.itemName);
//                    RemoveItem(item.itemID);
//                    //PlayerStats.IncreaseStat(STAT ID, BUFF Amount, BUFF Duration)
//                    break;
//                }

//            case 1:
//                {
//                    print("Used consumable" + item.itemName);
//                    RemoveItem(item.itemID);
//                    //PlayerStats.IncreaseStat(STAT ID, BUFF Amount, BUFF Duration)
//                    break;
//                }


//            case 2:
//                {
//                    print("Used consumable" + item.itemName);
//                    RemoveItem(item.itemID);
//                    //PlayerStats.IncreaseStat(STAT ID, BUFF Amount, BUFF Duration)
//                    break;
//                }



//        }

//    }



//    void RemoveItem(int id)
//    {
//        for (int i = 0; i < inventory.Count; i++)
//        {
//            if (inventory[i].itemID == id)
//            {

//                inventory[i] = new Item();
//                break;
//            }

//        }
//    }



//    public void AddItem(int id)
//    {
//        for (int i = 0; i < inventory.Count; i++)
//        {
//            if (inventory[i].itemName == null)
//            {
//                for (int j = 0; j < database.items.Count; j++)
//                {
//                    if (database.items[j].itemID == id)
//                    {
//                        inventory[i] = database.items[j];


//                    }
                   
//                }



//            }

//        }

//    }


//    bool InventoryContains(int id)
//    {
//        bool result = false;
//        for (int i = 0; i < inventory.Count; i++)
//        {

//            result = inventory[i].itemID == id;
//            if (result)
//            {

//                break;
//            }


//        }

//        return result;
//    }


//    void SaveInventory()
//    {
//        for (int i = 0; i < inventory.Count; i++)
//        {
//            PlayerPrefs.SetInt("Inventory" + i, inventory[i].itemID);
//        }
//    }



//    void LoadInventory()
//    {
//        for (int i = 0; i < inventory.Count; i++)
//        {
//            inventory[i] = PlayerPrefs.GetInt("Inventory" + i, -1) >= 0 ? database.items[PlayerPrefs.GetInt("Inventory" + i)] : new Item();
//        }

//    }



    


//}
