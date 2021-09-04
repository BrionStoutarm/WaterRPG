using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillageManager : MonoBehaviour
{
    public int foodSupply, waterSupply, woodSupply, metalSupply;
    public List<GameObject> villagerList;

    public int foodConsumptionModifier = 1;
    public int waterConsumptionModifier = 2;

    public GameObject villagerPrefab;

    public event EventHandler<OnResourceAmountChangeArgs> OnResourceAmountChange;
    public class OnResourceAmountChangeArgs : EventArgs {
        public int foodSupply;
        public int waterSupply;
        //more to come
    }

    private static VillageManager s_instance;
    public static VillageManager Instance {
        get => s_instance;
        set {
            if(value != null && s_instance == null) {
                s_instance = value;
            }
        }
    }

    private void Awake() {
        villagerList = new List<GameObject>();
        s_instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ConsumeVillagerResources", 0.0f, 10f);
        CreateVillager();
        CreateVillager();
        CreateVillager();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public static void CreateVillager() {
        GameObject villager = Instantiate(s_instance.villagerPrefab, new Vector3(0, 14, 0), Quaternion.identity);
        s_instance.villagerList.Add(villager);
    }

    void ConsumeVillagerResources() {
        foodSupply -= villagerList.Count * foodConsumptionModifier;
        waterSupply -= villagerList.Count * waterConsumptionModifier;
        if(OnResourceAmountChange != null) { OnResourceAmountChange(this, new OnResourceAmountChangeArgs { foodSupply = foodSupply, waterSupply = waterSupply });  }
        //UpdateUI();
    }

    void UpdateUI() {
        Canvas uiCanvas = FindObjectOfType<Canvas>();
        if (uiCanvas == null) {
            Debug.Log("no canvas");
            return;
        }

        string newFoodSupplyAmount = foodSupply.ToString();
        string newWaterSupplyAmount = waterSupply.ToString();

        GameObject foodTextObject = GameObject.Find("FoodSupplyUIText");
        Text foodText = foodTextObject.GetComponent<Text>();

        if (foodText != null) {
            foodText.text = "Food Supply: " + newFoodSupplyAmount;
        }

        GameObject waterTextObject = GameObject.Find("WaterSupplyUIText");
        Text waterText = waterTextObject.GetComponent<Text>();

        if (waterText != null) {
            waterText.text = "Water Supply: " + newWaterSupplyAmount;
        }
    }
}
