using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public Button toggleBuildButton;
    public Text foodSupplyText;
    public Text waterSupplyText;
    public Text woodSupplyText;
    public Text metalSupplyText;
    public Text deckTrackerText;

    private void Awake() {
        if(GridBuildingSystem.Instance != null) {
            toggleBuildButton.onClick.AddListener(GridBuildingSystem.Instance.ToggleActive);
        }
        if(VillageManager.Instance != null) {
            VillageManager.Instance.OnResourceAmountChange += UpdateResourceUI;
        }
        if(BoatManager.Instance != null) {
            BoatManager.Instance.ChangedDeckEvent += UpdateDeckTracker;
        }

        foodSupplyText.text = "Food Supply: " + VillageManager.Instance.foodSupply.ToString();
        waterSupplyText.text = "Water Supply: " + VillageManager.Instance.waterSupply.ToString();
        woodSupplyText.text = "Wood Supply: " + VillageManager.Instance.woodSupply.ToString();
        metalSupplyText.text = "Metal Supply: " + VillageManager.Instance.metalSupply.ToString();
        deckTrackerText.text = BoatManager.Instance.currentDeck.ToString();
    }

    void UpdateDeckTracker(object sender, BoatManager.ChangedDeckEventArgs args) {
        deckTrackerText.text = BoatManager.Instance.currentDeck.ToString();
    }

    void UpdateResourceUI(object sender, VillageManager.OnResourceAmountChangeArgs args) {
        foodSupplyText.text = "Food Supply: " + args.foodSupply.ToString();
        waterSupplyText.text = "Water Supply: " + args.waterSupply.ToString();
        woodSupplyText.text = "Wood Supply: " + args.woodSupply.ToString();
        metalSupplyText.text = "Metal Supply: " + args.metalSupply.ToString();
    }
}
