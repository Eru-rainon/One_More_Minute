using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "upgrades/weaponDamage")]
public class damageBoost : UpgradeData
{
    public float damageMultiplier = 1.1f;
    public override void applyUpgradetoWeapon(GameObject weapon)
    {
        weapon.GetComponent<assaultRifleScript>().damage *= damageMultiplier;
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
