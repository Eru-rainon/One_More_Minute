using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gamelogic : MonoBehaviour
{
    #region Singleton
    public static Gamelogic instance;
    void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject LogicManager;
    public float numberofBossesDefeated = 0;
    public float numberofBosses = 4;

    public float currentEnemyHealth = 1f;
    public float currentEnemyDamage = 1f;
    public float currentXPamount = 3f;
    public float currentSpawnInterval = 1f;
    public int currentMaxEnemyThreshold = 150;
    public float currentLookRadius = 15f;
    public EnemySpawner enemySpawner;

    public float maxTimeAvailable = 3600f;
    float currentTimeElapsed = 0f;
    public float difficultyIncreaseInterval = 20f;
    private int currentDifficulty = 0;

    [SerializeField] private Image difficultyImage;
    [SerializeField] private List<Sprite> difficultySprites = new List<Sprite>();

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float mediumMultiplier = 1.1f;
    [SerializeField] private float hardMultiplier = 1.2f;
    [SerializeField] private float extremeMultiplier = 2.27f;
    [SerializeField] private float extremeSpawnMultiplier = 1.4f;
    public float regularscoremultiplier = 1f;
    public float bossScoremultiplier = 1f;
    public float bossHealthmultiplier = 1f;
    public float bossDamagemultiplier = 1f;
    public GameObject Player;
    bool timeExpired = false;
    [SerializeField] private TextMeshProUGUI survivalText;
    


    void Start()
    {
        enemySpawner.currentDamage = 10f;
        enemySpawner.CurrentHealth = 50f;
        enemySpawner.CurrentXPAmount  = 3f;
        enemySpawner.spawnInterval = 2f;
        enemySpawner.maxEnemyThreshold = 150;
        enemySpawner.CurrentLookRadius = 15f;
        enemySpawner.regularscoremultiplier = regularscoremultiplier;
        difficultyImage.sprite = difficultySprites[0];
        StartCoroutine(ManageDifficulty());
    }

    public void defeataBoss()
    {
        numberofBossesDefeated++;
        switch(numberofBossesDefeated){
            case 1 : bossDamagemultiplier = mediumMultiplier;
                     bossHealthmultiplier = mediumMultiplier;
                     bossScoremultiplier = 2f;
                     break;
            case 2:  bossDamagemultiplier = hardMultiplier;
                     bossHealthmultiplier = hardMultiplier;
                     bossScoremultiplier = 3.5f;
                     break;
            case 3:  bossDamagemultiplier = extremeMultiplier;
                     bossHealthmultiplier = extremeMultiplier;
                     bossScoremultiplier = 7f;
                     break;
        }
        if (numberofBossesDefeated >= numberofBosses)
        {
            Player.GetComponent<PlayerScore>().addscore(Mathf.Max(Mathf.RoundToInt((maxTimeAvailable - currentTimeElapsed)*10),0));
            survivalText.gameObject.SetActive(true);
            survivalText.text = "SURVIVE!!!";
            SetMaxDifficulty();

        }
    }



    public void updateEnemySpawner()
    {
        enemySpawner.currentDamage = Mathf.RoundToInt(enemySpawner.currentDamage * currentEnemyDamage);
        enemySpawner.CurrentHealth = Mathf.RoundToInt(enemySpawner.CurrentHealth * currentEnemyHealth);
        enemySpawner.CurrentXPAmount = Mathf.RoundToInt(currentXPamount);
        enemySpawner.spawnInterval /= currentSpawnInterval;
        enemySpawner.maxEnemyThreshold = currentMaxEnemyThreshold;
        enemySpawner.CurrentLookRadius = currentLookRadius;
        enemySpawner.regularscoremultiplier = regularscoremultiplier;
    }

    private IEnumerator ManageDifficulty()
    {
        while (currentTimeElapsed < maxTimeAvailable)
        {
            yield return new WaitForSeconds(1f);
            currentTimeElapsed += 1f;
            UpdateTime();
            if (currentTimeElapsed >= (currentDifficulty + 1) * difficultyIncreaseInterval)
            {
                increaseDifficulty();
            }
        }

        timeText.text = "TIME UP!";
        timeExpired = true;
        StartCoroutine(ApplyDamageOverTime());
    }

    void increaseDifficulty()
    {
        currentDifficulty++;

    

        switch (currentDifficulty)
        {
            case 1: // Medium
                
                difficultyImage.sprite = difficultySprites[1];
                currentEnemyHealth = mediumMultiplier;
                currentEnemyDamage = mediumMultiplier;
                currentSpawnInterval = mediumMultiplier;
                currentXPamount = 5;
                regularscoremultiplier = 2f;
                currentMaxEnemyThreshold = 200;
                currentLookRadius = 20f;
                break;

            case 2: // Hard
                
                difficultyImage.sprite = difficultySprites[2];
                currentEnemyHealth = hardMultiplier;
                currentEnemyDamage = hardMultiplier;
                currentSpawnInterval = hardMultiplier;
                regularscoremultiplier = 3.5f;
                currentXPamount = 7;
                currentMaxEnemyThreshold = 250;
                currentLookRadius = 25f;
                break;

            case 3: // Extreme
                
                difficultyImage.sprite = difficultySprites[3];
                currentEnemyHealth = extremeMultiplier;
                currentEnemyDamage = extremeMultiplier;
                currentSpawnInterval = extremeSpawnMultiplier;;
                regularscoremultiplier = 7f;
                currentXPamount = 12;
                currentMaxEnemyThreshold = 300;
                currentLookRadius = 30f;
                break;
        }

        updateEnemySpawner();
    }

    public void UpdateTime()
    {
        int minutes = Mathf.FloorToInt(currentTimeElapsed / 60);
        int seconds = Mathf.FloorToInt(currentTimeElapsed % 60);
        timeText.text = String.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private IEnumerator ApplyDamageOverTime()
    {
        while(Player != null){
            Player.GetComponent<PlayerHealthManager>().applyDamage(5f);
            yield return new WaitForSeconds(1f);
        }
        yield break;

        
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void mainmenu(){
        SceneManager.LoadScene(0);
    }

    private void SetMaxDifficulty()
    {
        Debug.Log("Max difficulty activated!");

        // Set extreme values for enemies
        currentDifficulty = 10;
        currentEnemyHealth = extremeMultiplier * 2.5f;
        currentEnemyDamage = extremeMultiplier * 2.5f;
        currentSpawnInterval = extremeSpawnMultiplier * 2f;
        currentXPamount = 25;
        regularscoremultiplier = 10f;
        currentMaxEnemyThreshold = 500;
        currentLookRadius = 50f;

        // Change UI to indicate max difficulty
        difficultyImage.sprite = difficultySprites[difficultySprites.Count - 1]; // Use last sprite for max difficulty

        // Apply the changes to the enemy spawner
        updateEnemySpawner();
    }
}
