using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "upgrades/weaponFirerate")]
public class fireRateBoost : UpgradeData
{
    public float fireRateMultiplier =1.1f;
    public override void applyUpgradetoWeapon(GameObject weapon)
    {
        weapon.GetComponent<assaultRifleScript>().fireRate *= fireRateMultiplier;
    }
    public override void ApplyUpgradetoPlayer(GameObject Player)
    {
        throw new System.NotImplementedException();
    }

    public override void applyUpgradetoEnemy(GameObject EnemySpawner)
    {
        throw new System.NotImplementedException();
    }
}
