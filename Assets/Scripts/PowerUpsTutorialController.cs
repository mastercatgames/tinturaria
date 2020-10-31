using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpsTutorialController : MonoBehaviour
{
    private UIController uiController;
    public int step;
    public Transform PowerUpsButtons;
    private Transform powerUpButton;

    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        //TODO: Change the name of this key to "PowerUps_Unlocked"
        step = PlayerPrefs.GetInt("PowerUpsTutorial_Step");

        Invoke("NextStep", 0.5f);
    }

    public void NextStep()
    {
        // step++;

        transform.Find("Step-" + (step + 1)).gameObject.SetActive(true);

        powerUpButton = PowerUpsButtons.GetChild(step);
        powerUpButton.GetComponent<Canvas>().sortingOrder = 501;
        powerUpButton.GetComponent<Button>().interactable = true;

        // PowerUpsButtons.parent.Find("TapToPlayBtn").gameObject.SetActive(false);
        

        // if (step == 1)
        // {
        //     PowerUpsButtons.GetChild()
        //     PowerUpsButtons.Find("DoubleCash").GetComponent<Canvas>().sortingOrder = 501;
        // }
        // else if (step == 2)
        // {
        //     PowerUpsButtons.Find("NoBrokenBottles").GetComponent<Canvas>().sortingOrder = 501;
        // }
        // else if (step == 3)
        // {
        //     PowerUpsButtons.Find("BoosterFilling_OneBottle").GetComponent<Canvas>().sortingOrder = 501;
        // }
        // else if (step == 4)
        // {    
        //     PowerUpsButtons.Find("BoosterFilling_AllBottles").GetComponent<Canvas>().sortingOrder = 501;
        // }
        // else if (step == 5)
        // { 
        //     PowerUpsButtons.Find("FixInTime").GetComponent<Canvas>().sortingOrder = 501;
        // }
        // else if (step == 6)
        // { 
        //     PowerUpsButtons.Find("BoosterFilling_Box").GetComponent<Canvas>().sortingOrder = 501;
        // }
    }

    public void HighlightTapToPlay()
    {
        // powerUpButton.GetComponent<Canvas>().sortingOrder = 500;
        powerUpButton.GetComponent<Button>().interactable = false;
        transform.Find("Step-" + (step + 1)).gameObject.SetActive(false);
        PowerUpsButtons.parent.Find("TapToPlayBtn").gameObject.SetActive(true);
    }
}
