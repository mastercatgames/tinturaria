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
    public Text numCoinsText;
    public GameObject menu;
    public Text readyGo;
    public Button LevelButton;
    public bool isInGamePlay;
    public bool isTutorial;
    private PowerUpsController powerUpsController;

    void Start()
    {
        //timeRemaining = 120;
        // Starts the timer automatically
        //TODO: Start after 3 seconds
        //timerIsRunning = true;
        gameController = GameObject.Find("Gameplay").transform.Find("GameController").GetComponent<GameController>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        powerUpsController = transform.parent.Find("Panel_PowerUps").GetComponent<PowerUpsController>();

        //Increase music volume
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>().volume = 0.85f;

        //HideGameplayObjects();
        ShowMenu();

        //World
        for (int i = 1; i <= 14; i++)
        {
            //Level
            for (int j = 1; j <= 3; j++)
            {
                // i = 1; j = i
                CreateLevelButton(i + "_" + j);
            }
        }
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
        //ButtonsGrid.SetActive(false);
        gameObject.transform.parent.Find("GameplayMenu").gameObject.SetActive(false);
        gameObject.transform.parent.Find("GameplayMenuButtons").gameObject.SetActive(false);

        somePanelIsOpen = true;
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        //ButtonsGrid.SetActive(true);
        gameObject.transform.parent.Find("GameplayMenu").gameObject.SetActive(true);
        gameController.transform.parent.GetComponent<ZoomObject>().zoomIn = true;
        // somePanelIsOpen = false;
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
        int numDeliveredBoxesValue = gameController.numDeliveredBoxes * 100 * (powerUpsController.DoubleCash_Flag ? 2 : 1);
        int numFailedBoxesValue = gameController.numFailedBoxes * 150;
        boxesDeliveredText.transform.Find("num").GetComponent<Text>().text = numDeliveredBoxesValue.ToString();
        boxesFailedText.transform.Find("num").GetComponent<Text>().text = ((gameController.numFailedBoxes > 0) ? "-" : "") + numFailedBoxesValue.ToString();

        //Total
        // totalText.transform.Find("num").GetComponent<Text>().text = numCoinsText.text;
        totalText.transform.Find("num").GetComponent<Text>().text = (numDeliveredBoxesValue - numFailedBoxesValue).ToString();

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
        gameObject.transform.parent.Find("ButtonsGridPause").gameObject.SetActive(false);

        gameObject.transform.parent.Find("GameplayMenu").gameObject.SetActive(false);
    }

    public void ShowAllGameplayObjects()
    {
        gameObject.transform.parent.Find("RequestPanel").gameObject.SetActive(true);

        //gameObject.transform.parent.Find("ButtonsGrid").gameObject.SetActive(true); 

        gameObject.transform.parent.Find("GameplayMenu").gameObject.SetActive(true);
    }

    private void ShowInkMachine()
    {
        //Call Auto Animation when active
        gameController.transform.parent.Find("TopInkMachine").gameObject.SetActive(true);
        gameController.transform.parent.Find("BottomInkMachine").gameObject.SetActive(true);

        //Load inputManager here because when gameplay script start, the ink machine comes hidden
        //and this code returns null there. Then, here works fine
        gameController.inputManager = (SmoothMoveSwipe)FindObjectOfType(typeof(SmoothMoveSwipe));

        //Call other objects
        gameObject.transform.parent.Find("ButtonsGridPause").gameObject.SetActive(true);
        gameObject.transform.parent.Find("Coins").gameObject.SetActive(true);
        gameObject.transform.parent.Find("PaintButton").gameObject.SetActive(true);

        if (!isTutorial)
        {
            gameObject.transform.parent.Find("Timer").gameObject.SetActive(true);
        }

        if (!isInGamePlay)
        {
            gameObject.transform.parent.Find("ReadyGo").gameObject.SetActive(true);
        }
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

    public void TapToPlay()
    {
        GameObject.Find("Gameplay").transform.Find("GameController").gameObject.SetActive(true);
        //When showing ink machine here, it has an animation that calls TapToPlay script events
        ShowInkMachine();
        HideMenu();
        if (isInGamePlay)
        {
            if (!isTutorial)
            {
                timerIsRunning = true;
            }
            ShowAllGameplayObjects();
            Time.timeScale = 1f;
        }

        RefreshToolsCount();
        RefreshPowerUpsCount();
        //If is the TUTORIAL level
        // if (levelManager.world == 1 && levelManager.level == 1)
        // {
        //     timerIsRunning = false;
        //     isTutorial = true;
        //     print("Tutorial Level!");
        // }
    }

    private void ShowMenu()
    {
        menu.SetActive(true);
    }

    private void HideMenu()
    {
        menu.SetActive(false);
    }

    public void CreateLevelButton(string levelName)
    {
        var button = Instantiate(LevelButton) as Button;
        button.name = levelName;
        button.gameObject.transform.Find("Text").GetComponent<Text>().text = levelName;

        var rectTransform = button.GetComponent<RectTransform>();
        rectTransform.SetParent(menu.transform.Find("Levels").Find("ButtonsGrid"));
        button.onClick.AddListener(delegate { LoadLevel(button.name); });
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
        Time.timeScale = 1f;
    }

    public void BackToMainMenu()
    {
        menu.transform.Find("Levels").gameObject.SetActive(false);
        menu.transform.Find("Settings").gameObject.SetActive(false);
        menu.transform.Find("ButtonsGridBack").gameObject.SetActive(false);
        menu.transform.Find("Main").gameObject.SetActive(true);
    }

    public void OpenSettings()
    {
        menu.transform.Find("Main").gameObject.SetActive(false);
        menu.transform.Find("ButtonsGridBack").gameObject.SetActive(true);
        menu.transform.Find("Settings").gameObject.SetActive(true);
    }

    public void OpenLevels()
    {
        menu.transform.Find("Main").gameObject.SetActive(false);
        menu.transform.Find("ButtonsGridBack").gameObject.SetActive(true);
        menu.transform.Find("Levels").gameObject.SetActive(true);
    }

    public void PauseGame()
    {
        print("Pause!");
        menu.gameObject.SetActive(true);
        //gameObject.transform.parent.Find("RequestPanel").gameObject.SetActive(false);
        timerIsRunning = false;

        Time.timeScale = 0f;
    }

    public void RefreshToolsCount()
    {
        Transform bucketsButtons = gameObject.transform.parent.Find("Panel_Ink_Buckets").Find("Buckets");
        foreach (Transform bucket in bucketsButtons)
        {
            bucket.Find("Tool").Find("BGCount").Find("Num").GetComponent<Text>().text = PlayerPrefs.GetInt("toolsCount").ToString();
        }
    }

    public void RefreshPowerUpsCount()
    {
        Transform powerUpsButtons = gameObject.transform.parent.Find("Panel_PowerUps").Find("PowerUps");
        foreach (Transform powerUp in powerUpsButtons)
        {
            powerUp.Find("BGCount").Find("Num").GetComponent<Text>().text = PlayerPrefs.GetInt("PowerUp_" + powerUp.name).ToString();
        }
    }

    public void InkBtn_BoosterFilling_Icon_SetActive(bool active)
    {
        GameObject InkBtn_BoosterFilling_Icon = gameObject.transform.parent.Find("ButtonsGrid").Find("InkBtn").Find("PowerUp_Icons").Find("BoosterFilling").gameObject;
        InkBtn_BoosterFilling_Icon.SetActive(active);
    }

    public void InkBtn_NoBrokenBottles_Icon_SetActive(bool active)
    {
        GameObject InkBtn_NoBrokenBottles_Icon = gameObject.transform.parent.Find("ButtonsGrid").Find("InkBtn").Find("PowerUp_Icons").Find("NoBrokenBottles").gameObject;
        InkBtn_NoBrokenBottles_Icon.SetActive(active);
    }

    public void InkBtn_FixInTime_Icon_SetActive(bool active)
    {
        GameObject InkBtn_FixInTime_Icon = gameObject.transform.parent.Find("ButtonsGrid").Find("InkBtn").Find("PowerUp_Icons").Find("FixInTime").gameObject;
        InkBtn_FixInTime_Icon.SetActive(active);
    }

    public void InkBtn_BoosterFillingBox_Icon_SetActive(bool active)
    {
        GameObject InkBtn_BoosterFillingBox_Icon = gameController.transform.parent.Find("BottomInkMachine").Find("BoosterFillingBoxIcon").gameObject;
        InkBtn_BoosterFillingBox_Icon.SetActive(active);

        //Fix animation when show icon
        if (active == false)
        {
            InkBtn_BoosterFillingBox_Icon.transform.localScale = Vector3.zero;
        }
    }

    public void DoubleCash_Icon_SetActive(bool active)
    {
        GameObject DoubleCash_Icon = gameObject.transform.parent.Find("Coins").Find("DoubleCashIcon").gameObject;
        DoubleCash_Icon.SetActive(active);
    }

    public void FreezingTime_Icon_SetActive(bool active)
    {
        GameObject FreezingTime_Icon = gameObject.transform.parent.Find("Timer").Find("FreezingTime").gameObject;
        FreezingTime_Icon.SetActive(active);
    }

    public void Panel_PowerUps_SetInteractable(string buttonName, bool interactable)
    {
        gameObject.transform.parent.Find("Panel_PowerUps").Find("PowerUps").Find(buttonName).GetComponent<Button>().interactable = interactable;
    }

    public void OpenGameplayMenu()
    {
        if (!gameController.isChangingRepository)
        {
            if (somePanelIsOpen)
            {
                //transform.parent.Find("GameplayMenuButtons").gameObject.SetActive(false);
                transform.parent.Find("GameplayMenuButtons").GetComponent<Animator>().SetBool("hideButtons", true);
                // StartCoroutine(HideMenuButtons());
                gameController.transform.parent.GetComponent<ZoomObject>().zoomIn = true;
                // somePanelIsOpen = false;
            }
            else
            {
                somePanelIsOpen = true;
                transform.parent.Find("GameplayMenuButtons").gameObject.SetActive(true);
                gameController.transform.parent.GetComponent<ZoomObject>().zoomOut = true;
            }
        }
    }

    // IEnumerator HideMenuButtons()
    // {
    //     yield return new WaitForSeconds(0.5f);
    //     transform.parent.Find("GameplayMenuButtons").gameObject.SetActive(false);
    // }
}
