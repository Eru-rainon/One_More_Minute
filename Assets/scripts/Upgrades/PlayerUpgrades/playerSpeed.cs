using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "upgrades/playerSpeed")]
public class playerSpeed : UpgradeData
{
   public float playermovementSpeedMultiplier = 1.2f;
    public override void ApplyUpgradetoPlayer(GameObject Player)
    {
        FirstPerson firstPerson = Player.GetComponent<FirstPerson>();
        firstPerson.walkspeed *= playermovementSpeedMultiplier;
        firstPerson.SprintSpeed *= playermovementSpeedMultiplier;
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
