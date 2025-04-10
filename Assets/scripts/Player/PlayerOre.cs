
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class PlayerOre : MonoBehaviour
{
    public int goldamount = 0;
    [SerializeField] private TextMeshProUGUI OreText;
    [SerializeField] private TextMeshProUGUI ShopPrompt;

    [SerializeField] private GameObject StoreScreen;
    [SerializeField] private GameObject HUD;



    [SerializeField] List<StoreItemData> items;
    [SerializeField] List<purchaseButton> purchaseButtons;

    private float storeCooldown = 60f;
    private float lastStoreTime = -60f;
    
    
    
    
    
    
    void Start()
    {
        updateUI();
        ShopPrompt.gameObject.SetActive(false); 
    }

    void Update()
    {   
        if (!StoreScreen.activeSelf && Time.time - lastStoreTime >= storeCooldown){
            ShopPrompt.gameObject.SetActive(true);
            ShopPrompt.text = " F to open shop";
        }
        else{
            ShopPrompt.gameObject.SetActive(false);
        }


        if (Input.GetKeyDown(KeyCode.F) && Time.time - lastStoreTime >= storeCooldown){
            ActivateShop();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape) && StoreScreen.activeSelf){
            CloseShop();
        }
    }
    public void increaseGold(){
        goldamount++;
        updateUI();
    }
    public void ActivateShop()
    {
        
        if (StoreScreen.activeSelf) return;

        Time.timeScale = 0f;
        HUD.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StoreScreen.SetActive(true);

        List<StoreItemData> storeItemDatas = GetStoreItems(3);
        setStorePanel(storeItemDatas);
    }


    public void CloseShop()
    {
        Time.timeScale = 1f;
        StoreScreen.SetActive(false);
        HUD.SetActive(true);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        lastStoreTime = Time.time; 
    }


    public void updateUI(){
         OreText.text = goldamount.ToString();
    }

    public void setStorePanel(List<StoreItemData> storeItemDatas){
        for(int i = 0; i < purchaseButtons.Count; i++){
        if (i < purchaseButtons.Count){
            purchaseButtons[i].setStore(storeItemDatas[i]);
            purchaseButtons[i].gameObject.SetActive(true);
        } else {
            purchaseButtons[i].gameObject.SetActive(false); // Hide extra buttons
        }
    }
    }

    public List<StoreItemData> GetStoreItems(int count)
    {
        List<StoreItemData> availableItems = new List<StoreItemData>(items);

        if (availableItems.Count == 0) 
            return new List<StoreItemData>(); 

        count = Mathf.Min(count, availableItems.Count); 

        List<StoreItemData> selectedItems = new List<StoreItemData>();

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, availableItems.Count);
            selectedItems.Add(availableItems[randomIndex]);
            availableItems.RemoveAt(randomIndex); 
        }

        return selectedItems;
}


    

}
