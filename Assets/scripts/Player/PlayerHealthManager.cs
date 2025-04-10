
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealthManager : MonoBehaviour
{
    [Header("Health & Stamina parameters")]
    //health
    public float maxhealth = 100f;
  
     public float currentHealth;
    


    public UImanager uImanager;
    public FirstPerson firstPerson;
    public GameObject Weapon;

    [Header("Death Cam")]
    private Camera playerCamera;
    public Vector3 DeathCamPosition = new Vector3(0,10,0);
    public float cameraMovetime = 1.5f;

    public GameObject HUD;
    public GameObject deathScreen;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoretext;

    [Header("barrier parameters")]

    
    public float ShieldRefreshTime;
    public bool isShielded = false;
    public int shieldCount = 0;
    public int currentshieldCount = 0;
    public int DamageIgnoreThreshold = 0;
    public TextMeshProUGUI shieldText;

    [Header("Damage Effect")]

    public Image DamageOverlay;
    private float flashDuration = 0.1f;
    private float maxOverlayAlpha = 1f;

    //healthBar
    public Slider slider;





    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        currentHealth = maxhealth;
        slider.maxValue = maxhealth;
        uImanager.UpdateHealth(slider,currentHealth,maxhealth);
        uImanager.updateshieldState(currentshieldCount,currentshieldCount!=0);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            applyDamage(10);
        }
    }
    public void applyDamage(float dmg)
    {
        
        StartCoroutine(DamageFlashEFfect());
        if(isShielded)
        {
            currentshieldCount--;
            uImanager.updateshieldState(currentshieldCount,currentshieldCount != 0);
            if(currentshieldCount <= 0){
                isShielded = false;
                StartCoroutine(refreshShieldaftertime());
            }
            return;
        }
        currentHealth -= Mathf.Max(dmg - DamageIgnoreThreshold, 0);

        uImanager.UpdateHealth(slider,currentHealth,maxhealth);

        if (currentHealth <= 0)
        {
            killPlayer();
        }
    }


    private void killPlayer(){

        currentHealth = 0;
        firstPerson.canMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("dead!!");
        HUD.SetActive(false);
        deathScreen.SetActive(true);
   
        scoreText.text = "SCORE: " + GetComponent<PlayerScore>().currentScore.ToString("D7");
        Debug.Log(GetComponent<PlayerScore>().currentScore.ToString("D7"));
        highscoretext.text = "HS: " + GetComponent<PlayerScore>().highscore.ToString("D7");

        playerCamera.transform.SetParent(null);
        Destroy(Weapon);
        StartCoroutine(moveCamera());
        

    }
    private IEnumerator moveCamera(){
        Vector3 startPosition = playerCamera.transform.position;
        Vector3 targetPosition = transform.position + DeathCamPosition;
        Quaternion startRotation = playerCamera.transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(playerCamera.transform.eulerAngles + new Vector3(30f,0,0));

        applyGreyScale();

        float elapsedTime = 0f;
        while(elapsedTime < cameraMovetime){
            float t = elapsedTime / cameraMovetime;
            playerCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            playerCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        playerCamera.transform.position = targetPosition;
        playerCamera.transform.rotation = targetRotation;
        Destroy(gameObject,2f);
       
        

    }
    private IEnumerator refreshShieldaftertime(){
        yield return new WaitForSeconds(ShieldRefreshTime);
        isShielded = true;
        currentshieldCount = shieldCount;
        uImanager.updateshieldState(currentshieldCount,true);

    }
    private void applyGreyScale(){
         Volume volume = FindObjectOfType<Volume>();
        if (volume != null && volume.profile.TryGet(out ColorAdjustments colorAdjustments))
        {
            colorAdjustments.saturation.value = -100f; // -100 makes everything grayscale
        }
    }

    private IEnumerator DamageFlashEFfect()
    {
        DamageOverlay.color = new Color(1,0,0,maxOverlayAlpha);
        yield return new WaitForSeconds(flashDuration);
        DamageOverlay.color = new Color(1,0,0,0);
    }

    

    


    

}
