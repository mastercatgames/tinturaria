using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private UIController uiController;
    public int world;
    public int level;
    public int oneStarCoins;
    public int twoStarCoins;
    public int threeStarCoins;
    public List<int> requestedColorsPosition;
    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();

        world = int.Parse(SceneManager.GetActiveScene().name.Split('_')[0]);
        level = int.Parse(SceneManager.GetActiveScene().name.Split('_')[1]);

        SetLevelVariables();
    }

    private void SetLevelVariables()
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
            SetBrokenBottlesPosition(1);

            oneStarCoins = 100;
            twoStarCoins = 200;
            threeStarCoins = 300;
        }
        else if (world == 3)
        {
            uiController.timeRemaining = 120f;
            SetBrokenBottlesPosition(1);

            oneStarCoins = 200;
            twoStarCoins = 300;
            threeStarCoins = 400;
        }
        else if (world == 4)
        {
            uiController.timeRemaining = 150f;
            SetBrokenBottlesPosition(2);

            oneStarCoins = 300;
            twoStarCoins = 400;
            threeStarCoins = 500;
        }
        else if (world == 5)
        {
            uiController.timeRemaining = 170f;
            SetBrokenBottlesPosition(2);

            oneStarCoins = 400;
            twoStarCoins = 500;
            threeStarCoins = 600;
        }
        else if (world == 6)
        {
            uiController.timeRemaining = 170f;
            SetBrokenBottlesPosition(2);

            oneStarCoins = 500;
            twoStarCoins = 600;
            threeStarCoins = 700;
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
}
