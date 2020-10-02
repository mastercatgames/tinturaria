using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreController : MonoBehaviour
{
    private UIController uiController;
    public int[] powerUpsPrices;
    public string[] powerUpsTitles;
    public string[] powerUpsDescriptions;
    public int[] bundlesPrices;
    private Transform PowerUpButtons;
    private Transform BundleButtons;
    public GameObject PurchaseAlert;
    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();

        PowerUpButtons = transform.Find("PowerUpPanel").Find("ScrollRect").Find("Content").Find("Buttons");
        // BundleButtons = transform.Find("BundlePanel").Find("ScrollRect").Find("Content").Find("Buttons");

        // powerUpsPrices = new int[6];
        // powerUpsTitles = new string[6];
        // powerUpsDescriptions = new string[6];

        // powerUpsPrices[0] = 4000;
        // powerUpsPrices[1] = 5000;
        // powerUpsPrices[2] = 6000;
        // powerUpsPrices[3] = 7000;
        // powerUpsPrices[4] = 8000;
        // powerUpsPrices[5] = 9000;

        // powerUpsTitles[0] = "All bottles full";
        // powerUpsTitles[1] = "Booster Filling One Bottle";
        // powerUpsTitles[2] = "Fix in Time";
        // powerUpsTitles[3] = "No Broken Bottles";
        // powerUpsTitles[4] = "Booster Filling Box";
        // powerUpsTitles[5] = "Double Cash";

        // powerUpsDescriptions[0] = "Start one level with all bottles full.";
        // powerUpsDescriptions[1] = "You can fill one bottle faster than normal.";
        // powerUpsDescriptions[2] = "You can fix the broken bottles faster (in one level).";
        // powerUpsDescriptions[3] = "The bottles will not break (in one level).";
        // powerUpsDescriptions[4] = "You can fill the boxes with just one touch faster than normal (in one level).";
        // powerUpsDescriptions[5] = "You can get double cash using this power up (in one level).";

        RefreshPowerUpPanel();

        // bundlesPrices = new int[3];

        // bundlesPrices[0] = 25000;
        // bundlesPrices[1] = 50000;
        // bundlesPrices[2] = 75000;

        // for (int i = 0; i < bundlesPrices.Length; i++)
        // {
        //     BundleButtons.GetChild(i).Find("PriceButton").Find("Price").GetComponent<Text>().text = bundlesPrices[i].ToString();
        // }
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
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");

        int buttonIndex = int.Parse(button.transform.Find("_index").GetComponent<Text>().text);

        PurchaseAlert.transform.Find("Content").Find("Alert").Find("PriceButton").Find("Price").GetComponent<Text>().text = button.transform.Find("PriceButton").Find("Price").GetComponent<Text>().text;
        
        if (button.name.Contains("Bundle"))
        {
            PurchaseAlert.transform.Find("Content").Find("Title").Find("Text").GetComponent<Text>().text = "Bundle 1";
            PurchaseAlert.transform.Find("Content").Find("Alert").Find("Description").GetComponent<Text>().text = "123";
        }
        else
        {
            PurchaseAlert.transform.Find("Content").Find("Title").Find("Text").GetComponent<Text>().text = powerUpsTitles[buttonIndex];
            PurchaseAlert.transform.Find("Content").Find("Alert").Find("Description").GetComponent<Text>().text = powerUpsDescriptions[buttonIndex];
        }

        PurchaseAlert.SetActive(true);        
    }

    public void ClosePurchaseAlert()
    {
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");
        StartCoroutine(uiController.ClosePanelAnimation(PurchaseAlert.transform));
    }
}
