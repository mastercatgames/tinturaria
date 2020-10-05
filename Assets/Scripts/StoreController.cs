using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameToolkit.Localization;

public class StoreController : MonoBehaviour
{
    private UIController uiController;
    public Transform RepairsButtons;
    public Transform PowerUpButtons;
    public Transform BundleButtons;
    public Transform PurchaseAlert;
    public string[] repairsPricesCoins;
    public int[] repairsPricesGems;
    public int[] powerUpsPrices;
    public int[] bundlesPrices;
    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();

        //Prices
        #region Repairs
        repairsPricesCoins = new string[5];
        repairsPricesCoins[0] = "1000";
        repairsPricesCoins[1] = "3000";
        repairsPricesCoins[2] = "5000";
        repairsPricesCoins[3] = "10000";
        repairsPricesCoins[4] = "20000";

        repairsPricesGems = new int[5];
        repairsPricesGems[0] = 1;
        repairsPricesGems[1] = 3;
        repairsPricesGems[2] = 5;
        repairsPricesGems[3] = 10;
        repairsPricesGems[4] = 20;
        #endregion

        #region Power Ups
        powerUpsPrices = new int[6];
        powerUpsPrices[0] = 3;
        powerUpsPrices[1] = 4;
        powerUpsPrices[2] = 4;
        powerUpsPrices[3] = 5;
        powerUpsPrices[4] = 8;
        powerUpsPrices[5] = 8;
        #endregion

        #region Bundles
        bundlesPrices = new int[3];
        bundlesPrices[0] = 25;
        bundlesPrices[1] = 45;
        bundlesPrices[2] = 70;
        #endregion

        RefreshRapairsPanel();
        RefreshPowerUpPanel();
        RefreshBundlesPanel();
    }

    public void RefreshRapairsPanel()
    {
        for (int i = 0; i < RepairsButtons.childCount; i++)
        {
            //Get Tools Count
            RepairsButtons.GetChild(i).Find("YouHave").Find("Count").GetComponent<Text>().text = PlayerPrefs.GetInt("toolsCount").ToString();
            RepairsButtons.GetChild(i).Find("PriceButtonCoins").Find("Price").GetComponent<Text>().text = repairsPricesCoins[i];
            RepairsButtons.GetChild(i).Find("PriceButton").Find("Price").GetComponent<Text>().text = repairsPricesGems[i].ToString();
        }
    }

    public void RefreshPowerUpPanel()
    {
        for (int i = 0; i < PowerUpButtons.childCount; i++)
        {
            PowerUpButtons.GetChild(i).Find("YouHave").Find("Count").GetComponent<Text>().text = PlayerPrefs.GetInt("PowerUp_" + PowerUpButtons.GetChild(i).name).ToString();
            PowerUpButtons.GetChild(i).Find("PriceButton").Find("Price").GetComponent<Text>().text = powerUpsPrices[i].ToString();
        }
    }

    public void RefreshBundlesPanel()
    {
        for (int i = 0; i < BundleButtons.childCount; i++)
        {
            BundleButtons.GetChild(i).Find("PriceButton").Find("Price").GetComponent<Text>().text = bundlesPrices[i].ToString();
        }
    }

    public void OpenPurchaseAlert(GameObject button)
    {
        LocalizedTextBehaviour title = PurchaseAlert.Find("Content").Find("Alert").Find("Title").Find("Text").GetComponent<LocalizedTextBehaviour>();
        LocalizedTextBehaviour description = PurchaseAlert.Find("Content").Find("Alert").Find("Description").GetComponent<LocalizedTextBehaviour>();

        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");

        Transform buttons = PurchaseAlert.Find("Content").Find("Alert").Find("Buttons");

        string priceInCoins = "";
        string priceInGems = "";        

        if (button.name.Contains("Tool"))
        {
            title.LocalizedAsset = (LocalizedText)Resources.Load("repair_tool_qty", typeof(LocalizedText));
            description.LocalizedAsset = (LocalizedText)Resources.Load("repair_tool_description", typeof(LocalizedText));

            priceInCoins = button.transform.Find("PriceButtonCoins").Find("Price").GetComponent<Text>().text;
            priceInGems = button.transform.Find("PriceButton").Find("Price").GetComponent<Text>().text;

            buttons.Find("PriceButtonCoins").Find("Price").GetComponent<Text>().text = priceInCoins;
            buttons.Find("PriceButtonGems").Find("Price").GetComponent<Text>().text = priceInGems;

            title.FormatArgs = new string[1];

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
            else if (int.Parse(button.name.Split('_')[1]) == 4)
            {
                title.FormatArgs[0] = "(x10)";
            }
            else if (int.Parse(button.name.Split('_')[1]) == 5)
            {
                title.FormatArgs[0] = "(x20)";
            }

            buttons.Find("PriceButtonCoins").gameObject.SetActive(true);
        }
        else
        {
            //If is the Power Ups or Bundles panel...
            priceInGems = button.transform.Find("PriceButton").Find("Price").GetComponent<Text>().text;
            buttons.Find("PriceButtonCoins").gameObject.SetActive(false);
            buttons.Find("PriceButtonGems").Find("Price").GetComponent<Text>().text = priceInGems;

            if (button.name == "Classic"
             || button.name == "Supreme"
             || button.name == "Maximum")
            {
                //If is bundle buttons
                title.LocalizedAsset = null;
                PurchaseAlert.Find("Content").Find("Alert").Find("Title").Find("Text").GetComponent<Text>().text = button.name;
                description.LocalizedAsset = (LocalizedText)Resources.Load("bundle_description", typeof(LocalizedText));

                description.FormatArgs = new string[1];

                if (button.name == "Classic")
                {
                    description.FormatArgs[0] = "1";
                }
                else if (button.name == "Supreme")
                {
                    description.FormatArgs[0] = "2";
                }
                else if (button.name == "Maximum")
                {
                    description.FormatArgs[0] = "3";
                }
            }
            else
            {
                //If is power-ups buttons
                title.LocalizedAsset = (LocalizedText)Resources.Load(button.name, typeof(LocalizedText));
                description.LocalizedAsset = (LocalizedText)Resources.Load(button.name + "_description", typeof(LocalizedText));
            }
        }

        PurchaseAlert.gameObject.SetActive(true);
    }

    public void ClosePurchaseAlert()
    {
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");
        StartCoroutine(uiController.ClosePanelAnimation(PurchaseAlert));
    }
}
