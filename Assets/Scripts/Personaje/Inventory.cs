using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory {

    public event EventHandler OnItemListChanged;

    private List<Item> itemList;
    private Action<Item> useItemAction;

    public Inventory(Action<Item> useItemAction) {
        this.useItemAction = useItemAction;
        itemList = new List<Item>();

        AddItem(new Item { itemType = Item.ItemType.Sword, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
        AddItem(new Item { itemType = Item.ItemType.ManaPotion, amount = 1 });

        Debug.Log(itemList.Count);
    }

    public void AddItem(Item item) {
        if (item.IsStackable()) {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList) {
                if (inventoryItem.itemType == item.itemType) {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory) {
                itemList.Add(item);
            }
        } else {
            itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item) {
        if (item.IsStackable()) {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList) {
                if (inventoryItem.itemType == item.itemType) {
                    //Extract the inventory items one by one
                    inventoryItem.amount -= 1;
                    itemInInventory = inventoryItem;
                }
                Debug.Log(inventoryItem.itemType);
            }
            
            if (itemInInventory != null && itemInInventory.amount <= 0) {
                itemList.Remove(itemInInventory);
            }
        } else {
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseItem(Item item) {
        useItemAction(item);
    }

    public List<Item> GetItemList() {
        return itemList;
    }

}
