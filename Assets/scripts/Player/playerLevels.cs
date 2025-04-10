
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class playerLevels : MonoBehaviour
{

    public float playerLevel = 0;
    public float playerXP = 0;
    public float XPtoLevelUp = 10;
    //UI elements
    [SerializeField] private TextMeshProUGUI currentLevel;
    [SerializeField] private Slider XPslider;
    public int XPmultiplier;
    [SerializeField] private GameObject HeadsUpDisplay;
    [SerializeField] private GameObject UpgradeScreen;

    //upgrades
    [SerializeField] List<UpgradeData> upgrades;
    public List<UpgradeData> unlockedUpgrades = new List<UpgradeData>();
    [SerializeField] List<upgradeButton> upgradeButtons;

    

    
    void Awake(){
        updateUI();
        XPmultiplier =1;
    }
    //for testing
    void Update(){
        if(Input.GetKeyDown(KeyCode.L)){
            increaseLevel();
        }
    }

    public void IncreaseXP(float XP){
        playerXP += XP*XPmultiplier;
        updateUI();
        if(playerXP >= XPtoLevelUp)
            increaseLevel();

    }
    public void increaseLevel(){
        Time.timeScale = 0;

        //reactivating Cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        HeadsUpDisplay.SetActive(false);
        UpgradeScreen.SetActive(true);
        updateUpgradePanel(GetUpgrades(3,unlockedUpgrades));
        playerLevel++;
        XPtoLevelUp = Mathf.CeilToInt(XPtoLevelUp * 1.2f);
        playerXP = 0;
        updateUI();
    }
    private void updateUI(){
         currentLevel.text =playerLevel.ToString();
         XPslider.maxValue = XPtoLevelUp;
         XPslider.value = playerXP;
    }
    public void updateUpgradePanel(List<UpgradeData> upgradeDatas){
    for(int i = 0; i < upgradeButtons.Count; i++){
        if (i < upgradeDatas.Count){
            upgradeButtons[i].setText(upgradeDatas[i]);
            upgradeButtons[i].gameObject.SetActive(true);
        } else {
            upgradeButtons[i].gameObject.SetActive(false); // Hide extra buttons
        }
    }
    }

    public void ExitUpgradeScreen(){
        HeadsUpDisplay.SetActive(true);
        UpgradeScreen.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public List<UpgradeData> GetUpgrades(int count , List<UpgradeData> unlockedUpgrades){
        List<UpgradeData> upgradelist = new List<UpgradeData>();
        List<UpgradeData> validUpgrades = new List<UpgradeData>();

        Dictionary<upgradeRarity,float>  rarityWeights = new Dictionary<upgradeRarity, float>{
            {upgradeRarity.common, 0.5f},
            {upgradeRarity.rare, 0.3f},
            {upgradeRarity.legendary, 0.1f}

        };

        foreach(UpgradeData upgradeData in upgrades){
            if(upgradeData.isUpgradeUnlockable(unlockedUpgrades) && !unlockedUpgrades.Contains(upgradeData))
                validUpgrades.Add(upgradeData);
            
        }

        if(count > validUpgrades.Count)
            count = validUpgrades.Count;

       

        for(int i =0;i<count;i++){
            UpgradeData selectedUpgrade = getRandomUpgradebyRarity(validUpgrades,rarityWeights);
            if(selectedUpgrade != null){
                upgradelist.Add(selectedUpgrade);
                validUpgrades.Remove(selectedUpgrade);
            }
        }
        

        return upgradelist;
    }

    private UpgradeData getRandomUpgradebyRarity(List<UpgradeData> upgrades, Dictionary<upgradeRarity,float> rarityWeights){
        float totalWeight = 0f;
        foreach(var upgrade in upgrades){
            totalWeight += rarityWeights[upgrade.upgradeRarity];
        }
        float randomvalue = Random.value * totalWeight;
        float cumulativeWeight = 0f;
        foreach(var upgrade in upgrades){
            cumulativeWeight += rarityWeights[upgrade.upgradeRarity];
            if(randomvalue <= cumulativeWeight)
                return upgrade;
        }

        return null;
    }
}
