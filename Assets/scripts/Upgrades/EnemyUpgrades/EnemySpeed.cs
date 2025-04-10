
using UnityEngine;

[CreateAssetMenu(menuName = "upgrades/enemySpeed")]
public class EnemySpeed : UpgradeData
{
    public float speedreductionMultiplier;

    public override void applyUpgradetoEnemy(GameObject spawner)
    {
        EnemySpawner enemySpawner = spawner.GetComponent<EnemySpawner>();
        enemySpawner.currentWalkSpeed = Mathf.Round(enemySpawner.currentWalkSpeed * speedreductionMultiplier * 100f) / 100f;
        enemySpawner.currentRunSpeed = Mathf.Round(enemySpawner.currentRunSpeed * speedreductionMultiplier * 100f) / 100f;
        enemySpawner.currentSpeedpercentage+=(1-speedreductionMultiplier)*100;
        Debug.Log(enemySpawner.currentSpeedpercentage);
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
