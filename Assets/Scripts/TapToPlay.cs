using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameToolkit.Localization;

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
        LocalizedTextBehaviour goTranslate = uiController.readyGo.GetComponent<LocalizedTextBehaviour>();
        goTranslate.LocalizedAsset = (LocalizedText)Resources.Load("Go", typeof(LocalizedText));
        uiController.readyGo.gameObject.SetActive(true);
    }

    public void ShowReady()
    {        
        uiController.readyGo.gameObject.SetActive(true);
        StartCoroutine(uiController.PlayAnimationAfterTime(uiController.readyGo.GetComponent<Animator>(), "UI_JellyZoomOut_Auto", 2f, 1f));
        StartCoroutine(uiController.SetActiveAfterTime(uiController.readyGo.gameObject, false, 3f));
    }

    public void StartGame()
    {
        //Shoot this event at the end of the ZoonInkMachine animation
        uiController.readyGo.gameObject.SetActive(false);
        uiController.ShowAllGameplayObjects();
        uiController.isInGamePlay = true;        

        if (uiController.isTutorial)
        {
            print(uiController.transform.parent.Find("Tutorial").gameObject);
            StartCoroutine(uiController.ShowGameObjectAfterTime(uiController.transform.parent.Find("Tutorial").gameObject, 0.5f));
        }

        uiController.timerIsRunning = true;       
    }
}
