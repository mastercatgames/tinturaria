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
            oneStarCoins = twoStarCoins = threeStarCoins = 100;

            if (level > 1)
            {
                uiController.timeRemaining = 60f;
            }
            else
            {
                //1-1 is a tutorial
                //TODO: Make a tutorial design
                uiController.isTutorial = true;
                uiController.timeText.gameObject.SetActive(false);
            }
        }
        else if (world == 2)
        {
            uiController.timeRemaining = 126f;
            SetBrokenBottlesPosition(1);

            oneStarCoins = 100;
            twoStarCoins = 200;
            threeStarCoins = 300;
        }
        else if (world == 3)
        {
            uiController.timeRemaining = 120f;
            SetBrokenBottlesPosition(1);

            oneStarCoins = 100;
            twoStarCoins = 200;
            threeStarCoins = 300;
        }
        else if (world == 4)
        {
            uiController.timeRemaining = 150f;
            SetBrokenBottlesPosition(2);

            oneStarCoins = 200;
            twoStarCoins = 300;
            threeStarCoins = 400;
        }
        else if (world == 5)
        {
            uiController.timeRemaining = 176f;
            SetBrokenBottlesPosition(2);

            oneStarCoins = 300;
            twoStarCoins = 400;
            threeStarCoins = 500;
        }
        else if (world == 6)
        {
            uiController.timeRemaining = 177f;
            SetBrokenBottlesPosition(2);

            oneStarCoins = 300;
            twoStarCoins = 400;
            threeStarCoins = 500;
        }
        else if (world == 7)
        {
            uiController.timeRemaining = 162f;
            SetBrokenBottlesPosition(3);

            oneStarCoins = 300;
            twoStarCoins = 400;
            threeStarCoins = 500;
        }
        else if (world == 8)
        {
            uiController.timeRemaining = 186f;
            SetBrokenBottlesPosition(3);

            oneStarCoins = 400;
            twoStarCoins = 500;
            threeStarCoins = 600;
        }
        else if (world == 9)
        {
            uiController.timeRemaining = 180f;
            SetBrokenBottlesPosition(3);

            oneStarCoins = 400;
            twoStarCoins = 500;
            threeStarCoins = 600;
        }
        else if (world == 10)
        {
            uiController.timeRemaining = 174f;
            SetBrokenBottlesPosition(4);

            oneStarCoins = 400;
            twoStarCoins = 500;
            threeStarCoins = 600;
        }
        else if (world == 11)
        {
            uiController.timeRemaining = 200f;
            SetBrokenBottlesPosition(4);

            oneStarCoins = 500;
            twoStarCoins = 600;
            threeStarCoins = 700;
        }
        else if (world == 12)
        {
            uiController.timeRemaining = 193f;
            SetBrokenBottlesPosition(4);

            oneStarCoins = 500;
            twoStarCoins = 600;
            threeStarCoins = 700;
        }
        else if (world == 13)
        {
            uiController.timeRemaining = 210f;
            SetBrokenBottlesPosition(4);

            oneStarCoins = 600;
            twoStarCoins = 700;
            threeStarCoins = 800;
        }
        else if (world == 14)
        {
            if (level == 1)
                uiController.timeRemaining = 202f;
            else if (level == 2)
                uiController.timeRemaining = 194f;
            else if (level == 3)
                uiController.timeRemaining = 186f;

            SetBrokenBottlesPosition(4);

            oneStarCoins = 600;
            twoStarCoins = 700;
            threeStarCoins = 800;
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
