using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StoreItems/Stamina")]
public class Stamina : StoreItemData
{
    public float StaminaAmount;

    public override void purchasedPlayerItem(GameObject Player)
    {
         FirstPerson firstPerson = Player.GetComponent<FirstPerson>();
        firstPerson.currentStamina += StaminaAmount;
        firstPerson.canSprint = true;

        if(firstPerson.currentStamina >= firstPerson.maxStamina)
            firstPerson.currentStamina = firstPerson.maxStamina;


        firstPerson.uImanager.updateStamina(firstPerson.staminaSlider,firstPerson.currentStamina,firstPerson.maxStamina);
    }

    public override void PurchasedWeaponItem(GameObject Weapon)
    {
        throw new System.NotImplementedException();
    }
}
