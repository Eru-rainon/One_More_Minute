
using UnityEngine;

[CreateAssetMenu(menuName = "upgrades/enemyHealth")]
public class enemyHealth : UpgradeData
{
   public float enemyHealthmultiplier;

    public override void applyUpgradetoEnemy(GameObject spawner)
    {
        EnemySpawner enemySpawner = spawner.GetComponent<EnemySpawner>();
        enemySpawner.CurrentHealth = Mathf.RoundToInt(enemySpawner.CurrentHealth*enemyHealthmultiplier);
        enemySpawner.LogEnemyStats();
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
