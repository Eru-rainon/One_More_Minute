using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    public void callDealDamge(){
        enemyController enemyController = GetComponentInParent<enemyController>();
        if(enemyController.distanceToTarget <= enemyController.agent.stoppingDistance){
            enemyController.DealDamage();
        }

    }
}
