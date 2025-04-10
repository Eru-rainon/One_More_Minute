
using UnityEngine;


[CreateAssetMenu(menuName = "StoreItems/Heal")]
public class Heal : StoreItemData
{
    public float healAmount = 10f;

    public override void purchasedPlayerItem(GameObject Player)
    {
         PlayerHealthManager playerHealthManager = Player.GetComponent<PlayerHealthManager>();
        playerHealthManager.currentHealth += healAmount;

        if(playerHealthManager.currentHealth >= playerHealthManager.maxhealth)
            playerHealthManager.currentHealth = playerHealthManager.maxhealth;


        playerHealthManager.uImanager.UpdateHealth(playerHealthManager.slider,playerHealthManager.currentHealth,playerHealthManager.maxhealth);
    }

    public override void PurchasedWeaponItem(GameObject Weapon)
    {
        throw new System.NotImplementedException();
    }
}
