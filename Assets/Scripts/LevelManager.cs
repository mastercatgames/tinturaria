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
                print("Tutorial Level!");
                uiController.timeText.gameObject.SetActive(false);
            }
        }
        else if (world == 2)
        {
            uiController.timeRemaining = 126f;
            requestedColorsPosition.Add(2);

            oneStarCoins = 100;
            twoStarCoins = 200;
            threeStarCoins = 300;
        }
        else if (world == 3)
        {
            uiController.timeRemaining = 120f;
            requestedColorsPosition.Add(2);

            oneStarCoins = 100;
            twoStarCoins = 200;
            threeStarCoins = 300;
        }
        else if (world == 4)
        {
            uiController.timeRemaining = 150f;
            requestedColorsPosition.Add(3);

            oneStarCoins = 200;
            twoStarCoins = 300;
            threeStarCoins = 400;
        }
        else if (world == 5)
        {
            uiController.timeRemaining = 176f;
            requestedColorsPosition.Add(4);

            oneStarCoins = 300;
            twoStarCoins = 400;
            threeStarCoins = 500;
        }
        else if (world == 6)
        {
            uiController.timeRemaining = 177f;
            requestedColorsPosition.Add(2);
            requestedColorsPosition.Add(4);

            oneStarCoins = 300;
            twoStarCoins = 400;
            threeStarCoins = 500;
        }
        else if (world == 7)
        {
            uiController.timeRemaining = 162f;
            requestedColorsPosition.Add(2);
            requestedColorsPosition.Add(3);

            oneStarCoins = 300;
            twoStarCoins = 400;
            threeStarCoins = 500;
        }
        else if (world == 8)
        {
            uiController.timeRemaining = 186f;
            requestedColorsPosition.Add(3);
            requestedColorsPosition.Add(5);

            oneStarCoins = 400;
            twoStarCoins = 500;
            threeStarCoins = 600;
        }
        else if (world == 9)
        {
            uiController.timeRemaining = 180f;
            requestedColorsPosition.Add(1);
            requestedColorsPosition.Add(6);

            oneStarCoins = 400;
            twoStarCoins = 500;
            threeStarCoins = 600;
        }
        else if (world == 10)
        {
            uiController.timeRemaining = 174f;
            requestedColorsPosition.Add(2);
            requestedColorsPosition.Add(4);

            oneStarCoins = 400;
            twoStarCoins = 500;
            threeStarCoins = 600;
        }
        else if (world == 11)
        {
            uiController.timeRemaining = 200f;
            requestedColorsPosition.Add(1);
            requestedColorsPosition.Add(3);
            requestedColorsPosition.Add(6);

            oneStarCoins = 500;
            twoStarCoins = 600;
            threeStarCoins = 700;
        }
        else if (world == 12)
        {
            uiController.timeRemaining = 193f;
            requestedColorsPosition.Add(1);
            requestedColorsPosition.Add(3);
            requestedColorsPosition.Add(5);

            oneStarCoins = 500;
            twoStarCoins = 600;
            threeStarCoins = 700;
        }
        else if (world == 13)
        {
            uiController.timeRemaining = 210f;
            requestedColorsPosition.Add(2);
            requestedColorsPosition.Add(4);
            requestedColorsPosition.Add(7);

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

            requestedColorsPosition.Add(1);
            requestedColorsPosition.Add(3);
            requestedColorsPosition.Add(8);

            oneStarCoins = 600;
            twoStarCoins = 700;
            threeStarCoins = 800;
        }
    }
}
