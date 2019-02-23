using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UpgradeManager : MonoBehaviour
{	
	public static Dictionary<string, float> upgradeValues = new Dictionary<string, float>() {
		{"speed", 1f},
		{"fireRateBonus", 0.05f},
		{"damageBonus", 2.5f},
		{"health", 15f}
	};

	public static Dictionary<string, KeyValuePair<int, int>> upgradeValuesSteps = new Dictionary<string, KeyValuePair<int, int>>() {
		{"speed", new KeyValuePair<int, int>(300, 250)},
		{"fireRateBonus", new KeyValuePair<int, int>(500, 300)},
		{"damageBonus", new KeyValuePair<int, int>(700, 400)},
		{"health", new KeyValuePair<int, int>(1000, 500)}
	};

	public static Dictionary<string, KeyValuePair<int, int>> upgradeLevelWithPrice = new Dictionary<string, KeyValuePair<int, int>>() {
		{"speed", new KeyValuePair<int, int>(300, 0)},
		{"fireRateBonus", new KeyValuePair<int, int>(500, 0)},
		{"damageBonus", new KeyValuePair<int, int>(700, 0)},
		{"health", new KeyValuePair<int, int>(1000, 0)}
	};

	public List<Button> upgradeButtons = new List<Button>();

	public GameObject upgradePanel = null;
	public Text currency = null;

	public List<Text> levelTexts = new List<Text>();

	public List<Text> costTexts = new List<Text>();


    // Start is called before the first frame update
    void Start()
    {
    	upgradeButtons = upgradePanel.GetComponentsInChildren<Button>().ToList();
        foreach (var button in upgradeButtons) {
        	button.onClick.AddListener(() => Upgrade(button.gameObject.transform.name));
        }
        upgradePanel.SetActive(false);
        currency.gameObject.SetActive(false);
        PlayerStats.GetInstance().AcquireCurrency(10000);
        currency.text = string.Format("Available currency: {0}", PlayerStats.GetInstance().currency.ToString());
        UpdateCostTexts();
    	UpdateLevelTexts();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) {
        	upgradePanel.SetActive(!upgradePanel.activeSelf);
        	currency.gameObject.SetActive(upgradePanel.activeSelf);
        }
    }

    public void Upgrade(string upgradeName) {
    	if (PlayerStats.GetInstance().currency < upgradeLevelWithPrice[upgradeName].Key) return;
    	Debug.Log("here with " + upgradeName);
    	PlayerStats.GetInstance().UpgradeAttribute(upgradeName, upgradeLevelWithPrice[upgradeName].Key);
    	upgradeLevelWithPrice[upgradeName] = new KeyValuePair<int, int>(upgradeLevelWithPrice[upgradeName].Key + upgradeValuesSteps[upgradeName].Value, upgradeLevelWithPrice[upgradeName].Value + 1);
    	currency.text = string.Format("Available currency: {0}", PlayerStats.GetInstance().currency.ToString());
    	UpdateCostTexts();
    	UpdateLevelTexts();
    }

    public void UpdateLevelTexts() {
    	for (int i = 0; i < levelTexts.Count; ++i) {
    		levelTexts[i].text = string.Format("Current level: {0}", upgradeLevelWithPrice[levelTexts[i].gameObject.transform.name].Value);
    	}
    }

    public void UpdateCostTexts() {
    	for (int i = 0; i < costTexts.Count; ++i) {
    		costTexts[i].text = string.Format("Cost: {0}", upgradeLevelWithPrice[costTexts[i].gameObject.transform.name].Key);
    	}
    }
}
