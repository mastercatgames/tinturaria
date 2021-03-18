using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameToolkit.Localization;
using UnityEngine.Purchasing;
using UnityEngine.Advertisements;

public class StoreController : MonoBehaviour
{
    private UIController uiController;
    public Transform RepairsButtons, PowerUpButtons, BundleButtons, GemsButtons, PurchaseAlert;
    private string[] repairsPricesCoins;
    private int[] repairsPricesGems, powerUpsPrices, bundlesPrices;

    [Header("===== Selected Item in Shop =====")]
    public string selectedCurrency, selectedItemName, selectedItemQty;
    public int selectedItemPriceCoins, selectedItemPriceGems;
    public GameObject selectedButton;

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
        RefreshGemsPanel();
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
    public void RefreshGemsPanel()
    {
        if (PlayerPrefs.GetInt("removeAds") == 1)
        {
            string productId = "";
            foreach (Transform button in GemsButtons)
            {
                productId = button.GetComponent<IAPButton>().productId;
                button.GetComponent<IAPButton>().productId = productId.Substring(productId.IndexOf('g'));
                button.Find("NoAdsIcon").gameObject.SetActive(false);
            }
        }
    }

    public void OpenPurchaseAlert(GameObject button)
    {
        LocalizedTextBehaviour title = PurchaseAlert.Find("Content").Find("Alert").Find("Title").Find("Text").GetComponent<LocalizedTextBehaviour>();
        LocalizedTextBehaviour description = PurchaseAlert.Find("Content").Find("Alert").Find("Description").GetComponent<LocalizedTextBehaviour>();

        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");

        Transform buttons = PurchaseAlert.Find("Content").Find("Alert").Find("Buttons");

        ShowPurchaseButtons();

        selectedItemName = button.name;
        selectedItemQty = "1";
        selectedButton = button;

        selectedItemPriceGems = int.Parse(button.transform.Find("PriceButton").Find("Price").GetComponent<Text>().text);
        buttons.Find("PriceButtonGems").Find("Price").GetComponent<Text>().text = selectedItemPriceGems.ToString();

        if (button.name.Contains("Tool"))
        {
            title.LocalizedAsset = (LocalizedText)Resources.Load("repair_tool_qty", typeof(LocalizedText));
            description.LocalizedAsset = (LocalizedText)Resources.Load("repair_tool_description", typeof(LocalizedText));
            selectedItemPriceCoins = int.Parse(button.transform.Find("PriceButtonCoins").Find("Price").GetComponent<Text>().text);
            buttons.Find("PriceButtonCoins").Find("Price").GetComponent<Text>().text = selectedItemPriceCoins.ToString();

            title.FormatArgs = new string[1];
            selectedItemQty = button.name.Split('_')[1];
            title.FormatArgs[0] = "(x" + selectedItemQty + ")";

            buttons.Find("PriceButtonCoins").gameObject.SetActive(true);
        }
        else
        {
            //If is the Power Ups or Bundles panel...
            buttons.Find("PriceButtonCoins").gameObject.SetActive(false);

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
                    description.FormatArgs[0] = selectedItemQty = "1";
                }
                else if (button.name == "Supreme")
                {
                    description.FormatArgs[0] = selectedItemQty = "2";
                }
                else if (button.name == "Maximum")
                {
                    description.FormatArgs[0] = selectedItemQty = "3";
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

        selectedItemName = "";
        selectedItemPriceCoins = 0;
        selectedItemPriceGems = 0;
        selectedItemQty = "";
        selectedCurrency = "";
    }

    public void BuyItem(string currency)
    {
        int credits = PlayerPrefs.GetInt(currency + "Count");
        int selectedItemPrice = 0;
        selectedCurrency = currency;

        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");

        if (currency == "coins")
            selectedItemPrice = selectedItemPriceCoins;
        else if (currency == "gems")
            selectedItemPrice = selectedItemPriceGems;

        //If has credits
        if (credits >= selectedItemPrice)
        {
            //Payment
            PlayerPrefs.SetInt(currency + "Count", PlayerPrefs.GetInt(currency + "Count") - selectedItemPrice);

            //Verify which item is...
            if (selectedItemName.Contains("Tool"))
            {
                AddTools();
                CallYouHaveAnim();
            }
            else if (selectedItemName == "Classic"
             || selectedItemName == "Supreme"
             || selectedItemName == "Maximum")
            {
                AddBundle();
            }
            else
            {
                AddPowerUps(selectedItemName);
                CallYouHaveAnim();
            }

            uiController.RefreshUIToolsAndMoney();
            ClosePurchaseAlert();
            RefreshRapairsPanel();
            RefreshPowerUpPanel();

            ParticleSystem particles = selectedButton.transform.Find("Sparkle").GetComponent<ParticleSystem>();
            particles.Stop();
            if (particles.isStopped)
            {
                particles.Play();
            }
            GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("coinsPurchase");

            //print("Buy item with " + currency + "\nItem: " + selectedItemName + "\n selectedItemPriceCoins: " + selectedItemPriceCoins + "\nselectedItemPriceGems: " + selectedItemPriceGems + "\nselectedItemQty: " + selectedItemQty);
        }
        else
        {
            StartCoroutine(uiController.ClosePanelAnimation(PurchaseAlert));
            Invoke("ShowNoCreditsAlert", 0.6f);
        }
    }

    private void AddTools()
    {
        int itemQty = int.Parse(selectedItemQty);
        PlayerPrefs.SetInt("toolsCount", PlayerPrefs.GetInt("toolsCount") + (1 * itemQty));
    }

    private void AddPowerUps(string powerUpName)
    {
        int itemQty = int.Parse(selectedItemQty);
        PlayerPrefs.SetInt("PowerUp_" + powerUpName, PlayerPrefs.GetInt("PowerUp_" + powerUpName) + (1 * itemQty));
    }

    private void AddBundle()
    {
        int itemQty = int.Parse(selectedItemQty);

        foreach (Transform powerUpName in PowerUpButtons)
        {
            AddPowerUps(powerUpName.name);
        }
    }

    private void CallYouHaveAnim()
    {
        selectedButton.transform.Find("YouHave").GetComponent<Animator>().Play("UI_JellyZoom");
        // StartCoroutine(PlayAnimationAfterTime(star3.GetComponent<Animator>(), "UI_JellyZoom", 1.6f));
    }

    private void ShowNoCreditsAlert()
    {
        LocalizedTextBehaviour title = PurchaseAlert.Find("Content").Find("Alert").Find("Title").Find("Text").GetComponent<LocalizedTextBehaviour>();
        LocalizedTextBehaviour description = PurchaseAlert.Find("Content").Find("Alert").Find("Description").GetComponent<LocalizedTextBehaviour>();

        title.LocalizedAsset = (LocalizedText)Resources.Load("no_enough_" + selectedCurrency, typeof(LocalizedText));
        description.LocalizedAsset = (LocalizedText)Resources.Load("would_you_like_to_buy_gems", typeof(LocalizedText));

        ShowOkToGemsButton();

        PurchaseAlert.gameObject.SetActive(true);
    }

    public void ScrollToGems()
    {
        ClosePurchaseAlert();
        transform.Find("ScrollRectVertical").GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
    }

    private void ShowPurchaseButtons()
    {
        Transform buttons = PurchaseAlert.Find("Content").Find("Alert").Find("Buttons");

        foreach (Transform item in buttons)
        {
            item.gameObject.SetActive(true);
            if (item.name == "OkToGems")
                item.gameObject.SetActive(false);
        }
    }

    private void ShowOkToGemsButton()
    {
        Transform buttons = PurchaseAlert.Find("Content").Find("Alert").Find("Buttons");

        foreach (Transform item in buttons)
        {
            item.gameObject.SetActive(false);
            if (item.name == "OkToGems")
                item.gameObject.SetActive(true);
        }
    }

    public void RemoveAds()
    {
        PlayerPrefs.SetInt("removeAds", 1);
        Advertisement.Banner.Hide();
    }

    public void RestoreProduct(Product product)
    {
        //Calls when user reinstall the app
        if (product.definition.id.Contains("no_ads"))
        {
            string productId = product.definition.id;
            int gemsAmount = int.Parse(productId.Substring(productId.IndexOf('x') + 1));
            RemoveAds();            
            PlayerPrefs.SetInt("gemsCount", gemsAmount); 
        }
    }

    public void BuyGems(Product product)
    {
        uiController.SetLoading(true);
        string productId = product.definition.id;
        int gemsAmount = int.Parse(productId.Substring(productId.IndexOf('x') + 1));

        LocalizedText titleLocalized = (LocalizedText)Resources.Load("thank_you", typeof(LocalizedText));
        LocalizedText descriptionLocalized;

        if (productId.Contains("no_ads"))
        {            
            RemoveAds(); 
            descriptionLocalized = (LocalizedText)Resources.Load("gems_purchased_no_ads", typeof(LocalizedText));
        }
        else
        {
            descriptionLocalized = (LocalizedText)Resources.Load("gems_purchased", typeof(LocalizedText));
        }

        PlayerPrefs.SetInt("gemsCount", PlayerPrefs.GetInt("gemsCount") + gemsAmount);
        uiController.SetLoading(false);
        uiController.RefreshUIToolsAndMoney();
        RefreshGemsPanel();

        uiController.OpenGenericAlert(titleLocalized, descriptionLocalized, gemsAmount);       
    }

    public void PurchaseFailedFeedback(Product product, PurchaseFailureReason reason)
    {
        uiController.SetLoading(false);
        uiController.unityAds.ShowSkippableVideoAd();

        LocalizedText titleLocalized = (LocalizedText)Resources.Load("error", typeof(LocalizedText));
        LocalizedText descriptionLocalized = (LocalizedText)Resources.Load("purchase_failed", typeof(LocalizedText));

        uiController.OpenGenericAlert(titleLocalized, descriptionLocalized);        
    }
}
