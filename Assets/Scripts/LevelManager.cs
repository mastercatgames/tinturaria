using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class LevelManager : MonoBehaviour
{
    private UIController uiController;
    public int world;
    public int level;
    public int oneStarCoins;
    public int twoStarCoins;
    public int threeStarCoins;
    public List<int> requestedColorsPosition;
    public string path;

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
        public List<int> orderColorsToBreak;

        //Dynamic variables
        // public int starsEarned;
        // public int starsToUnlock;
    }

    public List<Level> levelListGlobal;

    [System.Serializable]
    public class LevelList
    {
        public List<Level> Levels;
    }

    [System.Serializable]
    public class LevelProgress
    {
        //Dynamic variables
        public int starsEarned;
        public int starsToUnlock;
    }

    public List<Level> levelProgressListGlobal;

    [System.Serializable]
    public class LevelProgressList
    {
        public List<Level> Levels;
    }


    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();

        world = int.Parse(SceneManager.GetActiveScene().name.Split('_')[0]);
        level = int.Parse(SceneManager.GetActiveScene().name.Split('_')[1]);

        SetSelectedLevelVariables();


        path = Path.Combine(Application.dataPath, "LevelData.json");

        // levelListGlobal = new List<Level>();

        //Code to generate JSON file
        //World        
        for (int i = 1; i <= 14; i++)
        {
            //Level
            for (int y = 1; y <= 3; y++)
            {
                // if (i < 5)
                GetLevelProperties(i, y);
            }
        }

        // LoadLevelDataJSON();
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
        if (world == 1)
        {
            uiController.timeRemaining = 60f;
            oneStarCoins = twoStarCoins = threeStarCoins = 100;

            if (level == 1)
            {
                uiController.isTutorial = true;
            }
        }
        else if (world == 2)
        {
            uiController.timeRemaining = 120f;

            if (level == 1)
            {
                uiController.isToolTutorial = true;
                requestedColorsPosition.Add(1);
            }
            else
            {
                SetBrokenBottlesPosition(1);
            }

            oneStarCoins = 100;
            twoStarCoins = 200;
            threeStarCoins = 300;

            if (level == 3 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 0)
            {
                uiController.isPowerUpTutorial = true;
            }
        }
        else if (world == 3)
        {
            uiController.timeRemaining = 120f;
            SetBrokenBottlesPosition(1);

            oneStarCoins = 200;
            twoStarCoins = 300;
            threeStarCoins = 400;

            if (level == 2 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 1)
            {
                uiController.isPowerUpTutorial = true;
            }
        }
        else if (world == 4)
        {
            uiController.timeRemaining = 150f;
            SetBrokenBottlesPosition(2);

            oneStarCoins = 300;
            twoStarCoins = 400;
            threeStarCoins = 500;

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
            uiController.timeRemaining = 170f;
            SetBrokenBottlesPosition(2);

            oneStarCoins = 400;
            twoStarCoins = 500;
            threeStarCoins = 600;

            if (level == 2 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 4)
            {
                uiController.isPowerUpTutorial = true;
            }
        }
        else if (world == 6)
        {
            uiController.timeRemaining = 170f;
            SetBrokenBottlesPosition(2);

            oneStarCoins = 500;
            twoStarCoins = 600;
            threeStarCoins = 700;

            if (level == 1 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 5)
            {
                uiController.isPowerUpTutorial = true;
            }
        }
        else if (world == 7)
        {
            uiController.timeRemaining = 160f;
            SetBrokenBottlesPosition(3);

            oneStarCoins = 600;
            twoStarCoins = 700;
            threeStarCoins = 800;
        }
        else if (world == 8)
        {
            uiController.timeRemaining = 180f;
            SetBrokenBottlesPosition(3);

            oneStarCoins = 600;
            twoStarCoins = 700;
            threeStarCoins = 800;
        }
        else if (world == 9)
        {
            uiController.timeRemaining = 180f;
            SetBrokenBottlesPosition(3);

            oneStarCoins = 700;
            twoStarCoins = 800;
            threeStarCoins = 900;
        }
        else if (world == 10)
        {
            uiController.timeRemaining = 170f;
            SetBrokenBottlesPosition(4);

            oneStarCoins = 700;
            twoStarCoins = 800;
            threeStarCoins = 900;
        }
        else if (world == 11)
        {
            uiController.timeRemaining = 200f;
            SetBrokenBottlesPosition(4);

            oneStarCoins = 800;
            twoStarCoins = 900;
            threeStarCoins = 1000;
        }
        else if (world == 12)
        {
            uiController.timeRemaining = 190f;
            SetBrokenBottlesPosition(4);

            oneStarCoins = 800;
            twoStarCoins = 900;
            threeStarCoins = 1000;
        }
        else if (world == 13)
        {
            uiController.timeRemaining = 210f;
            SetBrokenBottlesPosition(4);

            oneStarCoins = 2000;
            twoStarCoins = 2100;
            threeStarCoins = 2200;
        }
        else if (world == 14)
        {
            if (level == 1)
                uiController.timeRemaining = 200f;
            else if (level == 2)
                uiController.timeRemaining = 200f;
            else if (level == 3)
                uiController.timeRemaining = 200f;

            SetBrokenBottlesPosition(4);

            oneStarCoins = 2900;
            twoStarCoins = 3000;
            threeStarCoins = 3100;
        }
    }

    public void SetBrokenBottlesPosition(int howMany, Level level = null)
    {
        if (level != null)
        {
            for (int i = 1; i <= howMany; i++)
            {
                if (i == 1)
                    level.orderColorsToBreak.Add(Random.Range(1, 3)); //a < b  (1 to 2)
                else if (i == 2)
                    level.orderColorsToBreak.Add(Random.Range(3, 5)); //a < b  (3 to 4)
                else if (i == 3)
                    level.orderColorsToBreak.Add(Random.Range(5, 8)); //a < b  (5 to 7)
                else if (i == 4)
                    level.orderColorsToBreak.Add(Random.Range(8, 17)); //a < b  (8 to 16)
            }
        }
        else
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
    }

    //Code to generate JSON file
    public Level GetLevelProperties(int world, int level)
    {
        Level levelProprierties = new Level();

        levelProprierties.world = world;
        levelProprierties.level = level;

        levelProprierties.orderColorsToBreak = new List<int>();

        if (world == 1)
        {
            levelProprierties.time = 60f;
            levelProprierties.oneStarCoins = levelProprierties.twoStarCoins = levelProprierties.threeStarCoins = 100;

            if (level == 1)
            {
                uiController.isTutorial = true;
            }
        }
        else if (world == 2)
        {
            levelProprierties.time = 120f;

            if (level == 1)
            {
                uiController.isToolTutorial = true;
                levelProprierties.orderColorsToBreak.Add(1);
            }
            else
            {
                SetBrokenBottlesPosition(1, levelProprierties);
            }

            levelProprierties.oneStarCoins = 100;
            levelProprierties.twoStarCoins = 200;
            levelProprierties.threeStarCoins = 300;

            if (level == 3 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 0)
            {
                uiController.isPowerUpTutorial = true;
            }
        }
        else if (world == 3)
        {
            levelProprierties.time = 120f;
            SetBrokenBottlesPosition(1, levelProprierties);

            levelProprierties.oneStarCoins = 200;
            levelProprierties.twoStarCoins = 300;
            levelProprierties.threeStarCoins = 400;

            if (level == 2 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 1)
            {
                uiController.isPowerUpTutorial = true;
            }
        }
        else if (world == 4)
        {
            levelProprierties.time = 150f;
            SetBrokenBottlesPosition(2, levelProprierties);

            levelProprierties.oneStarCoins = 300;
            levelProprierties.twoStarCoins = 400;
            levelProprierties.threeStarCoins = 500;

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
            levelProprierties.time = 170f;
            SetBrokenBottlesPosition(2, levelProprierties);

            levelProprierties.oneStarCoins = 400;
            levelProprierties.twoStarCoins = 500;
            levelProprierties.threeStarCoins = 600;

            if (level == 2 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 4)
            {
                uiController.isPowerUpTutorial = true;
            }
        }
        else if (world == 6)
        {
            levelProprierties.time = 170f;
            SetBrokenBottlesPosition(2, levelProprierties);

            levelProprierties.oneStarCoins = 500;
            levelProprierties.twoStarCoins = 600;
            levelProprierties.threeStarCoins = 700;

            if (level == 1 && PlayerPrefs.GetInt("PowerUpsTutorial_Step") == 5)
            {
                uiController.isPowerUpTutorial = true;
            }
        }
        else if (world == 7)
        {
            levelProprierties.time = 160f;
            SetBrokenBottlesPosition(3, levelProprierties);

            levelProprierties.oneStarCoins = 600;
            levelProprierties.twoStarCoins = 700;
            levelProprierties.threeStarCoins = 800;
        }
        else if (world == 8)
        {
            levelProprierties.time = 180f;
            SetBrokenBottlesPosition(3, levelProprierties);

            levelProprierties.oneStarCoins = 600;
            levelProprierties.twoStarCoins = 700;
            levelProprierties.threeStarCoins = 800;
        }
        else if (world == 9)
        {
            levelProprierties.time = 180f;
            SetBrokenBottlesPosition(3, levelProprierties);

            levelProprierties.oneStarCoins = 700;
            levelProprierties.twoStarCoins = 800;
            levelProprierties.threeStarCoins = 900;
        }
        else if (world == 10)
        {
            levelProprierties.time = 170f;
            SetBrokenBottlesPosition(4, levelProprierties);

            levelProprierties.oneStarCoins = 700;
            levelProprierties.twoStarCoins = 800;
            levelProprierties.threeStarCoins = 900;
        }
        else if (world == 11)
        {
            levelProprierties.time = 200f;
            SetBrokenBottlesPosition(4, levelProprierties);

            levelProprierties.oneStarCoins = 800;
            levelProprierties.twoStarCoins = 900;
            levelProprierties.threeStarCoins = 1000;
        }
        else if (world == 12)
        {
            levelProprierties.time = 190f;
            SetBrokenBottlesPosition(4, levelProprierties);

            levelProprierties.oneStarCoins = 800;
            levelProprierties.twoStarCoins = 900;
            levelProprierties.threeStarCoins = 1000;
        }
        else if (world == 13)
        {
            levelProprierties.time = 210f;
            SetBrokenBottlesPosition(4, levelProprierties);

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

            SetBrokenBottlesPosition(4, levelProprierties);

            levelProprierties.oneStarCoins = 2900;
            levelProprierties.twoStarCoins = 3000;
            levelProprierties.threeStarCoins = 3100;
        }

        // PlayerData playerData = new PlayerData();
        // playerData.teste1 = 111;
        // playerData.teste1 = 222;

        // Save(levelProprierties);
        // print("json: " + JsonUtility.ToJson(playerData));
        // print("json: " + JsonUtility.ToJson(levelProprierties));

        levelListGlobal.Add(levelProprierties);

        if (world == 14 && level == 3)
        {
            var ql = new LevelList();
            ql.Levels = levelListGlobal;
            // print("json: " + JsonUtility.ToJson(ql));
            Save(ql);
        }

        return levelProprierties;
    }
}
