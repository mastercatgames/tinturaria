using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreezingTime : MonoBehaviour
{
    public Image bar;
    private UIController uiController;
    private RequestPanelController requestPanelController;

    public float MaxTime = 20f;
    public float ActiveTime = 0f;
    public float percent = 0f;
    
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        requestPanelController = uiController.transform.parent.Find("RequestPanel").GetComponent<RequestPanelController>();
        bar = transform.Find("BarBG").Find("Bar").GetComponent<Image>();
    }

    void Update()
    {
        if (bar.fillAmount > 0f)
        {
            ActiveTime += Time.deltaTime;
            percent = ActiveTime / MaxTime;
            bar.fillAmount = Mathf.Lerp(1, 0, percent);

            if (bar.fillAmount < 0.5f)
            {
                bar.color = requestPanelController.YellowColor;
            }

            if (bar.fillAmount < 0.2f)
            {
                bar.color = requestPanelController.RedColor;
            }
        }
        else
        {
            uiController.timerIsRunning = true;
            uiController.FreezingTime_Icon_SetActive(false);
            uiController.Panel_PowerUps_SetInteractable("FreezingTime", true);
            bar.fillAmount = 1f;
            bar.color = requestPanelController.GreenColor;
            ActiveTime = 0f;
            percent = 0f;
            print("FreezingTime is Over!");
        }
    }
}
