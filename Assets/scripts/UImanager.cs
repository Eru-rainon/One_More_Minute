using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UImanager : MonoBehaviour
{

    [SerializeField] private GameObject Shield1Image;
    [SerializeField] private GameObject Shield2Image;
    public Gradient gradient;
    public Image healthfill;
    
    public void updateshieldState(int shieldCount, bool shielded){
        if(shielded){
            if(shieldCount == 1){
                Shield1Image.SetActive(true);
                Shield2Image.SetActive(false);
            }else{
                Shield1Image.SetActive(true);
                Shield2Image.SetActive(true);
            }
        }else{
            Shield1Image.SetActive(false);
            Shield2Image.SetActive(false);
        }
    }

    public void UpdateHealth(Slider slider,float currentHealth,float maxHealth){

        slider.maxValue = maxHealth;
        slider.value = currentHealth;
        healthfill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void updateStamina(Slider slider,float currentStamina,float maxStamina){
        slider.maxValue = maxStamina;
        slider.value = currentStamina;
    }
}
