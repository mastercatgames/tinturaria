using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToPlay : MonoBehaviour
{
    private UIController uiController;
    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
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
        uiController.readyGo.gameObject.SetActive(false);
        uiController.ShowAllGameplayObjects();
        uiController.timerIsRunning = true;
        uiController.isInGamePlay = true;                
    }
}
