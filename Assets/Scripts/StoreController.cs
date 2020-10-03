using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameToolkit.Localization;

public class StoreController : MonoBehaviour
{
    private UIController uiController;
    public int[] powerUpsPrices;
    public string[] powerUpsTitles;
    public string[] powerUpsDescriptions;
    public int[] bundlesPrices;
    public Transform PowerUpButtons;
    private Transform BundleButtons;
    public GameObject PurchaseAlert;
    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        RefreshPowerUpPanel();
    }

    public void RefreshPowerUpPanel()
    {
        for (int i = 0; i < powerUpsPrices.Length; i++)
        {
            PowerUpButtons.GetChild(i).Find("YouHave").Find("Count").GetComponent<Text>().text = PlayerPrefs.GetInt("PowerUp_" + PowerUpButtons.GetChild(i).name).ToString();
            // PowerUpButtons.GetChild(i).Find("PriceButton").Find("Price").GetComponent<Text>().text = powerUpsPrices[i].ToString();
        }
    }

    public void OpenPurchaseAlert(GameObject button)
    {
        LocalizedTextBehaviour title = PurchaseAlert.transform.Find("Content").Find("Alert").Find("Title").Find("Text").GetComponent<LocalizedTextBehaviour>();
        LocalizedTextBehaviour description = PurchaseAlert.transform.Find("Content").Find("Alert").Find("Description").GetComponent<LocalizedTextBehaviour>();
        
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");

        title.FormatArgs = new string[1];

        if (button.name.Contains("Tool"))
        {
            title.LocalizedAsset = (LocalizedText)Resources.Load("repair_tool_qty", typeof(LocalizedText));
            description.LocalizedAsset = (LocalizedText)Resources.Load("repair_tool_description", typeof(LocalizedText));

            if (int.Parse(button.name.Split('_')[1]) == 1)
            {                
                title.FormatArgs[0] = "(x1)";
            }
            else if (int.Parse(button.name.Split('_')[1]) == 2)
            {
                title.FormatArgs[0] = "(x3)";
            }
            else if (int.Parse(button.name.Split('_')[1]) == 3)
            {
                title.FormatArgs[0] = "(x5)";
            }
        }
        else
        {
            title.LocalizedAsset = (LocalizedText)Resources.Load(button.name + "_qty", typeof(LocalizedText));
            description.LocalizedAsset = (LocalizedText)Resources.Load(button.name + "_description", typeof(LocalizedText));            
        }

        PurchaseAlert.SetActive(true);        
    }

    public void ClosePurchaseAlert()
    {
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");
        StartCoroutine(uiController.ClosePanelAnimation(PurchaseAlert.transform));
    }
}
