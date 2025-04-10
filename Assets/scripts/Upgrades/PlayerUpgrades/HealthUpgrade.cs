using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "upgrades/playerHealth")]
public class HealthUpgrade : UpgradeData
{
    public float healthamont = 25f;
    public override void ApplyUpgradetoPlayer(GameObject player){
        PlayerHealthManager playerHealthManager = player.GetComponent<PlayerHealthManager>();
        playerHealthManager.maxhealth += healthamont;
        playerHealthManager.currentHealth += healthamont;
        playerHealthManager.uImanager.UpdateHealth(playerHealthManager.slider,playerHealthManager.currentHealth,playerHealthManager.maxhealth);
    }
    public override void applyUpgradetoWeapon(GameObject weapon)
    {
        throw new System.NotImplementedException();
    }

    public override void applyUpgradetoEnemy(GameObject EnemySpawner)
    {
        throw new System.NotImplementedException();
    }
}
