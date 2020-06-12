using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject ButtonsGrid;
    public bool somePanelIsOpen = false;

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        ButtonsGrid.SetActive(false);
        somePanelIsOpen = true;
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        ButtonsGrid.SetActive(true);
        somePanelIsOpen = false;
    }
}
