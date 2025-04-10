
using UnityEngine;

[CreateAssetMenu(menuName = "upgrades/enemySpeedtoDamage")]
public class speedToDamage : UpgradeData
{
    public override void applyUpgradetoEnemy(GameObject spawner)
    {
        EnemySpawner enemySpawner = spawner.GetComponent<EnemySpawner>();
        enemySpawner.currentDamage = Mathf.RoundToInt(enemySpawner.currentDamage * (1-enemySpawner.currentSpeedpercentage/100));
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
