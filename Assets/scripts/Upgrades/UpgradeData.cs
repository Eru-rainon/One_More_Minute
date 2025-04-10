using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public enum upgradeType{
    playerStats,
    weaponstats,
    enemy
}
public enum upgradeRarity{    
    common,
    rare,
    legendary
}

[CreateAssetMenu]
public abstract class UpgradeData : ScriptableObject
{ 
    public upgradeType upgradeType;
    public upgradeRarity upgradeRarity;
    public String upgradeName;
    public String Description;
    public List<UpgradeData> preRequisites;

    public bool isUpgradeUnlockable(List<UpgradeData> unlockedUpgrades){
        foreach(UpgradeData preRequisite in preRequisites){
            if(!unlockedUpgrades.Contains(preRequisite))
                return false;
        }
        return true;
    }
    public abstract void ApplyUpgradetoPlayer(GameObject Player);
    public abstract void applyUpgradetoWeapon(GameObject weapon);
    public abstract void applyUpgradetoEnemy(GameObject spawner);
    
}
