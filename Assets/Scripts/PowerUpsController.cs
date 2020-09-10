using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsController : MonoBehaviour
{
    private UIController uiController;
    private GameController gameController;
    public bool BoosterFilling_OneBottle_Flag;
    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    
    public void BoosterFilling_OneBottle()
    {
        int boosterFilling_OneBottle = PlayerPrefs.GetInt("PowerUp_BoosterFilling_OneBottle");
        PlayerPrefs.SetInt("PowerUp_BoosterFilling_OneBottle", boosterFilling_OneBottle - 1);

        uiController.InkBtn_BoosterFilling_Icon_SetActive(true);

        BoosterFilling_OneBottle_Flag = true;

        uiController.RefreshPowerUpsCount();
        uiController.ClosePanel(gameObject);
    }    
}
