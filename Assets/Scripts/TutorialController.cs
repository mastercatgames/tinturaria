using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    // Start is called before the first frame update
    private UIController uiController;
    public int step;
    public Canvas RequestPanel;
    public Canvas FormButton;
    public GameObject Panel_Forms;
    public GameObject Panel_Ink_Buckets;
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        step = 1;
        ShowStep();      
        Invoke("FreezeTime", 2f);  
    }

    void ShowStep()
    {
        if (step == 1)
        {
            RequestPanel.sortingOrder = 501;
        }
        else if (step == 2)
        {
            RequestPanel.sortingOrder = 0;
            FormButton.sortingOrder = 501;
            transform.Find("Step-1").gameObject.SetActive(false);
            transform.Find("Step-2").gameObject.SetActive(true);
        }
        else if (step == 3)
        {
            transform.Find("GrayBackground").gameObject.SetActive(false);
            Panel_Forms.transform.Find("Close_Button").gameObject.SetActive(false);
            uiController.OpenPanel(Panel_Forms);
            transform.Find("Step-2").gameObject.SetActive(false);
            transform.Find("Step-3").gameObject.SetActive(true);
        }
        else if (step == 4)
        {
            Time.timeScale = 1f;
            transform.Find("Step-3").gameObject.SetActive(false);
            transform.Find("Step-4").gameObject.SetActive(true);
        }
        else if (step == 5)
        {
            
        }
    }

    public void FreezeTime()
    {
        Time.timeScale = 0f;
        print("Freeze Time!");
    }

    public void NextStep()
    {
        step++;
        ShowStep();
    }
}
