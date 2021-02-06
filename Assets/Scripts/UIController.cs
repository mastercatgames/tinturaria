using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameToolkit.Localization;
using System.IO;

public class UIController : MonoBehaviour
{
    private GameController gameController;
    private LevelManager levelManager;
    private PowerUpsController powerUpsController;
    public GameObject ButtonsGrid, gameOverPanel, rewardPanel, menu, coinIcon, LevelPanel, creditsAlert;
    public Text timeText, titleText, boxesDeliveredText, boxesFailedText, totalText, quoteText, numCoinsText, toolsUI, gemsUI, coinsUI, readyGo, languageText;
    public Button LevelButton;
    public Toggle vibrationToggle;
    public float timeRemaining;
    public bool timerIsRunning, somePanelIsOpen, isInGamePlay, isTutorial, isToolTutorial, isPowerUpTutorial, blockSwipe, blockRightSwipe, blockPainting;
    public int currentTotalCoins, starsCount, levelsCount, worldsCount;

    void Start()
    {
        //timeRemaining = 120;
        // Starts the timer automatically
        //TODO: Start after 3 seconds
        //timerIsRunning = true;
        blockSwipe = false;
        blockRightSwipe = false;
        blockPainting = false;
        gameController = GameObject.Find("Gameplay").transform.Find("GameController").GetComponent<GameController>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        powerUpsController = transform.parent.Find("PowerUps").GetComponent<PowerUpsController>();

        if (PlayerPrefs.GetString("vibration") == "")
        {
            PlayerPrefs.SetString("vibration", "on");
        }

        SetVibrationOption(PlayerPrefs.GetString("vibration") == "on" ? true : false);

        print("Current Language: " + Localization.Instance.CurrentLanguage);

        ChangeLanguage();

        //Verify if the first tools was given
        if (PlayerPrefs.GetInt("firstToolsGiven") == 0)
        {
            PlayerPrefs.SetInt("firstToolsGiven", 1);
            PlayerPrefs.SetInt("toolsCount", 5);
        }

        ShowMenu();
        RefreshUIToolsAndMoney();

        //Init Levels
        SetSelectedLevelVariables();
        SetStarsCount();

        worldsCount = (levelManager.GetAllLevelsProperties().Count / 3);

        string LevelDataJSON = File.ReadAllText(Application.dataPath + "/LevelData.json");

        for (int world = 1; world <= worldsCount; world++)
        {
            CreateLevelPanel(world, LevelDataJSON);
        }

        //Init Levels button in Main Menu
        Text starCountText = transform.parent.Find("Menu").Find("Main").Find("LevelsButton").Find("StarCount").GetComponentInChildren<Text>();
        starCountText.text = starsCount + "/" + (worldsCount * 9); //9 = 3 Levels X 3 Stars
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

    public void LoadNextLevel()
    {
        int nextWorld = levelManager.world,
            nextLevel = levelManager.level + 1;

        if (nextLevel == 4)
        {
            nextWorld += 1;
            nextLevel = 1;
        }

        PlayerPrefs.SetString("_CurrentLevel", nextWorld + "_" + nextLevel);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameOver()
    {
        HideGameplayObjects();

        //Hide GameOver Objects
        foreach (Transform child in gameOverPanel.transform)
        {
            if (child.name != "Title" && child.name != "Stars")
                child.gameObject.SetActive(false);
        }

        //Close all open panels
        transform.parent.Find("Panel_Ink_Buckets").gameObject.SetActive(false);
        transform.parent.Find("Panel_Forms").gameObject.SetActive(false);

        //Show UI counters (TopHeader)
        transform.parent.Find("TopHeader").gameObject.SetActive(true);

        Debug.Log("Time has run out!");
        timeRemaining = 0;
        timerIsRunning = false;
        gameOverPanel.SetActive(true);

        //Set Title
        titleText.text = levelManager.world + "-" + levelManager.level;

        //Set (qty) Boxes delivered and failed
        boxesDeliveredText.text += " " + gameController.numDeliveredBoxes;
        boxesFailedText.text += " " + gameController.numFailedBoxes;

        //Num Coins
        int numDeliveredBoxesValue = gameController.numDeliveredBoxes * gameController.earnCoinsValue;
        int numFailedBoxesValue = gameController.numFailedBoxes * gameController.discountCoinsValue;
        boxesDeliveredText.transform.Find("num").GetComponent<Text>().text = numDeliveredBoxesValue.ToString();
        boxesFailedText.transform.Find("num").GetComponent<Text>().text = ((gameController.numFailedBoxes > 0) ? "-" : "") + numFailedBoxesValue.ToString();

        //Total
        int totalCoins = numDeliveredBoxesValue - numFailedBoxesValue;
        currentTotalCoins = totalCoins;
        totalText.transform.Find("num").GetComponent<Text>().text = (totalCoins).ToString();

        if (totalCoins > 0)
        {
            PlayerPrefs.SetInt("coinsCount", PlayerPrefs.GetInt("coinsCount") + totalCoins);
        }

        //Highscore (Your best)
        int currentHighscore = totalCoins > levelManager.highscore ? totalCoins : levelManager.highscore;
        gameOverPanel.transform.Find("TotalAndHighscore").Find("YourBest").Find("num").GetComponent<Text>().text = (currentHighscore).ToString();

        //Reward
        rewardPanel.transform.Find("num").GetComponent<Text>().text = totalCoins > 0 ? (totalCoins).ToString() : "0";

        //Stars animation
        //.Play("Stars_GameOver");
        Transform star1 = gameOverPanel.transform.Find("Stars").Find("1").Find("Star");
        Transform star2 = gameOverPanel.transform.Find("Stars").Find("2").Find("Star");
        Transform star3 = gameOverPanel.transform.Find("Stars").Find("3").Find("Star");

        int numStarsWon = 0;

        if (totalCoins >= levelManager.oneStarCoins)
        {
            // StartCoroutine(PlayAnimationAfterTime(star1.GetComponent<Animator>(), "UI_JellyZoom", 0f));
            StartCoroutine(SetActiveAfterTime(star1.gameObject, true, 0.8f));
            numStarsWon = 1;
        }
        if (totalCoins >= levelManager.twoStarCoins)
        {
            // StartCoroutine(PlayAnimationAfterTime(star2.GetComponent<Animator>(), "UI_JellyZoom", 0.8f));
            StartCoroutine(SetActiveAfterTime(star2.gameObject, true, 1.6f));
            numStarsWon = 2;
        }
        if (totalCoins >= levelManager.threeStarCoins)
        {
            // StartCoroutine(PlayAnimationAfterTime(star3.GetComponent<Animator>(), "UI_JellyZoom", 1.6f));
            StartCoroutine(SetActiveAfterTime(star3.gameObject, true, 2.4f));
            numStarsWon = 3;
            gameOverPanel.transform.Find("Stars").Find("SunRay").gameObject.SetActive(true);
            StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("Confetti").gameObject, true, 2.5f));
        }

        SetFinalQuote(numStarsWon);

        StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("boxesDelivered").gameObject, true, 1f));
        StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("boxesFailed").gameObject, true, 1.5f));
        StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("TotalAndHighscore").gameObject, true, 2f));
        StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("Quote").gameObject, true, 2.5f));
        StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("Reward").gameObject, true, 2.5f));

        if (totalCoins > 0)
        {
            StartCoroutine(StartMoveCoinsToUI(totalCoins, 3f));
        }
        else
        {
            StartCoroutine(SetActiveAfterTime(gameOverPanel.transform.Find("ButtonsGrid").gameObject, true, 2.5f));
        }

        if (isPowerUpTutorial)
        {
            PlayerPrefs.SetInt("PowerUpsTutorial_Step", PlayerPrefs.GetInt("PowerUpsTutorial_Step") + 1);
        }

        levelManager.SetLevelProgress(new LevelManager.LevelProgress()
        {
            world = levelManager.world, //selected world
            level = levelManager.level, //selected level
            highscore = currentHighscore,
            starsEarned = numStarsWon
        });

        SetStarsCount();

        //Verify if the next level was unlocked
        string LevelDataJSON = File.ReadAllText(Application.dataPath + "/LevelData.json");

        int nextWorld = levelManager.world,
            nextLevel = levelManager.level + 1;

        if (nextLevel == 4)
        {
            nextWorld += 1;
            nextLevel = 1;
        }

        int starsToUnlock = levelManager.GetLevelProperties(nextWorld, nextLevel, LevelDataJSON).starsToUnlock;

        //starsCount += numStarsWon;        

        if (starsCount >= starsToUnlock)
        {
            gameOverPanel.transform.Find("ButtonsGrid").Find("NextLevel").gameObject.SetActive(true);

            if (starsCount == starsToUnlock)
            {
                gameOverPanel.transform.Find("ButtonsGrid").Find("NextLevel").Find("UnlockedTxt").gameObject.SetActive(true);
            }
        }
    }

    public IEnumerator StartMoveCoinsToUI(int totalCoins, float delay)
    {
        yield return new WaitForSeconds(delay);
        int timesToMoveCoins = (totalCoins / 50);
        float moveCoinsDelay = 1f;

        for (int i = 0; i < timesToMoveCoins; i++)
        {
            Invoke("MoveCoinsToUI", moveCoinsDelay);
            moveCoinsDelay = moveCoinsDelay + 0.2f;
        }
    }

    public void MoveCoinsToUI()
    {
        GameObject coin = Instantiate(coinIcon) as GameObject;
        coin.GetComponent<RectTransform>().position = rewardPanel.transform.Find("Coin").GetComponent<RectTransform>().position;
        coin.GetComponent<MoveCoinsToUI>().target = coinsUI.transform.parent.Find("Icon").GetComponent<RectTransform>();
        coin.transform.SetParent(gameObject.transform.parent);
        //coinsUI.GetComponentInParent<Animator>().enabled = false;
        coinsUI.GetComponentInParent<Animator>().Play("Idle");
        // coinsUI.transform.parent.GetComponent<Animator>().enabled = false;
        coin.SetActive(true);
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
        gameObject.transform.parent.Find("Tutorial").gameObject.SetActive(false);

        //gameObject.transform.parent.Find("GameplayMenu").gameObject.SetActive(false);
    }

    public void ShowAllGameplayObjects()
    {
        gameObject.transform.parent.Find("RequestPanel").gameObject.SetActive(true);

        if (!isTutorial)
        {
            gameObject.transform.parent.Find("ButtonsGridPause").gameObject.SetActive(true);
            gameObject.transform.parent.Find("ButtonsGrid").gameObject.SetActive(true);
        }
    }

    private void ShowInkMachine()
    {
        //Call Auto Animation when active
        gameController.transform.parent.Find("TopInkMachine").gameObject.SetActive(true);
        gameController.transform.parent.Find("BottomInkMachine").gameObject.SetActive(true);

        //Load inputManager here because when gameplay script start, the ink machine comes hidden
        //and this code returns null there. Then, here works fine
        // gameController.inputManager = (SmoothMoveSwipe)FindObjectOfType(typeof(SmoothMoveSwipe));
        gameController.InitSmoothMoveSwipe();

        //Call other objects
        gameObject.transform.parent.Find("Coins").gameObject.SetActive(true);
        gameObject.transform.parent.Find("PaintButton").gameObject.SetActive(true);

        gameObject.transform.parent.Find("Timer").gameObject.SetActive(true);
    }

    private void SetFinalQuote(int numStarsWon)
    {
        if (numStarsWon == 0)
            quoteText.text = "Keep trying!";
        else if (numStarsWon == 1)
            quoteText.text = "Nice job!";
        else if (numStarsWon == 2)
            quoteText.text = "Awesome!";
        else if (numStarsWon == 3)
            quoteText.text = "Perfect!";
    }

    public IEnumerator PlayAnimationAfterTime(Animator animator, string animationName, float delay, float speed = 0)
    {
        yield return new WaitForSeconds(delay);
        animator.Play(animationName);
        if (speed > 0)
            animator.speed = speed;
        print("Finish Animation!");
    }

    public IEnumerator SetActiveAfterTime(GameObject gameObject, bool active, float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(active);
    }

    public void TapToPlay()
    {
        GameObject.Find("Gameplay").transform.Find("GameController").gameObject.SetActive(true);
        transform.parent.Find("TopHeader").gameObject.SetActive(false);
        //When showing ink machine here, it has an animation that calls TapToPlay script events
        ShowInkMachine();
        HideMenu();
        SetPowerUps();
        RefreshToolsCount();

        if (isPowerUpTutorial)
        {
            transform.parent.Find("PowerUpsTutorial").gameObject.SetActive(false);
        }

        //If game was a paused and the player tap to continue
        if (isInGamePlay)
        {
            // if (!isTutorial && !powerUpsController.FreezingTime_Flag)
            // {
            timerIsRunning = true;
            // }
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

    private void DiscountPowerUpCount(string powerUpName)
    {
        if (!isPowerUpTutorial)
        {
            PlayerPrefs.SetInt(powerUpName, PlayerPrefs.GetInt(powerUpName) - 1);
        }
    }

    private void SetPowerUps()
    {
        if (powerUpsController.DoubleCash_Flag)
        {
            // PlayerPrefs.SetInt("PowerUp_DoubleCash", PlayerPrefs.GetInt("PowerUp_DoubleCash") - 1);
            DiscountPowerUpCount("PowerUp_DoubleCash");
            gameObject.transform.parent.Find("Coins").Find("DoubleCashIcon").gameObject.SetActive(true);
        }

        if (powerUpsController.NoBrokenBottles_Flag)
        {
            // PlayerPrefs.SetInt("PowerUp_NoBrokenBottles", PlayerPrefs.GetInt("PowerUp_NoBrokenBottles") - 1);
            DiscountPowerUpCount("PowerUp_NoBrokenBottles");
            gameObject.transform.parent.Find("ButtonsGrid").Find("InkBtn").Find("PowerUp_Icons").Find("NoBrokenBottles").gameObject.SetActive(true);
        }

        if (powerUpsController.BoosterFilling_OneBottle_Flag)
        {
            // PlayerPrefs.SetInt("PowerUp_BoosterFilling_OneBottle", PlayerPrefs.GetInt("PowerUp_BoosterFilling_OneBottle") - 1);
            DiscountPowerUpCount("PowerUp_BoosterFilling_OneBottle");
            gameObject.transform.parent.Find("ButtonsGrid").Find("InkBtn").Find("PowerUp_Icons").Find("BoosterFilling_OneBottle").gameObject.SetActive(true);
        }

        if (powerUpsController.FixInTime_Flag)
        {
            // PlayerPrefs.SetInt("PowerUp_FixInTime", PlayerPrefs.GetInt("PowerUp_FixInTime") - 1);
            DiscountPowerUpCount("PowerUp_FixInTime");
            gameObject.transform.parent.Find("ButtonsGrid").Find("InkBtn").Find("PowerUp_Icons").Find("FixInTime").gameObject.SetActive(true);
        }

        if (powerUpsController.BoosterFilling_Box_Flag)
        {
            // PlayerPrefs.SetInt("PowerUp_BoosterFilling_Box", PlayerPrefs.GetInt("PowerUp_BoosterFilling_Box") - 1);
            DiscountPowerUpCount("PowerUp_BoosterFilling_Box");
            //gameObject.transform.parent.Find("ButtonsGrid").Find("InkBtn").Find("PowerUp_Icons").Find("BoosterFilling_Box").gameObject.SetActive(true);            
        }

        if (powerUpsController.BoosterFilling_AllBottles_Flag)
        {
            // PlayerPrefs.SetInt("PowerUp_BoosterFilling_AllBottles", PlayerPrefs.GetInt("PowerUp_BoosterFilling_AllBottles") - 1);
            DiscountPowerUpCount("PowerUp_BoosterFilling_AllBottles");
        }
    }

    private void ShowMenu()
    {
        menu.SetActive(true);
        menu.transform.Find("Main").gameObject.SetActive(true);
    }

    private void HideMenu()
    {
        menu.SetActive(false);
        StartCoroutine(ClosePanelAnimation(transform.parent.Find("Menu").Find("Main").Find("LevelDetails")));
    }

    #region Level Manager

    public void SetSelectedLevelVariables()
    {
        string _currentLevel = PlayerPrefs.GetString("_CurrentLevel");

        if (_currentLevel == "")
        {
            PlayerPrefs.SetString("_CurrentLevel", "1_1");
        }

        //Load a background (Assets/Resources/Backgrounds/WORLD_LEVEL.png)
        GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>().sprite =
        Resources.Load<Sprite>("Backgrounds/" + _currentLevel);

        levelManager.world = int.Parse(_currentLevel.Split('_')[0]);
        levelManager.level = int.Parse(_currentLevel.Split('_')[1]);

        string LevelDataJSON = File.ReadAllText(Application.dataPath + "/LevelData.json");
        LevelManager.Level selectedLevel = levelManager.GetLevelProperties(levelManager.world, levelManager.level, LevelDataJSON);//JsonUtility.FromJson<LevelList>(LevelDataJSON).Levels.Find(c => c.world == world && c.level == level);

        levelManager.oneStarCoins = selectedLevel.oneStarCoins;
        levelManager.twoStarCoins = selectedLevel.twoStarCoins;
        levelManager.threeStarCoins = selectedLevel.threeStarCoins;

        if (PlayerPrefs.GetString("_LevelProgress") == "")
        {
            //Init the Level Progress save data
            PlayerPrefs.SetString("_LevelProgress", File.ReadAllText(Application.dataPath + "/LevelProgressRaw.json"));
        }

        levelManager.highscore = JsonUtility.FromJson<LevelManager.LevelProgressList>(PlayerPrefs.GetString("_LevelProgress")).LevelsProgress.Find(c => c.world == levelManager.world && c.level == levelManager.level).highscore;

        timeRemaining = selectedLevel.time;
        levelManager.SetBrokenBottlesPosition(selectedLevel.qtyColorsToBreak);

        isTutorial = false;
        isToolTutorial = false;
        isPowerUpTutorial = false;

        //Conditions
        if (levelManager.world == 1 && levelManager.level == 1)
        {
            isTutorial = true;
        }
        else if (levelManager.world == 2)
        {
            if (levelManager.level == 1)
            {
                isToolTutorial = true;
                levelManager.requestedColorsPosition.Add(1);
            }
            else if (levelManager.level == 3 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 0)
            {
                isPowerUpTutorial = true;
            }
        }
        else if (levelManager.world == 3)
        {
            if (levelManager.level == 2 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 1)
            {
                isPowerUpTutorial = true;
            }
        }
        else if (levelManager.world == 4)
        {
            if (levelManager.level == 1 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 2)
            {
                isPowerUpTutorial = true;
            }
            else if (levelManager.level == 3 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 3)
            {
                isPowerUpTutorial = true;
            }
        }
        else if (levelManager.world == 5)
        {
            if (levelManager.level == 2 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 4)
            {
                isPowerUpTutorial = true;
            }
        }
        else if (levelManager.world == 6)
        {
            if (levelManager.level == 1 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 5)
            {
                isPowerUpTutorial = true;
            }
        }
    }

    public void SetStarsCount()
    {
        starsCount = 0;
        foreach (var item in JsonUtility.FromJson<LevelManager.LevelProgressList>(PlayerPrefs.GetString("_LevelProgress")).LevelsProgress)
        {
            starsCount += item.starsEarned;
        }
    }

    public void CreateLevelButton(int world, int level)
    {
        string levelName = world + "_" + level;
        var button = Instantiate(LevelButton) as Button;
        button.name = levelName;
        button.gameObject.transform.Find("Text").GetComponent<Text>().text = levelName;
        button.gameObject.transform.Find("Text").GetComponent<Text>().resizeTextForBestFit = true;

        var rectTransform = button.GetComponent<RectTransform>();
        rectTransform.SetParent(menu.transform.Find("Levels").Find("ButtonsGrid"));
        button.onClick.AddListener(delegate { LoadLevel(button.name); });
    }

    public void CreateLevelPanel(int worldNum, string LevelDataJSON)
    {
        var panel = Instantiate(LevelPanel) as GameObject;
        panel.name = "World" + worldNum;

        LocalizedTextBehaviour title = panel.transform.Find("Title").Find("Text").GetComponent<LocalizedTextBehaviour>();
        title.FormatArgs[0] = worldNum.ToString();

        var rectTransform = panel.GetComponent<RectTransform>();
        rectTransform.SetParent(menu.transform.Find("Levels").Find("ScrollRect").Find("Content"));

        panel.transform.localScale = new Vector3(1f, 1f, 1f);

        Transform levelButtons = panel.transform.Find("Buttons");
        Button levelButton = null;
        int starsEarned = 0;
        int starsToUnlock = 0;

        for (int levelNum = 1; levelNum <= 3; levelNum++)
        {
            levelButton = levelButtons.Find(levelNum.ToString()).GetComponent<Button>();
            // levelButton.onClick = null;
            // levelButton.onClick = new Button.ButtonClickedEvent();

            // string sceneName = (worldNum + "_" + levelNum);
            // levelButton.onClick.AddListener(delegate { LoadLevel(sceneName); });
            starsEarned = levelManager.GetLevelProgress(worldNum, levelNum).starsEarned;

            //Activate/Show yellow star (if the player have) 
            for (int star = 1; star <= starsEarned; star++)
            {
                levelButton.transform.Find("Stars").Find(star.ToString()).Find("star").gameObject.SetActive(true);
            }

            //Unlock level
            if (worldNum == 1 && levelNum == 1)
            {
                // levelButton.interactable = true;
                levelButton.transform.Find("Lock").gameObject.SetActive(false);
            }

            levelButton.onClick = null;
            levelButton.onClick = new Button.ButtonClickedEvent();

            starsToUnlock = levelManager.GetLevelProperties(worldNum, levelNum, LevelDataJSON).starsToUnlock;

            string sceneName = (worldNum + "_" + levelNum);

            if (starsCount >= starsToUnlock)
            {
                levelButton.colors = ColorBlock.defaultColorBlock;
                levelButton.transform.Find("Lock").gameObject.SetActive(false);
                // string sceneName = (worldNum + "_" + levelNum);
                levelButton.onClick.AddListener(delegate { LoadLevel(sceneName); });
            }
            else
            {
                levelButton.onClick = null;
                levelButton.onClick = new Button.ButtonClickedEvent();
                levelButton.onClick.AddListener(delegate { ShowLockedLevelAlert(sceneName); });
            }
        }
    }

    public void ShowLockedLevelAlert(string sceneName)
    {
        Transform LockAlert = menu.transform.Find("Levels").Find("LockAlert");
        LocalizedTextBehaviour description = LockAlert.Find("Content").Find("Alert").Find("Description").GetComponent<LocalizedTextBehaviour>();
        int starsToUnlock = levelManager.GetLevelProperties(int.Parse(sceneName.Split('_')[0]), int.Parse(sceneName.Split('_')[1])).starsToUnlock;
        description.FormatArgs[0] = starsToUnlock.ToString();

        LocalizedTextBehaviour youHave = LockAlert.Find("Content").Find("Alert").Find("YouHave").GetComponent<LocalizedTextBehaviour>();
        youHave.FormatArgs[0] = starsCount.ToString();
        LockAlert.gameObject.SetActive(true);
    }

    public void CloseLockedLevelAlert()
    {
        Transform LockAlert = menu.transform.Find("Levels").Find("LockAlert");
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");
        StartCoroutine(ClosePanelAnimation(LockAlert));
    }

    public void LoadLevel(string levelName)
    {
        //SceneManager.LoadScene(levelName);
        PlayerPrefs.SetString("_CurrentLevel", levelName);
        SetSelectedLevelVariables();
        BackToMainMenu();
        Time.timeScale = 1f;
        RestartGame();
    }

    #endregion

    public void BackToMainMenu()
    {
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");
        menu.transform.Find("Levels").gameObject.SetActive(false);
        menu.transform.Find("Settings").gameObject.SetActive(false);
        menu.transform.Find("Store").gameObject.SetActive(false);
        menu.transform.Find("ButtonsGridBack").gameObject.SetActive(false);
        menu.transform.Find("Main").gameObject.SetActive(true);
    }

    public void OpenSettings()
    {
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");
        menu.transform.Find("Main").gameObject.SetActive(false);
        menu.transform.Find("ButtonsGridBack").gameObject.SetActive(true);
        menu.transform.Find("Settings").gameObject.SetActive(true);

        if (Localization.Instance.CurrentLanguage == SystemLanguage.English)
            menu.transform.Find("Settings").Find("Languages").Find("English").GetComponent<Toggle>().isOn = true;
        if (Localization.Instance.CurrentLanguage == SystemLanguage.Portuguese)
            menu.transform.Find("Settings").Find("Languages").Find("Portuguese").GetComponent<Toggle>().isOn = true;
    }

    public void OpenLevels()
    {
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");
        menu.transform.Find("Main").gameObject.SetActive(false);
        menu.transform.Find("ButtonsGridBack").gameObject.SetActive(true);
        menu.transform.Find("Levels").gameObject.SetActive(true);
    }

    public void OpenStore()
    {
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");
        menu.transform.Find("Main").gameObject.SetActive(false);
        menu.transform.Find("ButtonsGridBack").gameObject.SetActive(true);
        menu.transform.Find("Store").gameObject.SetActive(true);
        //Reset scroll to top
        menu.transform.Find("Store").Find("ScrollRectVertical").GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
    }

    public void PauseGame()
    {
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");
        gameObject.transform.parent.Find("Tutorial").gameObject.SetActive(false);
        gameObject.transform.parent.Find("Menu").Find("Main").Find("ShopButton").gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
        transform.parent.Find("TopHeader").gameObject.SetActive(true);
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

            if (powerUp.name == "DoubleCash")
            {
                if ((isPowerUpTutorial && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 0))
                {
                    powerUp.Find("BGCount").gameObject.SetActive(false);
                    powerUp.Find("Lock").gameObject.SetActive(false);
                }
                else if (PlayerPrefs.GetInt("PowerUpsTutorial_Step") > 0)
                {
                    powerUp.Find("BGCount").gameObject.SetActive(true);
                    powerUp.Find("Lock").gameObject.SetActive(false);
                }
            }

            if (powerUp.name == "NoBrokenBottles")
            {
                if ((isPowerUpTutorial && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 1))
                {
                    powerUp.Find("BGCount").gameObject.SetActive(false);
                    powerUp.Find("Lock").gameObject.SetActive(false);
                }
                else if (PlayerPrefs.GetInt("PowerUpsTutorial_Step") > 1)
                {
                    powerUp.Find("BGCount").gameObject.SetActive(true);
                    powerUp.Find("Lock").gameObject.SetActive(false);
                }
            }

            if (powerUp.name == "BoosterFilling_OneBottle")
            {
                if ((isPowerUpTutorial && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 2))
                {
                    powerUp.Find("BGCount").gameObject.SetActive(false);
                    powerUp.Find("Lock").gameObject.SetActive(false);
                }
                else if (PlayerPrefs.GetInt("PowerUpsTutorial_Step") > 2)
                {
                    powerUp.Find("BGCount").gameObject.SetActive(true);
                    powerUp.Find("Lock").gameObject.SetActive(false);
                }
            }

            if (powerUp.name == "BoosterFilling_AllBottles")
            {                
                if ((isPowerUpTutorial && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 3))
                {
                    powerUp.Find("BGCount").gameObject.SetActive(false);
                    powerUp.Find("Lock").gameObject.SetActive(false);
                }
                else if (PlayerPrefs.GetInt("PowerUpsTutorial_Step") > 3)
                {
                    powerUp.Find("BGCount").gameObject.SetActive(true);
                    powerUp.Find("Lock").gameObject.SetActive(false);
                }
            }

            if (powerUp.name == "FixInTime")
            {
                if ((isPowerUpTutorial && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 4))
                {
                    powerUp.Find("BGCount").gameObject.SetActive(false);
                    powerUp.Find("Lock").gameObject.SetActive(false);
                }
                else if (PlayerPrefs.GetInt("PowerUpsTutorial_Step") > 4)
                {
                    powerUp.Find("BGCount").gameObject.SetActive(true);
                    powerUp.Find("Lock").gameObject.SetActive(false);
                }
            }

            if (powerUp.name == "BoosterFilling_Box")
            {
                if ((isPowerUpTutorial && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 5))
                {
                    powerUp.Find("BGCount").gameObject.SetActive(false);
                    powerUp.Find("Lock").gameObject.SetActive(false);
                }
                else if (PlayerPrefs.GetInt("PowerUpsTutorial_Step") > 5)
                {
                    powerUp.Find("BGCount").gameObject.SetActive(true);
                    powerUp.Find("Lock").gameObject.SetActive(false);
                }
            }
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
        PlayerPrefs.SetInt("coinsCount", PlayerPrefs.GetInt("coinsCount") + 10000);
        PlayerPrefs.SetInt("gemsCount", PlayerPrefs.GetInt("gemsCount") + 10);
        RefreshUIToolsAndMoney();
    }

    public void CloseLevelDetailPanel()
    {
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");
        StartCoroutine(ClosePanelAnimation(transform.parent.Find("Menu").Find("Main").Find("LevelDetails")));
        transform.parent.Find("Menu").Find("Main").Find("Play_Button").gameObject.SetActive(true);
    }

    public IEnumerator ClosePanelAnimation(Transform panel)
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
        if (isInGamePlay //|| isTutorial)
        || levelManager.world == 1)
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

            //Verify if show Power Up tutorial
            if (isPowerUpTutorial)
            {
                levelDetails.transform.Find("Content").Find("TapToPlayBtn").gameObject.SetActive(false);
                transform.parent.Find("PowerUpsTutorial").gameObject.SetActive(true);
            }
        }
    }

    public void CallFillOrFixRepository(GameObject bucketSelected)
    {
        gameController.transform.parent.Find("TopInkMachine").Find("Rail").Find(bucketSelected.name).GetComponent<InkRepositoryController>().FillOrFixRepository();

        if (powerUpsController.FixInTime_Flag == false)
            SetActiveBucketButton(bucketSelected.name, false);
    }

    public void SetActiveBucketButton(string colorName, bool active)
    {
        Transform bucketButton = transform.parent.Find("Panel_Ink_Buckets").Find("Buckets").Find(colorName);
        bucketButton.GetComponent<Button>().interactable = active;
        bucketButton.Find("Clock").gameObject.SetActive(!active);
    }

    public void ChangeLanguage(bool buttonClick = false/*string language*/)
    {
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");

        // if (language == "en")
        //     Localization.Instance.CurrentLanguage = SystemLanguage.English;
        // else if (language == "pt")
        //     Localization.Instance.CurrentLanguage = SystemLanguage.Portuguese;
        if (buttonClick)
        {
            if (languageText.text == "English")
                Localization.Instance.CurrentLanguage = SystemLanguage.Portuguese;
            else
                Localization.Instance.CurrentLanguage = SystemLanguage.English;
        }

        string currentLanguage = Localization.Instance.CurrentLanguage.ToString();
        languageText.text = currentLanguage == "Portuguese" ? "Português" : currentLanguage.ToString();

    }

    public void RefreshUIToolsAndMoney()
    {
        toolsUI.text = PlayerPrefs.GetInt("toolsCount").ToString();
        gemsUI.text = PlayerPrefs.GetInt("gemsCount").ToString();
        coinsUI.text = PlayerPrefs.GetInt("coinsCount").ToString();
    }

    public void CallChangeCurrentBox(GameObject newBox)
    {
        gameController.ChangeCurrentBox(newBox);
    }

    public void CallNewPaintFluid()
    {
        gameController.NewPaintFluid(false);
    }

    public void SetVibrationOption(bool isOn)
    {
        PlayerPrefs.SetString("vibration", isOn ? "on" : "off");
        vibrationToggle.isOn = isOn;

        if (isOn)
        {
            vibrationToggle.transform.Find("OffIcon").gameObject.SetActive(false);
        }
        else
        {
            vibrationToggle.transform.Find("OffIcon").gameObject.SetActive(true);
        }
    }

    public void OpenCredits()
    {
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");
        creditsAlert.SetActive(true);
    }

    public void CloseCredits()
    {
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");
        StartCoroutine(ClosePanelAnimation(creditsAlert.transform));
    }

    public void OpenAlertPowerUpsLocked(GameObject powerUp)
    {
        if (!isPowerUpTutorial && !powerUp.transform.Find("BGCount").gameObject.activeSelf /*&& PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 0*/)
        {
            GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");
            transform.parent.Find("Menu").Find("Main").Find("LevelDetails").Find("AlertLocked").gameObject.SetActive(true);
        }
    }
    public void CloseAlertPowerUpsLocked()
    {
        GameObject.Find("AudioController").GetComponent<AudioController>().PlaySFX("UIButtonClick");
        StartCoroutine(ClosePanelAnimation(transform.parent.Find("Menu").Find("Main").Find("LevelDetails").Find("AlertLocked")));
    }
}
