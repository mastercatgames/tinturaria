using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsController : MonoBehaviour
{
    private UIController uiController;
    private GameController gameController;
    public bool BoosterFilling_OneBottle_Flag;
    public bool BoosterFilling_Box_Flag;
    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
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
            gameController.paintSpeed = 2f;
        }
        else
        {
            print("You have to buy this power up!");
        }
    }
}
