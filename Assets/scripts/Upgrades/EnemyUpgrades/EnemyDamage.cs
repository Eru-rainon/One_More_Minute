
using UnityEngine;

[CreateAssetMenu(menuName = "upgrades/enemyDamage")]
public class EnemyDamage : UpgradeData
{
    public float damageReductionMultiplier;
    public override void applyUpgradetoEnemy(GameObject spawner)
    {
        EnemySpawner enemySpawner = spawner.GetComponent<EnemySpawner>();
        enemySpawner.currentDamage = Mathf.RoundToInt(enemySpawner.currentDamage * damageReductionMultiplier);

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
