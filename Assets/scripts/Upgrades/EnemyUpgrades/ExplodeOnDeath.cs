
using UnityEngine;

[CreateAssetMenu(menuName = "upgrades/explosionOnDeath")]
public class ExplodeOnDeath : UpgradeData
{
    public bool explosionOnDeath = true;
    public float explosionChance = 0.2f;
    public float explosionRadius = 10f;
    public float explosionDamage = 20f;

    public override void applyUpgradetoEnemy(GameObject spawner)
    {
        EnemySpawner enemySpawner = spawner.GetComponent<EnemySpawner>();
        enemySpawner.explosionChance = explosionChance;
        enemySpawner.explosionDamage = explosionDamage;
        enemySpawner.explosionOnDeath = explosionOnDeath;
        enemySpawner.explosionRadius = explosionRadius;
    }

    public override void ApplyUpgradetoPlayer(GameObject Player)
    {
        throw new System.NotImplementedException();
    }

    public override void applyUpgradetoWeapon(GameObject weapon)
    {
        throw new System.NotImplementedException();
    }
}
