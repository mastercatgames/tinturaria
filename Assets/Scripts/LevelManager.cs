using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class LevelManager : MonoBehaviour
{
    private UIController uiController;

    [Header("=== Selected Level ===")]
    public int world;
    public int level;
    public int oneStarCoins;
    public int twoStarCoins;
    public int threeStarCoins;
    public int highscore;
    public List<int> requestedColorsPosition;
    private string path;
    private string pathLevelProgress;

    [System.Serializable]
    public class Level
    {
        //Fixed variables
        public int world;
        public int level;
        public int oneStarCoins;
        public int twoStarCoins;
        public int threeStarCoins;
        public float time;
        public string color;
        public int starsToUnlock;
        // public List<int> orderColorsToBreak;
        public int qtyColorsToBreak;
    }
    private List<Level> levelListGlobal;

    [System.Serializable]
    public class LevelList
    {
        public List<Level> Levels;
    }

    [System.Serializable]
    public class LevelProgress
    {
        public int world;
        public int level;
        public int starsEarned;
        public int highscore;
    }

    private List<LevelProgress> levelProgressListGlobal;

    [System.Serializable]
    public class LevelProgressList
    {
        public List<LevelProgress> LevelsProgress;
    }


    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();

        if (PlayerPrefs.GetString("_LevelProgress") == "")
        {
            //Init the Level Progress save data
            PlayerPrefs.SetString("_LevelProgress", File.ReadAllText(Application.dataPath + "/LevelProgressRaw.json"));
        }  

        SetSelectedLevelVariables();           

        //UTIL: Code to generate JSON file
        // path = Path.Combine(Application.dataPath, "LevelData.json");
        // pathLevelProgress = Path.Combine(Application.dataPath, "LevelProgressRaw.json");

        // levelListGlobal = new List<Level>();
        // levelProgressListGlobal = new List<LevelProgress>();
        // //World        
        // for (int i = 1; i <= 14; i++)
        // {
        //     //Level
        //     for (int y = 1; y <= 3; y++)
        //     {
        //         CreateLevelsJSON(i, y, true);
        //     }
        // }
    }

    public void Save(LevelList levelList)
    {
        string json = JsonUtility.ToJson(levelList);
        File.WriteAllText(path, json);
    }

    public void LoadLevelDataJSON()
    {
        string json = File.ReadAllText(Application.dataPath + "/LevelData.json");
        LevelList levelList = JsonUtility.FromJson<LevelList>(json);

        foreach (Level item in levelList.Levels)
        {
            print(item.time);
        }
    }

    private void SetSelectedLevelVariables()
    {
        world = int.Parse(SceneManager.GetActiveScene().name.Split('_')[0]);
        level = int.Parse(SceneManager.GetActiveScene().name.Split('_')[1]);

        string LevelDataJSON = File.ReadAllText(Application.dataPath + "/LevelData.json");
        Level selectedLevel = GetLevelProperties(world, level, LevelDataJSON);//JsonUtility.FromJson<LevelList>(LevelDataJSON).Levels.Find(c => c.world == world && c.level == level);

        oneStarCoins = selectedLevel.oneStarCoins;
        twoStarCoins = selectedLevel.twoStarCoins;
        threeStarCoins = selectedLevel.threeStarCoins;
        highscore = JsonUtility.FromJson<LevelProgressList>(PlayerPrefs.GetString("_LevelProgress")).LevelsProgress.Find(c => c.world == world && c.level == level).highscore;

        uiController.timeRemaining = selectedLevel.time;
        SetBrokenBottlesPosition(selectedLevel.qtyColorsToBreak);

        //Conditions
        if (world == 1 && level == 1)
        {
            uiController.isTutorial = true;
        }
        else if (world == 2)
        {
            if (level == 1)
            {
                uiController.isToolTutorial = true;
                requestedColorsPosition.Add(1);
            }
            else if (level == 3 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 0)
            {
                uiController.isPowerUpTutorial = true;
            }
        }
        else if (world == 3)
        {
            if (level == 2 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 1)
            {
                uiController.isPowerUpTutorial = true;
            }
        }
        else if (world == 4)
        {
            if (level == 1 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 2)
            {
                uiController.isPowerUpTutorial = true;
            }
            else if (level == 3 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 3)
            {
                uiController.isPowerUpTutorial = true;
            }
        }
        else if (world == 5)
        {
            if (level == 2 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 4)
            {
                uiController.isPowerUpTutorial = true;
            }
        }
        else if (world == 6)
        {
            if (level == 1 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 5)
            {
                uiController.isPowerUpTutorial = true;
            }
        }
    }

    public Level GetLevelProperties(int world, int level, string LevelDataJSON)
    {
        // string LevelDataJSON = File.ReadAllText(Application.dataPath + "/LevelData.json");
        Level levelProperties = JsonUtility.FromJson<LevelList>(LevelDataJSON).Levels.Find(c => c.world == world && c.level == level);

        return levelProperties;
    }

    public void SetLevelProgress(LevelProgress levelProgress)
    {
        string levelProgressJSON = PlayerPrefs.GetString("_LevelProgress");

        LevelProgressList levelsProgress = JsonUtility.FromJson<LevelProgressList>(levelProgressJSON);
        LevelProgress levelToUpdate = levelsProgress.LevelsProgress.Find(c => c.world == levelProgress.world && c.level == levelProgress.level);

        //Update data
        if (levelProgress.starsEarned > levelToUpdate.starsEarned)
        {
            levelToUpdate.starsEarned = levelProgress.starsEarned;
        }

        if (levelProgress.highscore > levelToUpdate.highscore)
        {
            levelToUpdate.highscore = levelProgress.highscore;
        }

        PlayerPrefs.SetString("_LevelProgress", JsonUtility.ToJson(levelsProgress));

        print(JsonUtility.ToJson(levelsProgress));
    }

    public void SetBrokenBottlesPosition(int howMany)
    {
        for (int i = 1; i <= howMany; i++)
        {
            if (i == 1)
                requestedColorsPosition.Add(Random.Range(1, 3)); //a < b  (1 to 2)
            else if (i == 2)
                requestedColorsPosition.Add(Random.Range(3, 5)); //a < b  (3 to 4)
            else if (i == 3)
                requestedColorsPosition.Add(Random.Range(5, 8)); //a < b  (5 to 7)
            else if (i == 4)
                requestedColorsPosition.Add(Random.Range(8, 17)); //a < b  (8 to 16)
        }
    }





    #region Generate JSON file
    //Code to generate JSON file
    public Level CreateLevelsJSON(int world, int level, bool isLevelProgress)
    {
        Level levelProprierties = new Level();
        LevelProgress levelProgress = new LevelProgress();

        levelProprierties.world = levelProgress.world = world;
        levelProprierties.level = levelProgress.level = level;

        if (world == 1)
        {
            levelProprierties.time = 60f;
            levelProprierties.oneStarCoins = levelProprierties.twoStarCoins = levelProprierties.threeStarCoins = 100;

        }
        else if (world == 2)
        {
            levelProprierties.time = 120f;

            levelProprierties.qtyColorsToBreak = 1;

            levelProprierties.oneStarCoins = 100;
            levelProprierties.twoStarCoins = 200;
            levelProprierties.threeStarCoins = 300;
        }
        else if (world == 3)
        {
            levelProprierties.time = 120f;

            levelProprierties.qtyColorsToBreak = 1;

            levelProprierties.oneStarCoins = 200;
            levelProprierties.twoStarCoins = 300;
            levelProprierties.threeStarCoins = 400;
        }
        else if (world == 4)
        {
            levelProprierties.time = 150f;

            levelProprierties.qtyColorsToBreak = 2;

            levelProprierties.oneStarCoins = 300;
            levelProprierties.twoStarCoins = 400;
            levelProprierties.threeStarCoins = 500;
        }
        else if (world == 5)
        {
            levelProprierties.time = 170f;

            levelProprierties.qtyColorsToBreak = 2;

            levelProprierties.oneStarCoins = 400;
            levelProprierties.twoStarCoins = 500;
            levelProprierties.threeStarCoins = 600;
        }
        else if (world == 6)
        {
            levelProprierties.time = 170f;

            levelProprierties.qtyColorsToBreak = 2;

            levelProprierties.oneStarCoins = 500;
            levelProprierties.twoStarCoins = 600;
            levelProprierties.threeStarCoins = 700;
        }
        else if (world == 7)
        {
            levelProprierties.time = 160f;

            levelProprierties.qtyColorsToBreak = 3;

            levelProprierties.oneStarCoins = 600;
            levelProprierties.twoStarCoins = 700;
            levelProprierties.threeStarCoins = 800;
        }
        else if (world == 8)
        {
            levelProprierties.time = 180f;

            levelProprierties.qtyColorsToBreak = 3;

            levelProprierties.oneStarCoins = 600;
            levelProprierties.twoStarCoins = 700;
            levelProprierties.threeStarCoins = 800;
        }
        else if (world == 9)
        {
            levelProprierties.time = 180f;

            levelProprierties.qtyColorsToBreak = 3;

            levelProprierties.oneStarCoins = 700;
            levelProprierties.twoStarCoins = 800;
            levelProprierties.threeStarCoins = 900;
        }
        else if (world == 10)
        {
            levelProprierties.time = 170f;

            levelProprierties.qtyColorsToBreak = 4;

            levelProprierties.oneStarCoins = 700;
            levelProprierties.twoStarCoins = 800;
            levelProprierties.threeStarCoins = 900;
        }
        else if (world == 11)
        {
            levelProprierties.time = 200f;

            levelProprierties.qtyColorsToBreak = 4;

            levelProprierties.oneStarCoins = 800;
            levelProprierties.twoStarCoins = 900;
            levelProprierties.threeStarCoins = 1000;
        }
        else if (world == 12)
        {
            levelProprierties.time = 190f;

            levelProprierties.qtyColorsToBreak = 4;

            levelProprierties.oneStarCoins = 800;
            levelProprierties.twoStarCoins = 900;
            levelProprierties.threeStarCoins = 1000;
        }
        else if (world == 13)
        {
            levelProprierties.time = 210f;

            levelProprierties.qtyColorsToBreak = 4;

            levelProprierties.oneStarCoins = 2000;
            levelProprierties.twoStarCoins = 2100;
            levelProprierties.threeStarCoins = 2200;
        }
        else if (world == 14)
        {
            if (level == 1)
                levelProprierties.time = 200f;
            else if (level == 2)
                levelProprierties.time = 200f;
            else if (level == 3)
                levelProprierties.time = 200f;

            levelProprierties.qtyColorsToBreak = 4;

            levelProprierties.oneStarCoins = 2900;
            levelProprierties.twoStarCoins = 3000;
            levelProprierties.threeStarCoins = 3100;
        }

        levelListGlobal.Add(levelProprierties);
        levelProgressListGlobal.Add(levelProgress);

        if (world == 14 && level == 3)
        {            
            //Level progress JSON
            if (isLevelProgress)
            {
                var ql2 = new LevelProgressList();
                ql2.LevelsProgress = levelProgressListGlobal;
                // print("json: " + JsonUtility.ToJson(ql2));

                string levelProgressJSON = JsonUtility.ToJson(ql2);

                // PlayerPrefs.SetString("_LevelProgress", levelProgressJSON);

                File.WriteAllText(pathLevelProgress, levelProgressJSON);
            }
            else
            {
                //Create Level properties JSON file
                var ql = new LevelList();
                ql.Levels = levelListGlobal;
                // print("json: " + JsonUtility.ToJson(ql));
                Save(ql);
            }
        }

        return levelProprierties;
    }

    #endregion

    // void Update()
    // {
    //     //Teste to set highscore of world 2 level 1 
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         // SetLevelProgress(new LevelManager.LevelProgress()
    //         // {
    //         //     world = 7,
    //         //     level = 1,
    //         //     highscore = 32456,
    //         //     starsEarned = 2
    //         // });

    //         print(PlayerPrefs.GetInt("_LevelProgress222"));

    //         if (PlayerPrefs.GetString("_LevelProgress222") == ""){
    //             print("Não possui _LevelProgress222 cadastrado");
    //         }
    //     }        
    // }
}
