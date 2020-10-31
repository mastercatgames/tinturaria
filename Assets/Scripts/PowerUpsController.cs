using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsController : MonoBehaviour
{
    private UIController uiController;
    private GameController gameController;
    private PowerUpsTutorialController powerUpsTutorialController;
    public bool BoosterFilling_OneBottle_Flag;
    public bool BoosterFilling_AllBottles_Flag;
    public bool BoosterFilling_Box_Flag;
    public bool NoBrokenBottles_Flag;
    public bool FixInTime_Flag;
    public bool DoubleCash_Flag;
    public bool FreezingTime_Flag;
    public GameObject powerUpButtons;
    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        gameController = GameObject.Find("Gameplay").transform.Find("GameController").GetComponent<GameController>();
        powerUpsTutorialController = uiController.transform.parent.Find("PowerUpsTutorial").GetComponent<PowerUpsTutorialController>();
    }

    public void BoosterFilling_OneBottle()
    {
        int boosterFilling_OneBottle = PlayerPrefs.GetInt("PowerUp_BoosterFilling_OneBottle");
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");

        if (!BoosterFilling_OneBottle_Flag)
        {
            if (boosterFilling_OneBottle > 0 || uiController.isPowerUpTutorial)
            {
                BoosterFilling_OneBottle_Flag = true;
                powerUpButtons.transform.Find("BoosterFilling_OneBottle").Find("BGCount").gameObject.SetActive(false);
                powerUpButtons.transform.Find("BoosterFilling_OneBottle").Find("Check").gameObject.SetActive(true);

                if (uiController.isPowerUpTutorial)
                {
                    powerUpsTutorialController.HighlightTapToPlay();
                }
            }
            else
            {
                print("You have to buy this power up!");
            }
        }
        else
        {
            BoosterFilling_OneBottle_Flag = false;
            powerUpButtons.transform.Find("BoosterFilling_OneBottle").Find("BGCount").gameObject.SetActive(true);
            powerUpButtons.transform.Find("BoosterFilling_OneBottle").Find("Check").gameObject.SetActive(false);
        }
    }

    public void BoosterFilling_AllBottles()
    {
        int BoosterFilling_AllBottles = PlayerPrefs.GetInt("PowerUp_BoosterFilling_AllBottles");
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");

        if (!BoosterFilling_AllBottles_Flag)
        {
            if (BoosterFilling_AllBottles > 0 || uiController.isPowerUpTutorial)
            {
                BoosterFilling_AllBottles_Flag = true;
                powerUpButtons.transform.Find("BoosterFilling_AllBottles").Find("BGCount").gameObject.SetActive(false);
                powerUpButtons.transform.Find("BoosterFilling_AllBottles").Find("Check").gameObject.SetActive(true);

                if (uiController.isPowerUpTutorial)
                {
                    powerUpsTutorialController.HighlightTapToPlay();
                }
            }
            else
            {
                print("You have to buy this power up!");
            }
        }
        else
        {
            BoosterFilling_AllBottles_Flag = false;
            powerUpButtons.transform.Find("BoosterFilling_AllBottles").Find("BGCount").gameObject.SetActive(true);
            powerUpButtons.transform.Find("BoosterFilling_AllBottles").Find("Check").gameObject.SetActive(false);
        }
    }

    public void BoosterFilling_Box()
    {
        int BoosterFilling_Box = PlayerPrefs.GetInt("PowerUp_BoosterFilling_Box");
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");

        if (!BoosterFilling_Box_Flag)
        {
            if (BoosterFilling_Box > 0 || uiController.isPowerUpTutorial)
            {
                BoosterFilling_Box_Flag = true;
                powerUpButtons.transform.Find("BoosterFilling_Box").Find("BGCount").gameObject.SetActive(false);
                powerUpButtons.transform.Find("BoosterFilling_Box").Find("Check").gameObject.SetActive(true);

                if (uiController.isPowerUpTutorial)
                {
                    powerUpsTutorialController.HighlightTapToPlay();
                }
            }
            else
            {
                print("You have to buy this power up!");
            }
        }
        else
        {
            BoosterFilling_Box_Flag = false;
            powerUpButtons.transform.Find("BoosterFilling_Box").Find("BGCount").gameObject.SetActive(true);
            powerUpButtons.transform.Find("BoosterFilling_Box").Find("Check").gameObject.SetActive(false);
        }
    }

    public void NoBrokenBottles()
    {
        int noBrokenBottles = PlayerPrefs.GetInt("PowerUp_NoBrokenBottles");
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");

        if (!NoBrokenBottles_Flag)
        {
            if (noBrokenBottles > 0 || uiController.isPowerUpTutorial)
            {
                NoBrokenBottles_Flag = true;
                powerUpButtons.transform.Find("NoBrokenBottles").Find("BGCount").gameObject.SetActive(false);
                powerUpButtons.transform.Find("NoBrokenBottles").Find("Check").gameObject.SetActive(true);

                if (uiController.isPowerUpTutorial)
                {
                    powerUpsTutorialController.HighlightTapToPlay();
                }
            }
            else
            {
                print("You have to buy this power up!");
            }
        }
        else
        {
            NoBrokenBottles_Flag = false;
            powerUpButtons.transform.Find("NoBrokenBottles").Find("BGCount").gameObject.SetActive(true);
            powerUpButtons.transform.Find("NoBrokenBottles").Find("Check").gameObject.SetActive(false);
        }
    }

    public void FixInTime()
    {
        int FixInTime = PlayerPrefs.GetInt("PowerUp_FixInTime");
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");

        if (!FixInTime_Flag)
        {
            if (FixInTime > 0 || uiController.isPowerUpTutorial)
            {
                FixInTime_Flag = true;
                powerUpButtons.transform.Find("FixInTime").Find("BGCount").gameObject.SetActive(false);
                powerUpButtons.transform.Find("FixInTime").Find("Check").gameObject.SetActive(true);

                if (uiController.isPowerUpTutorial)
                {
                    powerUpsTutorialController.HighlightTapToPlay();
                }
            }
            else
            {
                print("You have to buy this power up!");
            }
        }
        else
        {
            FixInTime_Flag = false;
            powerUpButtons.transform.Find("FixInTime").Find("BGCount").gameObject.SetActive(true);
            powerUpButtons.transform.Find("FixInTime").Find("Check").gameObject.SetActive(false);
        }
    }

    public void DoubleCash()
    {
        int DoubleCash = PlayerPrefs.GetInt("PowerUp_DoubleCash");
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");

        if (!DoubleCash_Flag)
        {
            if (DoubleCash > 0 || uiController.isPowerUpTutorial)
            {
                DoubleCash_Flag = true;
                powerUpButtons.transform.Find("DoubleCash").Find("BGCount").gameObject.SetActive(false);
                powerUpButtons.transform.Find("DoubleCash").Find("Check").gameObject.SetActive(true);

                if (uiController.isPowerUpTutorial)
                {
                    powerUpsTutorialController.HighlightTapToPlay();
                }
            }
            else
            {
                print("You have to buy this power up!");
            }
        }
        else
        {
            DoubleCash_Flag = false;
            powerUpButtons.transform.Find("DoubleCash").Find("BGCount").gameObject.SetActive(true);
            powerUpButtons.transform.Find("DoubleCash").Find("Check").gameObject.SetActive(false);
        }
    }

    // public void FreezingTime()
    // {
    //     int FreezingTime = PlayerPrefs.GetInt("PowerUp_FreezingTime");

    //     if (FreezingTime > 0)
    //     {
    //         FreezingTime_Flag = true;
    //         PlayerPrefs.SetInt("PowerUp_FreezingTime", FreezingTime - 1);
    //         uiController.Panel_PowerUps_SetInteractable("FreezingTime", false);
    //         uiController.RefreshPowerUpsCount();
    //         uiController.ClosePanel(gameObject);

    //         //Freeze time, then start the countdown
    //         uiController.timerIsRunning = false;

    //         //Activating this gameObject will start the FreezingTime script (that controls the bar)
    //         uiController.FreezingTime_Icon_SetActive(true);
    //     }
    //     else
    //     {
    //         print("You have to buy this power up!");
    //     }
    // }
}
