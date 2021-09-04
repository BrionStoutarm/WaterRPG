using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public Button toggleBuildButton;
    public Text foodSupplyText;
    public Text waterSupplyText;

    private void Awake() {
        if(GridBuildingSystem.Instance != null) {
            toggleBuildButton.onClick.AddListener(GridBuildingSystem.Instance.ToggleActive);
        }
        if(VillageManager.Instance != null) {
            VillageManager.Instance.OnResourceAmountChange += UpdateResourceUI;
        }
    }
    
    void UpdateResourceUI(object sender, VillageManager.OnResourceAmountChangeArgs args) {
        foodSupplyText.text = "Food Supply: " + args.foodSupply.ToString();
        waterSupplyText.text = "Water Supply: " + args.waterSupply.ToString();
    }
}
