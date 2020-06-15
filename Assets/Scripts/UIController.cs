using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject ButtonsGrid;
    public bool somePanelIsOpen = false;
    public float timeRemaining;
    public bool timerIsRunning = false;
    public Text timeText;

    void Start()
    {
        timeRemaining = 120;
        // Starts the timer automatically
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

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
