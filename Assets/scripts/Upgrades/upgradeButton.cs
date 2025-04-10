
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class upgradeButton : MonoBehaviour
{   
    public Button button;
    public Image background;
    public TextMeshProUGUI descriptionText;
    public playerLevels playerLevels;
    private UpgradeData assignedUpgrade;

    public void setText(UpgradeData upgradeData){
        assignedUpgrade = upgradeData;
        descriptionText.text = upgradeData.Description.ToString();
        
        switch(upgradeData.upgradeRarity){

            case upgradeRarity.common : background.color = new Color32(176,213,230,200);
                break;
            case upgradeRarity.rare : background.color = new Color32(128,0,255,200);
                break;
            case upgradeRarity.legendary : background.color = new Color32(255,215,0,200);
                break;
        }
    }
    public void onclickApplyUpgrade(){
        if(assignedUpgrade != null){
            playerLevels.unlockedUpgrades.Add(assignedUpgrade);
            if(assignedUpgrade.upgradeType == upgradeType.playerStats ){
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                assignedUpgrade.ApplyUpgradetoPlayer(player);
            }
            else if(assignedUpgrade.upgradeType == upgradeType.weaponstats){
                GameObject weapon = GameObject.FindGameObjectWithTag("Weapon");
                assignedUpgrade.applyUpgradetoWeapon(weapon);
            }
            else if(assignedUpgrade.upgradeType == upgradeType.enemy){
                GameObject enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner");
                assignedUpgrade.applyUpgradetoEnemy(enemySpawner);
            }
        }
    }

}
