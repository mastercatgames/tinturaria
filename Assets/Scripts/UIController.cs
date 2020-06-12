using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject inkPanel;
    public GameObject formsPanel;
    public GameObject ButtonsGrid;

    public void OpenPanel(string panel)
    {        
        if (panel == "ink")
        {            
            inkPanel.SetActive(true);                       
        }

        if (panel == "form")
        {            
            formsPanel.SetActive(true);                       
        }

        ButtonsGrid.SetActive(false);
    }

    public void ClosePanel(string panel)
    {        
        if (panel == "ink")
        {            
            inkPanel.SetActive(false);                       
        }

        if (panel == "form")
        {            
            formsPanel.SetActive(false);                       
        }

        ButtonsGrid.SetActive(true);
    }
}
