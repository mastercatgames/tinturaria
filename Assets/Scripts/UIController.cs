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
    public Text quoteText;

    void Start()
    {        
        //timeRemaining = 120;
        // Starts the timer automatically
        //TODO: Start after 3 seconds
        //timerIsRunning = true;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

        //Increase music volume
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().volume = 0.85f;
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

        //Stars animation
        //.Play("Stars_GameOver");
        Transform star1 = gameOverPanel.transform.Find("Stars").Find("1").Find("Star");
        Transform star2 = gameOverPanel.transform.Find("Stars").Find("2").Find("Star");
        Transform star3 = gameOverPanel.transform.Find("Stars").Find("3").Find("Star");

        int numStarsWon = 0;

        if (gameController.numCoins >= levelManager.oneStarCoins)
        {
            StartCoroutine(PlayAnimationAfterTime(star1.GetComponent<Animator>(), "UI_JellyZoom", 0f));
            numStarsWon = 1;
        }
        if (gameController.numCoins >= levelManager.twoStarCoins)
        {
            StartCoroutine(PlayAnimationAfterTime(star2.GetComponent<Animator>(), "UI_JellyZoom", 0.8f));
            numStarsWon = 2;
        }
        if (gameController.numCoins >= levelManager.threeStarCoins)
        {
            StartCoroutine(PlayAnimationAfterTime(star3.GetComponent<Animator>(), "UI_JellyZoom", 1.6f));
            numStarsWon = 3;
        }

        ShowFinalQuote(numStarsWon);

        boxesDeliveredText.text += " " + gameController.numDeliveredBoxes;
        boxesFailedText.text += " " + gameController.numFailedBoxes;

        //Num Coins
        boxesDeliveredText.transform.Find("num").GetComponent<Text>().text = (gameController.numDeliveredBoxes * 100).ToString();
        boxesFailedText.transform.Find("num").GetComponent<Text>().text = (gameController.numFailedBoxes * 150).ToString();

        //Total
        totalText.transform.Find("num").GetComponent<Text>().text = gameController.numCoinsText.text;

        HideGameplayObjects();
    }

    private void HideGameplayObjects()
    {
        gameController.transform.parent.Find("TopInkMachine").gameObject.SetActive(false);
        gameController.transform.parent.Find("BottomInkMachine").gameObject.SetActive(false);
        gameObject.transform.parent.Find("RequestPanel").gameObject.SetActive(false);
        gameObject.transform.parent.Find("Timer").gameObject.SetActive(false);
        gameObject.transform.parent.Find("ButtonsGrid").gameObject.SetActive(false);
        gameObject.transform.parent.Find("Coins").gameObject.SetActive(false);
        gameObject.transform.parent.Find("Restart_Button").gameObject.SetActive(false);
    }

    private void ShowFinalQuote(int numStarsWon)
    {
        float delay = 0f;

        if (numStarsWon == 0)
            delay = 0.5f;
        if (numStarsWon == 1)
            delay = 1.4f;
        else if (numStarsWon == 2)
            delay = 2.2f;
        else if (numStarsWon == 3)
            delay = 2.8f;

        if (numStarsWon == 0)
            quoteText.text = "Keep trying!";
        else if (numStarsWon == 1)
            quoteText.text = "Nice job!";
        else if (numStarsWon == 2)
            quoteText.text = "Awesome!";
        else if (numStarsWon == 3)
            quoteText.text = "Perfect!";

        StartCoroutine(PlayAnimationAfterTime(gameOverPanel.transform.Find("Quote").GetComponent<Animator>(), "UI_JellyZoom", delay, 1.2f));
    }

    private IEnumerator PlayAnimationAfterTime(Animator animator, string animationName, float delay, float speed = 0)
    {
        yield return new WaitForSeconds(delay);
        animator.Play(animationName);
        if (speed > 0)
            animator.speed = speed;
        print("Finish Animation!");
    }
}
