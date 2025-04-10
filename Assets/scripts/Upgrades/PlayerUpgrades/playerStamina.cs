
using UnityEngine;

[CreateAssetMenu(menuName = "upgrades/playerStamina")]
public class playerStamina : UpgradeData
{
    public float StaminaIncreaseAmount;
    public override void ApplyUpgradetoPlayer(GameObject Player)
    {
        FirstPerson firstPerson = Player.GetComponent<FirstPerson>();
        firstPerson.maxStamina += StaminaIncreaseAmount;
        firstPerson.currentStamina = firstPerson.maxStamina;
        firstPerson.canSprint = true;
        firstPerson.uImanager.updateStamina(firstPerson.staminaSlider,firstPerson.currentStamina,firstPerson.maxStamina);
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
