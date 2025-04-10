using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class temporarybossScript : MonoBehaviour
{
    private Pillar linkedPillar;
    public void setPillar(Pillar pillar){
        linkedPillar = pillar;
    }

    public void onBossDeath(){
        Gamelogic gamelogic = GameObject.FindGameObjectWithTag("Logic").GetComponent<Gamelogic>();
        gamelogic.defeataBoss();
        linkedPillar.BossDefeated();
    }

}
