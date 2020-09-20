using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsController : MonoBehaviour
{
    private UIController uiController;
    private GameController gameController;
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
    }

    public void BoosterFilling_OneBottle()
    {
        int boosterFilling_OneBottle = PlayerPrefs.GetInt("PowerUp_BoosterFilling_OneBottle");

        if (boosterFilling_OneBottle > 0)
        {
            BoosterFilling_OneBottle_Flag = true;
            PlayerPrefs.SetInt("PowerUp_BoosterFilling_OneBottle", boosterFilling_OneBottle - 1);
            uiController.Panel_PowerUps_SetInteractable("BoosterFilling_OneBottle", false);
            uiController.Panel_PowerUps_SetInteractable("BoosterFilling_AllBottles", false);
            uiController.RefreshPowerUpsCount();
            uiController.ClosePanel(gameObject);
            uiController.InkBtn_BoosterFilling_Icon_SetActive(true);
        }
        else
        {
            print("You have to buy this power up!");
        }
    }

    public void BoosterFilling_AllBottles()
    {
        int boosterFilling_AllBottles = PlayerPrefs.GetInt("PowerUp_BoosterFilling_AllBottles");

        if (boosterFilling_AllBottles > 0)
        {
            BoosterFilling_OneBottle_Flag = true; // Activate the flag to fill one bottle faster
            BoosterFilling_AllBottles_Flag = true;
            PlayerPrefs.SetInt("PowerUp_BoosterFilling_AllBottles", boosterFilling_AllBottles - 1);
            uiController.Panel_PowerUps_SetInteractable("BoosterFilling_OneBottle", false);
            uiController.Panel_PowerUps_SetInteractable("BoosterFilling_AllBottles", false);
            uiController.RefreshPowerUpsCount();
            uiController.ClosePanel(gameObject);

            foreach (Transform bottle in GameObject.FindGameObjectWithTag("Rail").transform)
            {
                //Reset all Bottles to fill at the same time (ignoring the broken ones)
                if (!bottle.GetComponent<InkRepositoryController>().isBroken)
                {
                    bottle.GetComponent<InkRepositoryController>().inkfillAmount = 0f;
                    bottle.GetComponent<InkRepositoryController>().FillOrFixRepository();
                }
            }
        }
        else
        {
            print("You have to buy this power up!");
        }
    }

    public void BoosterFilling_Box()
    {
        int boosterFilling_OneBottle = PlayerPrefs.GetInt("PowerUp_BoosterFilling_Box");

        if (boosterFilling_OneBottle > 0)
        {
            BoosterFilling_Box_Flag = true;
            PlayerPrefs.SetInt("PowerUp_BoosterFilling_Box", boosterFilling_OneBottle - 1);
            uiController.Panel_PowerUps_SetInteractable("BoosterFilling_Box", false);
            uiController.RefreshPowerUpsCount();
            uiController.ClosePanel(gameObject);

            if (gameController.currentBox != null)
            {
                uiController.InkBtn_BoosterFillingBox_Icon_SetActive(true);
            }

            Water2D.Water2D_Spawner.instance.initSpeed = new Vector2(-0.15f, -10f);
            Water2D.Water2D_Spawner.instance.size = 0.25f;
            gameController.paintSpeed = 10f;
        }
        else
        {
            print("You have to buy this power up!");
        }
    }

    public void NoBrokenBottles()
    {
        int noBrokenBottles = PlayerPrefs.GetInt("PowerUp_NoBrokenBottles");

        if (noBrokenBottles > 0)
        {
            NoBrokenBottles_Flag = true;
            PlayerPrefs.SetInt("PowerUp_NoBrokenBottles", noBrokenBottles - 1);
            uiController.Panel_PowerUps_SetInteractable("NoBrokenBottles", false);
            uiController.RefreshPowerUpsCount();
            uiController.ClosePanel(gameObject);
            uiController.InkBtn_NoBrokenBottles_Icon_SetActive(true);
        }
        else
        {
            print("You have to buy this power up!");
        }
    }

    public void FixInTime()
    {
        int FixInTime = PlayerPrefs.GetInt("PowerUp_FixInTime");

        if (FixInTime > 0)
        {
            FixInTime_Flag = true;
            PlayerPrefs.SetInt("PowerUp_FixInTime", FixInTime - 1);
            uiController.Panel_PowerUps_SetInteractable("FixInTime", false);
            uiController.RefreshPowerUpsCount();
            uiController.ClosePanel(gameObject);
            uiController.InkBtn_FixInTime_Icon_SetActive(true);
        }
        else
        {
            print("You have to buy this power up!");
        }
    }

    public void DoubleCash()
    {
        int DoubleCash = PlayerPrefs.GetInt("PowerUp_DoubleCash");
        uiController.GetComponent<AudioSource>().Play();

        if (!DoubleCash_Flag)
        {
            if (DoubleCash > 0)
            {
                DoubleCash_Flag = true;
                //PlayerPrefs.SetInt("PowerUp_DoubleCash", DoubleCash - 1);
                uiController.DoubleCash_Icon_SetActive(true);
                powerUpButtons.transform.Find("DoubleCash").Find("BGCount").gameObject.SetActive(false);
                powerUpButtons.transform.Find("DoubleCash").Find("Check").gameObject.SetActive(true);
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

    public void FreezingTime()
    {
        int FreezingTime = PlayerPrefs.GetInt("PowerUp_FreezingTime");

        if (FreezingTime > 0)
        {
            FreezingTime_Flag = true;
            PlayerPrefs.SetInt("PowerUp_FreezingTime", FreezingTime - 1);
            uiController.Panel_PowerUps_SetInteractable("FreezingTime", false);
            uiController.RefreshPowerUpsCount();
            uiController.ClosePanel(gameObject);

            //Freeze time, then start the countdown
            uiController.timerIsRunning = false;

            //Activating this gameObject will start the FreezingTime script (that controls the bar)
            uiController.FreezingTime_Icon_SetActive(true);
        }
        else
        {
            print("You have to buy this power up!");
        }
    }
}
