using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillageManager : MonoBehaviour {
    public int startFoodSupply, startWaterSupply, startWoodSupply, startMetalSupply;

    public int foodSupply { get; private set; }
    public int waterSupply { get; private set; }
    public int woodSupply { get; private set; } 
    public int metalSupply { get; private set; }

    public List<GameObject> villagerList;

    public int foodConsumptionModifier = 1;
    public int waterConsumptionModifier = 2;

    public GameObject villagerPrefab;

    public event EventHandler<OnResourceAmountChangeArgs> OnResourceAmountChange;
    public class OnResourceAmountChangeArgs : EventArgs {
        public int foodSupply;
        public int waterSupply;
        public int woodSupply;
        public int metalSupply;
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
        foodSupply = startFoodSupply;
        woodSupply = startWoodSupply;
        waterSupply = startWaterSupply;
        metalSupply = startMetalSupply;
        GridBuildingSystem.Instance.OnPlacedBuilding += HandlePlacedBuilding;
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

    void HandlePlacedBuilding(object sender, GridBuildingSystem.OnPlacedBuildingArgs args) {
        foodSupply -= args.placedObject.GetBuildingType().foodCost;
        waterSupply -= args.placedObject.GetBuildingType().waterCost;
        woodSupply -= args.placedObject.GetBuildingType().woodCost;
        metalSupply -= args.placedObject.GetBuildingType().metalCost;
        if (OnResourceAmountChange != null) { OnResourceAmountChange(this, new OnResourceAmountChangeArgs { foodSupply = foodSupply, waterSupply = waterSupply, woodSupply = woodSupply, metalSupply = metalSupply }) ; }
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
