
using UnityEngine;
using UnityEngine.UI;


public class EnemyHealthBar : MonoBehaviour
{


   public void updateHealth(Slider slider,float currentHealth,float maxhealth){
        slider.maxValue = maxhealth;
        slider.value = currentHealth;
   }
}
