using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class XPBehaviour : MonoBehaviour
{
    public float XPAmount = 1f;
    public float moveSpeed = 10f;
    private GameObject Player;
    private bool canPickup = false;
    public float pickUprange = 2f;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

    }

    
    void Update()
    {
        if(Player != null){
            float distance = Vector3.Distance(transform.position,Player.transform.position);
            if(distance < pickUprange)
                canPickup = true;
            if(canPickup)
                transform.position = Vector3.Lerp(transform.position,Player.transform.position,Time.deltaTime*moveSpeed);
            if(distance < 0.5f){
                playerLevels playerLevels = Player.GetComponent<playerLevels>();
                playerLevels.IncreaseXP(XPAmount);
                Destroy(gameObject);
            }
        }
    }
}
