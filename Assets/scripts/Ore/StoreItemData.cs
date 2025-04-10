using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public abstract class StoreItemData : ScriptableObject
{
    public enum ItemType{
        playerItem,
        weaponItem,
    }
    public int price;
    public String ItemName;
    public String ItemDescription;
    public ItemType itemType;

    public abstract void purchasedPlayerItem(GameObject Player);
    public abstract void PurchasedWeaponItem(GameObject Weapon);
}
