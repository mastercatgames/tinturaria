using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTutorialController : MonoBehaviour
{
    private UIController uiController;
    public int step;
    // public Canvas RequestPanel;
    // public Canvas FormButton;
    public Canvas toolUI;
    public Canvas InkButton;
    // public GameObject Panel_Forms;
    public GameObject Panel_Ink_Buckets;
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();

        Invoke("NextStep", 0.5f);

        uiController.blockSwipe = true;
        uiController.blockPainting = true;
    }

    public void NextStep()
    {
        step++;

        if (step == 1)
        {
            FreezeTime();
            print("Show Step 1!");
            transform.Find("GrayBackground").gameObject.SetActive(false);
            transform.Find("Step-1").gameObject.SetActive(true);
        }
        else if (step == 2)
        {
            transform.Find("GrayBackground").gameObject.SetActive(true);
            print("Show Step 2!");
            InkButton.sortingOrder = 501;
            transform.Find("Step-1").gameObject.SetActive(false);
            transform.Find("Step-2").gameObject.SetActive(true);
        }
        else if (step == 3)
        {
            print("Show Step 3!");
            Panel_Ink_Buckets.transform.Find("Close_Button").gameObject.SetActive(false);
            InkButton.sortingOrder = 0;
            uiController.OpenPanel(Panel_Ink_Buckets);
            Panel_Ink_Buckets.transform.Find("Buckets").Find("Blue").GetComponent<Canvas>().sortingOrder = 505;
            transform.Find("Step-2").gameObject.SetActive(false);
            transform.Find("Step-3").gameObject.SetActive(true);
        }
        else if (step == 4)
        {    
            transform.Find("GrayBackground").gameObject.SetActive(false);   
            Panel_Ink_Buckets.transform.Find("Close_Button").gameObject.SetActive(true);
            print("Show Step 4!");
            InkButton.sortingOrder = 0;
            uiController.CallFillOrFixRepository(Panel_Ink_Buckets.transform.Find("Buckets").Find("Blue").gameObject);
            Panel_Ink_Buckets.transform.Find("Buckets").Find("Blue").GetComponent<Canvas>().sortingOrder = 505;
            transform.Find("Step-3").gameObject.SetActive(false);
            transform.Find("Step-4").gameObject.SetActive(true);

            Invoke("NextStep", 6f);  
        }
        else if (step == 5)
        { 
            print("Show Step 5 (Done)!");
            transform.Find("Step-4").gameObject.SetActive(false);
            transform.Find("DoneAlert").gameObject.SetActive(true);
        }
    }

    public void FreezeTime()
    {
        uiController.timerIsRunning = false;
        print("Freeze Time!");
    }

    public void NormalizeTime()
    {
        uiController.timerIsRunning = true;
        print("Normalize Time!");
    }

    public void FinishTutorial()
    {
        NormalizeTime();
        transform.Find("DoneAlert").gameObject.SetActive(false);
        uiController.blockSwipe = false;
        uiController.blockPainting = false;
        transform.parent.Find("RequestPanel").GetComponent<RequestPanelController>().InvokeRepeating("RequestBox", 15f, 15f);
    }
}
