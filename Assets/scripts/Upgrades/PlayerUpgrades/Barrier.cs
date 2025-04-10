using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "upgrades/barrier")]
public class Barrier : UpgradeData
{
    public int shieldCount;
    public int DamageIgnoreThreshold;
    public float ShieldRefreshTime;

    public override void ApplyUpgradetoPlayer(GameObject Player)
    {
        PlayerHealthManager healthManager = Player.GetComponent<PlayerHealthManager>();
        healthManager.isShielded = true;
        healthManager.shieldCount = shieldCount;
        healthManager.currentshieldCount = shieldCount;
        healthManager.ShieldRefreshTime = ShieldRefreshTime;
        healthManager.DamageIgnoreThreshold = DamageIgnoreThreshold;
        healthManager.uImanager.updateshieldState(shieldCount,true);

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
