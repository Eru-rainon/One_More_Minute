using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Ore : MonoBehaviour
{
    public float collectionDistance = 0.5f;
    private Transform player;

    void Start()
    {
        if (playerFinder.instance.player != null)
        {
            player = playerFinder.instance.player.transform;
        }
        Destroy(gameObject,10f);

    }
    void Update()
    {
        if(player != null){
            float distance = Vector3.Distance(transform.position,player.position);
            if(distance < collectionDistance){
                collectOre();
            }
        }
    }

    void collectOre(){
        player.GetComponent<PlayerOre>().increaseGold();
        Debug.Log("1 ore collected");
        Destroy(gameObject);
    }
}
