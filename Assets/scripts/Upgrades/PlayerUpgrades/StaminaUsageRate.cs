
using UnityEngine;
[CreateAssetMenu(menuName = "upgrades/playerStaminaUsage")]
public class StaminaUsageRate : UpgradeData
{
    public float StaminaUsageRateMultiplier = 0.8f;
    public override void ApplyUpgradetoPlayer(GameObject Player)
    {
        Player.GetComponent<FirstPerson>().StaminaUsageMultiplier *= StaminaUsageRateMultiplier;
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
