using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewards : MonoBehaviour
{
    public Text datetimeText;
    private DateTime DATE_REWARD;
    private DateTime currentDateTime;
    private DateTime limitDateTime;
    private UIController uiController;
    private Transform rewardsContent;

    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        rewardsContent = uiController.menu.transform.Find("DailyRewards").Find("Content");

        currentDateTime = DateTime.Now;

        //Test to add Days
        //currentDateTime = currentDateTime.AddDays(7);

        limitDateTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0);

        InitReward();
    }

    void Update()
    {
        datetimeText.text = currentDateTime.ToString();

        // if (WorldTimeAPI.Instance.IsTimeLodaed) 
        //{
        // 	DateTime currentDateTime = WorldTimeAPI.Instance.GetCurrentDateTime();
        // 	datetimeText.text = currentDateTime.ToString();
        // }
    }

    private void InitReward()
    {
        if (PlayerPrefs.GetString("DATE_REWARD") != "")
            DATE_REWARD = Convert.ToDateTime(PlayerPrefs.GetString("DATE_REWARD"));

        if (PlayerPrefs.GetInt("LAST_DAY_REWARD") == 0)
        {
            PlayerPrefs.SetInt("LAST_DAY_REWARD", 1);
            PlayerPrefs.SetInt("NEXT_DAY_REWARD", 1);
            PlayerPrefs.SetString("DATE_REWARD", currentDateTime.ToString("YYYY-MM-dd"));
            DATE_REWARD = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0); ;
        }
        
        var hours = (limitDateTime - DATE_REWARD).TotalHours;

        if (hours >= 48)
        {
            //Reset
            PlayerPrefs.SetInt("LAST_DAY_REWARD", 1);
            PlayerPrefs.SetInt("NEXT_DAY_REWARD", 1);
        }
        else if (hours == 24 && hours < 48)
        {
            if (PlayerPrefs.GetInt("LAST_DAY_REWARD") == 7)
            {
                PlayerPrefs.SetInt("LAST_DAY_REWARD", 1);
                PlayerPrefs.SetInt("NEXT_DAY_REWARD", 1);
            }
            else
            {
                PlayerPrefs.SetInt("LAST_DAY_REWARD", PlayerPrefs.GetInt("LAST_DAY_REWARD") + 1);
            }
        }

        VerifyReward();
    }

    private void VerifyReward()
    {
        if (PlayerPrefs.GetInt("NEXT_DAY_REWARD") == PlayerPrefs.GetInt("LAST_DAY_REWARD"))
        {
            PlayerPrefs.SetInt("NEXT_DAY_REWARD", PlayerPrefs.GetInt("NEXT_DAY_REWARD") + 1);
            PlayerPrefs.SetString("DATE_REWARD", currentDateTime.ToString("yyyy-MM-dd"));

            if (PlayerPrefs.GetInt("LAST_DAY_REWARD") == 1)
            {
                rewardsContent.Find("GridLayoutGroup").Find("Day1").Find("Check").gameObject.SetActive(true);
            }
            else
            {
                for (int i = 1; i <= PlayerPrefs.GetInt("LAST_DAY_REWARD"); i++)
                {
                    if (i == 7)
                    {
                        rewardsContent.Find("Day7").Find("Check").gameObject.SetActive(true);
                        //Reset
                        PlayerPrefs.SetInt("LAST_DAY_REWARD", 7);
                        PlayerPrefs.SetInt("NEXT_DAY_REWARD", 1);
                    }
                    else
                    {
                        rewardsContent.Find("GridLayoutGroup").Find("Day" + i).Find("Check").gameObject.SetActive(true);
                    }
                }
            }

            uiController.OpenDailyRewards();

            switch (PlayerPrefs.GetInt("LAST_DAY_REWARD"))
            {
                case 1:
                    //Give the reward
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
            }

            print("REWARD DAY " + PlayerPrefs.GetInt("LAST_DAY_REWARD") + "!");
        }
    }
}
