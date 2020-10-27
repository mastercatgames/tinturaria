using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameToolkit.Localization;

public class TutorialController : MonoBehaviour
{
    // Start is called before the first frame update
    private UIController uiController;
    public int step;
    public Canvas RequestPanel;
    public Canvas FormButton;
    public Canvas InkButton;
    public GameObject Panel_Forms;
    public GameObject Panel_Ink_Buckets;
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        // step = 1;
        //ShowStep();
        ShowWelcome1();
        Invoke("FreezeTime", 2f);  

        uiController.blockSwipe = true;
        uiController.blockRightSwipe = true;
        uiController.blockPainting = true;
    }

    private void ShowStep()
    {
        step++;
        if (step == 1)
        {
            //Show Request Panel
            RequestPanel.sortingOrder = 501;
            transform.Find("Step-1").gameObject.SetActive(true);
        }
        else if (step == 2)
        {
            //Tap on Forms button (to open boxes panel)
            RequestPanel.sortingOrder = 0;
            FormButton.sortingOrder = 501;
            transform.Find("Step-1").gameObject.SetActive(false);
            transform.Find("Step-2").gameObject.SetActive(true);
            uiController.transform.parent.Find("ButtonsGrid").gameObject.SetActive(true);
        }
        else if (step == 3)
        {
            //Show Choose the box
            Panel_Forms.transform.Find("Close_Button").gameObject.SetActive(false);
            Panel_Forms.transform.Find("Forms").Find("Moon").GetComponent<Canvas>().sortingOrder = 505;
            FormButton.sortingOrder = 0;
            uiController.OpenPanel(Panel_Forms);
            transform.Find("Step-2").gameObject.SetActive(false);
            transform.Find("Step-3").gameObject.SetActive(true);
        }
        else if (step == 4)
        {
            //Box was choosed show next step (5)
            // NormalizeTime();
            
            transform.Find("GrayBackground").gameObject.SetActive(false);
            transform.Find("Step-3").gameObject.SetActive(false);
            uiController.CallChangeCurrentBox(Panel_Forms.transform.Find("Forms").Find("Moon").gameObject);
            uiController.transform.parent.Find("ButtonsGrid").gameObject.SetActive(false);
            // step++;
            //Show step 5 after 1 second (to give time to box falling animaiton)
            Invoke("ShowStep", 1f);            
            
        }
        else if (step == 5)
        {
            //Show Swipe to green bottle
            print("Show Step 5!");
            uiController.transform.parent.Find("ButtonsGrid").gameObject.SetActive(true);
            uiController.blockSwipe = false;
            transform.Find("Step-5").gameObject.SetActive(true);
        }
        else if (step == 6)
        {
            //Show Tap here to fill the box
            print("Show Step 6!");
            uiController.blockSwipe = true;
            uiController.blockPainting = false;
            // uiController.blockRightSwipe = false;
            transform.Find("Step-5").gameObject.SetActive(false);
            transform.Find("Step-6").gameObject.SetActive(true);
        }
        // else if (step == 7)
        // {
        //     //Show only highlight until fill box
        //     // print("Show Step 7!");
        //     // transform.Find("Step-6").gameObject.SetActive(false);
        //     // transform.Find("Step-7").gameObject.SetActive(true);
        //     //Call next step to inform to reload
        //     //nvoke("ShowStep", 1f); 
        // }
        else if (step == 7)
        {
            //Show Oops! It's look like you are out of ink...Let's fill it!
            print("Show Step 7!");
            transform.Find("Step-6").gameObject.SetActive(false);
            transform.Find("Step-7").gameObject.SetActive(true);
        }
        else if (step == 8)
        {
            //Show Tap here to reload bottles
            transform.Find("GrayBackground").gameObject.SetActive(true);
            print("Show Step 8!");
            InkButton.sortingOrder = 501;
            transform.Find("Step-7").gameObject.SetActive(false);
            transform.Find("Step-8").gameObject.SetActive(true);
        }
        else if (step == 9)
        {
            //Show choose the requested box
            print("Show Step 9!");
            Panel_Ink_Buckets.transform.Find("Close_Button").gameObject.SetActive(false);
            FormButton.sortingOrder = 0;
            InkButton.sortingOrder = 0;
            uiController.OpenPanel(Panel_Ink_Buckets);
            Panel_Ink_Buckets.transform.Find("Buckets").Find("Green").GetComponent<Canvas>().sortingOrder = 505;
            transform.Find("Step-8").gameObject.SetActive(false);
            transform.Find("Step-9").gameObject.SetActive(true);
        }
        else if (step == 10)
        {
            //Show Notice that the bottle will start to fill.      
            transform.Find("GrayBackground").gameObject.SetActive(false);   
            print("Show Step 10!");
            FormButton.sortingOrder = 0;
            InkButton.sortingOrder = 0;
            uiController.CallFillOrFixRepository(Panel_Ink_Buckets.transform.Find("Buckets").Find("Green").gameObject);
            // Panel_Ink_Buckets.transform.Find("Buckets").Find("Green").GetComponent<Canvas>().sortingOrder = 505;
            transform.Find("Step-9").gameObject.SetActive(false);
            transform.Find("Step-10").gameObject.SetActive(true);

            Invoke("ShowStep", 7f);  
        }
        else if (step == 11)
        {
            //Show Now, fill the box to the end before the time runs out!      
            NormalizeTime();    
            print("Show Step 11!");
            transform.Find("Step-10").gameObject.SetActive(false);
            transform.Find("Step-11").gameObject.SetActive(true);
        }
    }

    public void FreezeTime()
    {
        // Time.timeScale = 0f;
        uiController.timerIsRunning = false;
        print("Freeze Time!");
    }

    public void NormalizeTime()
    {
        uiController.timerIsRunning = true;
        print("Normalize Time!");
    }

    public void ShowTapHereAgain()
    {
        transform.Find("Step-6").Find("Dialog").gameObject.SetActive(true);
        transform.Find("Step-6").Find("HandIcon").gameObject.SetActive(true);
        transform.Find("Step-6").Find("Dialog").Find("Text").GetComponent<LocalizedTextBehaviour>().LocalizedAsset = (LocalizedText)Resources.Load("tap_here_again", typeof(LocalizedText));
    }

    public void ShowHandIcon()
    {
        transform.Find("Step-11").Find("HandIcon").gameObject.SetActive(true);
    }

    public void ShowWelcome1()
    {
        StartCoroutine(uiController.SetActiveAfterTime(transform.Find("WelcomeAlert").gameObject, true, 0.2f));
    }

    public void ShowWelcome2()
    {
        transform.Find("WelcomeAlert").gameObject.SetActive(false);

        Transform alert = transform.Find("WelcomeAlert").Find("Content").Find("Alert");

        alert.Find("Title").gameObject.SetActive(false);
        alert.Find("BG").Find("FakeMask").gameObject.SetActive(false);
        alert.Find("Description").GetComponent<LocalizedTextBehaviour>().LocalizedAsset = (LocalizedText)Resources.Load("welcome_description_2", typeof(LocalizedText));

        Button okButton = alert.Find("Buttons").Find("Ok").GetComponent<Button>();

        UnityEditor.Events.UnityEventTools.RemovePersistentListener(okButton.onClick, 0);
        okButton.onClick.AddListener(StartTutorial);
        StartCoroutine(uiController.SetActiveAfterTime(transform.Find("WelcomeAlert").gameObject, true, 0.4f));
    }

    public void StartTutorial()
    {
        transform.Find("WelcomeAlert").gameObject.SetActive(false);
        ShowStep();
    }

    public void NextStep()
    {
        // step++;
        ShowStep();
    }
}
