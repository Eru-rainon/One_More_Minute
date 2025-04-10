
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class purchaseButton : MonoBehaviour
{   
    public Button button;
    public TextMeshProUGUI descriptionText;
    public PlayerOre playerOre;
    private StoreItemData assignedItem;
    public GameObject HUD;
    public GameObject StorePanel;

    public TextMeshProUGUI priceText;
    private Color canBuyColor = Color.green;
    private Color cannotBuyColor = Color.red;

    public void setStore(StoreItemData storeItemData){
        assignedItem = storeItemData;
        descriptionText.text = storeItemData.ItemDescription.ToString();
        updatebutton();
    }
    public void PurchaseItem(){

        if(assignedItem != null && playerOre.goldamount>=assignedItem.price){

            playerOre.goldamount -= assignedItem.price;
            playerOre.updateUI();

            if(assignedItem.itemType == StoreItemData.ItemType.playerItem){
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                assignedItem.purchasedPlayerItem(player);
            }
            else if(assignedItem.itemType == StoreItemData.ItemType.weaponItem){
                GameObject weapon = GameObject.FindGameObjectWithTag("Weapon");
                assignedItem.PurchasedWeaponItem(weapon);
            }
            playerOre.CloseShop();
        }
        
    }

    private void updatebutton(){
        priceText.text = assignedItem.price.ToString() + "  ORE";
        priceText.color = assignedItem.price <= playerOre.goldamount ? canBuyColor : cannotBuyColor;
    }

}
