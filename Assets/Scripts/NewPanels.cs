using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPanels : MonoBehaviour
{
    public GameObject inkPanel;
    public GameObject formsPanel;

    public void OpenInkPanel() 
    {
        if (inkPanel != null)
        {
            bool isActive = inkPanel.activeSelf;
            inkPanel.SetActive(!isActive);
            
        }
    }

    public void OpenFormsPanel() 
    {
        if (formsPanel != null)
        {
            bool isActive = formsPanel.activeSelf;
            formsPanel.SetActive(!isActive);
            
        }
    }


    /*public void ClosePanel()
    {
        inkPanel.SetActive(false);
    }*/
}
