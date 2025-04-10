using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
[CreateAssetMenu(menuName = "upgrades/BurningBullets")]
public class FireBullets : UpgradeData
{
    public float ChancetoBurn = 0.1f;
    public float fireDuration = 3f;
    public float fireDamage = 5f;
    public override void applyUpgradetoWeapon(GameObject weapon)
    {
        assaultRifleScript assaultRifleScript = weapon.GetComponent<assaultRifleScript>();
        assaultRifleScript.CanFireBurningBullets = true;
        assaultRifleScript.FlamingBulletChance = ChancetoBurn;
        assaultRifleScript.fireDuration = fireDuration;
        assaultRifleScript.fireDamage = fireDamage;

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
