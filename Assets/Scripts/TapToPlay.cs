using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToPlay : MonoBehaviour
{
    private UIController uiController;
    private PowerUpsController powerUpsController;
    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        powerUpsController = uiController.transform.parent.Find("PowerUps").GetComponent<PowerUpsController>();
    }

    public void ShowGo()
    {
        uiController.readyGo.text = "Go!";
    }

    public void ShowReady()
    {
        uiController.readyGo.text = "Ready?";
    }

    public void StartGame()
    {
        //Shoot this event at the end of the ZoonInkMachine animation
        uiController.readyGo.gameObject.SetActive(false);
        uiController.ShowAllGameplayObjects();
        uiController.isInGamePlay = true;

        if (!uiController.isTutorial)
        {
            uiController.timerIsRunning = true;
        }        
    }
}
