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
        powerUpsController = transform.parent.Find("PowerUps").GetComponent<PowerUpsController>();

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
        ButtonsGrid.SetActive(false);
        //gameObject.transform.parent.Find("GameplayMenu").gameObject.SetActive(false);
        //gameObject.transform.parent.Find("GameplayMenuButtons").gameObject.SetActive(false);

        somePanelIsOpen = true;
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
        ButtonsGrid.SetActive(true);
        // gameObject.transform.parent.Find("GameplayMenu").gameObject.SetActive(true);
        // gameController.transform.parent.GetComponent<ZoomObject>().zoomIn = true;
        somePanelIsOpen = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameOver()
    {
        HideGameplayObjects();

        //Close all open panels
        transform.parent.Find("Panel_Ink_Buckets").gameObject.SetActive(false);
        transform.parent.Find("Panel_Forms").gameObject.SetActive(false);

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
        int numDeliveredBoxesValue = gameController.numDeliveredBoxes * gameController.earnCoinsValue;
        int numFailedBoxesValue = gameController.numFailedBoxes * gameController.discountCoinsValue;
        boxesDeliveredText.transform.Find("num").GetComponent<Text>().text = numDeliveredBoxesValue.ToString();
        boxesFailedText.transform.Find("num").GetComponent<Text>().text = ((gameController.numFailedBoxes > 0) ? "-" : "") + numFailedBoxesValue.ToString();

        //Total
        // totalText.transform.Find("num").GetComponent<Text>().text = numCoinsText.text;
        totalText.transform.Find("num").GetComponent<Text>().text = (numDeliveredBoxesValue - numFailedBoxesValue).ToString();
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

        //gameObject.transform.parent.Find("GameplayMenu").gameObject.SetActive(false);
    }

    public void ShowAllGameplayObjects()
    {
        gameObject.transform.parent.Find("RequestPanel").gameObject.SetActive(true);

        gameObject.transform.parent.Find("ButtonsGrid").gameObject.SetActive(true);

        //gameObject.transform.parent.Find("GameplayMenu").gameObject.SetActive(true);
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
        SetPowerUps();
        RefreshToolsCount();

        //If game was a paused and the player tap to continue
        if (isInGamePlay)
        {
            if (!isTutorial && !powerUpsController.FreezingTime_Flag)
            {
                timerIsRunning = true;
            }
            ShowAllGameplayObjects();
            Time.timeScale = 1f;
        }
        else
        {
            Transform Play_Button = transform.parent.Find("Menu").Find("Main").Find("Play_Button");
            Play_Button.Find("Text").GetComponent<Text>().text = "Tap to continue";
            Play_Button.gameObject.SetActive(true);
        }
    }

    private void SetPowerUps()
    {
        if (powerUpsController.DoubleCash_Flag)
        {
            PlayerPrefs.SetInt("PowerUp_DoubleCash", PlayerPrefs.GetInt("PowerUp_DoubleCash") - 1);
            gameObject.transform.parent.Find("Coins").Find("DoubleCashIcon").gameObject.SetActive(true);
        }

        if (powerUpsController.NoBrokenBottles_Flag)
        {
            PlayerPrefs.SetInt("PowerUp_NoBrokenBottles", PlayerPrefs.GetInt("PowerUp_NoBrokenBottles") - 1);
            gameObject.transform.parent.Find("ButtonsGrid").Find("InkBtn").Find("PowerUp_Icons").Find("NoBrokenBottles").gameObject.SetActive(true);
        }

        if (powerUpsController.BoosterFilling_OneBottle_Flag)
        {
            PlayerPrefs.SetInt("PowerUp_BoosterFilling_OneBottle", PlayerPrefs.GetInt("PowerUp_BoosterFilling_OneBottle") - 1);
            gameObject.transform.parent.Find("ButtonsGrid").Find("InkBtn").Find("PowerUp_Icons").Find("BoosterFilling_OneBottle").gameObject.SetActive(true);
        }

        if (powerUpsController.FixInTime_Flag)
        {
            PlayerPrefs.SetInt("PowerUp_FixInTime", PlayerPrefs.GetInt("PowerUp_FixInTime") - 1);
            gameObject.transform.parent.Find("ButtonsGrid").Find("InkBtn").Find("PowerUp_Icons").Find("FixInTime").gameObject.SetActive(true);
        }

        if (powerUpsController.BoosterFilling_Box_Flag)
        {
            PlayerPrefs.SetInt("PowerUp_BoosterFilling_Box", PlayerPrefs.GetInt("PowerUp_BoosterFilling_Box") - 1);
            //gameObject.transform.parent.Find("ButtonsGrid").Find("InkBtn").Find("PowerUp_Icons").Find("BoosterFilling_Box").gameObject.SetActive(true);            
        }

        if (powerUpsController.BoosterFilling_AllBottles_Flag)
        {
            PlayerPrefs.SetInt("PowerUp_BoosterFilling_AllBottles", PlayerPrefs.GetInt("PowerUp_BoosterFilling_AllBottles") - 1);
        }
    }

    private void ShowMenu()
    {
        menu.SetActive(true);
    }

    private void HideMenu()
    {
        menu.SetActive(false);
        StartCoroutine(ClosePanelAnimation(transform.parent.Find("Menu").Find("Main").Find("LevelDetails")));
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
        PlayClickButtonSFX();
        menu.transform.Find("Levels").gameObject.SetActive(false);
        menu.transform.Find("Settings").gameObject.SetActive(false);
        menu.transform.Find("Store").gameObject.SetActive(false);
        menu.transform.Find("ButtonsGridBack").gameObject.SetActive(false);
        menu.transform.Find("Main").gameObject.SetActive(true);
    }

    public void OpenSettings()
    {
        PlayClickButtonSFX();
        menu.transform.Find("Main").gameObject.SetActive(false);
        menu.transform.Find("ButtonsGridBack").gameObject.SetActive(true);
        menu.transform.Find("Settings").gameObject.SetActive(true);
    }

    public void OpenLevels()
    {
        PlayClickButtonSFX();
        menu.transform.Find("Main").gameObject.SetActive(false);
        menu.transform.Find("ButtonsGridBack").gameObject.SetActive(true);
        menu.transform.Find("Levels").gameObject.SetActive(true);
    }

    public void OpenStore()
    {
        PlayClickButtonSFX();
        menu.transform.Find("Main").gameObject.SetActive(false);
        menu.transform.Find("ButtonsGridBack").gameObject.SetActive(true);
        menu.transform.Find("Store").gameObject.SetActive(true);
    }

    public void PauseGame()
    {
        PlayClickButtonSFX();
        menu.gameObject.SetActive(true);
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
        Transform powerUpsButtons = transform.parent.Find("Menu").Find("Main").Find("LevelDetails").Find("Content").Find("PowerUps");
        foreach (Transform powerUp in powerUpsButtons)
        {
            powerUp.Find("BGCount").Find("Num").GetComponent<Text>().text = PlayerPrefs.GetInt("PowerUp_" + powerUp.name).ToString();
            powerUp.GetComponent<Button>().interactable = PlayerPrefs.GetInt("PowerUp_" + powerUp.name) > 0;
        }
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

    public void Test_AddMorePowerUpAndTools()
    {
        PlayerPrefs.SetInt("PowerUp_BoosterFilling_OneBottle", PlayerPrefs.GetInt("PowerUp_BoosterFilling_OneBottle") + 10);
        PlayerPrefs.SetInt("PowerUp_BoosterFilling_AllBottles", PlayerPrefs.GetInt("PowerUp_BoosterFilling_AllBottles") + 10);
        PlayerPrefs.SetInt("PowerUp_NoBrokenBottles", PlayerPrefs.GetInt("PowerUp_NoBrokenBottles") + 10);
        PlayerPrefs.SetInt("PowerUp_DoubleCash", PlayerPrefs.GetInt("PowerUp_DoubleCash") + 10);
        PlayerPrefs.SetInt("PowerUp_FreezingTime", PlayerPrefs.GetInt("PowerUp_FreezingTime") + 10);
        PlayerPrefs.SetInt("PowerUp_FixInTime", PlayerPrefs.GetInt("PowerUp_FixInTime") + 10);
        PlayerPrefs.SetInt("PowerUp_BoosterFilling_Box", PlayerPrefs.GetInt("PowerUp_BoosterFilling_Box") + 10);
        PlayerPrefs.SetInt("toolsCount", PlayerPrefs.GetInt("toolsCount") + 10);
    }

    public void CloseLevelDetailPanel()
    {
        PlayClickButtonSFX();
        StartCoroutine(ClosePanelAnimation(transform.parent.Find("Menu").Find("Main").Find("LevelDetails")));
        transform.parent.Find("Menu").Find("Main").Find("Play_Button").gameObject.SetActive(true);
    }

    public void PlayClickButtonSFX()
    {
        GetComponent<AudioSource>().Play();
    }

    IEnumerator ClosePanelAnimation(Transform panel)
    {
        panel.Find("BG").GetComponent<Animator>().Play("UI_BG_Transition_Close");
        panel.Find("Content").GetComponent<Animator>().Play("UI_JellyZoomOut_Auto");
        //Wait until the animations end, then hide the panel gameObject
        yield return new WaitForSeconds(0.5f);
        panel.gameObject.SetActive(false);
    }

    public void OpenLevelDetailPanel()
    {
        //If game was a paused and the player tap to continue
        if (isInGamePlay)
        {
            TapToPlay();
        }
        else
        {
            transform.parent.Find("Menu").Find("Main").Find("Play_Button").gameObject.SetActive(false);
            RefreshPowerUpsCount();

            Transform levelDetails = transform.parent.Find("Menu").Find("Main").Find("LevelDetails");
            levelDetails.gameObject.SetActive(true);            
            Text levelName = levelDetails.transform.Find("Content").Find("BG_Title").Find("Text").GetComponent<Text>();
            levelName.text = levelManager.world + "-" + levelManager.level;

            Transform stars = levelDetails.transform.Find("Content").Find("Stars");

            Text starsChallenge1 = stars.Find("Star1").Find("Text").GetComponent<Text>();
            starsChallenge1.text = levelManager.oneStarCoins.ToString();

            Text starsChallenge2 = stars.Find("Star2").Find("Text").GetComponent<Text>();
            starsChallenge2.text = levelManager.twoStarCoins.ToString();

            Text starsChallenge3 = stars.Find("Star3").Find("Text").GetComponent<Text>();
            starsChallenge3.text = levelManager.threeStarCoins.ToString();

        }    
    }

    public void CallFillOrFixRepository(GameObject bucketSelected)
    {
        gameController.transform.parent.Find("TopInkMachine").Find("Rail").Find(bucketSelected.name).GetComponent<InkRepositoryController>().FillOrFixRepository();
        SetActiveBucketButton(bucketSelected.name, false);
    }

    public void SetActiveBucketButton(string colorName, bool active)
    {
        Transform bucketButton = transform.parent.Find("Panel_Ink_Buckets").Find("Buckets").Find(colorName);
        bucketButton.GetComponent<Button>().interactable = active;
        bucketButton.Find("Clock").gameObject.SetActive(!active);
    }
}
