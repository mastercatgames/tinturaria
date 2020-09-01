using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private GameController gameController;
    private LevelManager levelManager;
    public GameObject ButtonsGrid;
    public bool somePanelIsOpen = false;
    public float timeRemaining;
    public bool timerIsRunning = false;
    public Text timeText;
    public GameObject gameOverPanel;
    public Text titleText;
    public Text boxesDeliveredText;
    public Text boxesFailedText;
    public Text totalText;

    void Start()
    {
        //timeRemaining = 120;
        // Starts the timer automatically
        //TODO: Start after 3 seconds
        //timerIsRunning = true;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
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
                GameOver();
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

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameOver()
    {
        Debug.Log("Time has run out!");
        timeRemaining = 0;
        timerIsRunning = false;
        gameOverPanel.SetActive(true);

        //Set Title
        titleText.text = levelManager.world + "-" + levelManager.level;

        boxesDeliveredText.text += " " + gameController.numDeliveredBoxes;
        boxesFailedText.text += " " + gameController.numFailedBoxes;

        //Num Coins
        boxesDeliveredText.transform.Find("num").GetComponent<Text>().text = (gameController.numDeliveredBoxes * 100).ToString();
        boxesFailedText.transform.Find("num").GetComponent<Text>().text = (gameController.numFailedBoxes * 150).ToString();

        //Total
        totalText.transform.Find("num").GetComponent<Text>().text = gameController.numCoins.ToString();
    }
}
