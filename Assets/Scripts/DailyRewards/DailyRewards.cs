using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewards : MonoBehaviour
{
    public static DailyRewards Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private int connectAttempts;
    //public bool isDebug;

    // [Header("Local Date Debug")]
    public bool isLocalDate, isUsingTimedReward;
    public int SimulateNextXDays;
    [HideInInspector]
    public Text CurrentDate_debug, LAST_DAY_REWARD_debug, NEXT_DAY_REWARD_debug, DATE_REWARD_debug, Connectivity_debug;
    private DateTime DATE_REWARD, currentDateTime, limitDateTime;

    [HideInInspector]
    public UIController uiController;
    private Transform rewardsContent;
    public Transform DailyRewardsDebugTexts;

    // Start is called before the first frame update
    void Start()
    {
        uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
        rewardsContent = uiController.menu.transform.Find("DailyRewards").Find("Content");

        if (DailyRewardsDebugTexts != null)
        {
            DailyRewardsDebugTexts.Find("Content").gameObject.SetActive(true);
            Connectivity_debug = DailyRewardsDebugTexts.Find("Content").Find("Connectivity").GetComponent<Text>();
            CurrentDate_debug = DailyRewardsDebugTexts.Find("Content").Find("CurrentDate").GetComponent<Text>();
            LAST_DAY_REWARD_debug = DailyRewardsDebugTexts.Find("Content").Find("LAST_DAY_REWARD").GetComponent<Text>();
            NEXT_DAY_REWARD_debug = DailyRewardsDebugTexts.Find("Content").Find("NEXT_DAY_REWARD").GetComponent<Text>();
            DATE_REWARD_debug = DailyRewardsDebugTexts.Find("Content").Find("DATE_REWARD").GetComponent<Text>();
        }

        Invoke("InitReward", 2f);
    }

    void Update()
    {
        if (isLocalDate)
        {
            currentDateTime = DateTime.Now;
        }
        else if (WorldTimeAPI.Instance.IsTimeLodaed)
        {
            currentDateTime = WorldTimeAPI.Instance.GetCurrentDateTime();
        }

        SetDebugText(CurrentDate_debug, currentDateTime.ToString(), false);
    }

    private void InitReward()
    {
        if (isLocalDate)
        {
            currentDateTime = DateTime.Now;
            currentDateTime = currentDateTime.AddDays(SimulateNextXDays);
            uiController.SetLoading(false);
        }
        else if (WorldTimeAPI.Instance.IsTimeLodaed)
        {
            currentDateTime = WorldTimeAPI.Instance.GetCurrentDateTime();
            // SetDebugText(CurrentDate_debug, currentDateTime.ToString());
            //SetDebugText(Connectivity_debug, "Connected!", false);
            uiController.SetLoading(false);
            connectAttempts = 0;
        }
        else
        {
            if (connectAttempts < 3)
            {
                connectAttempts++;
                SetDebugText(Connectivity_debug, "Trying to connect (Attempt " + connectAttempts + ")...", false);
                WorldTimeAPI.Instance.Reload();
                Invoke("InitReward", 2f);
                return;
            }
            else
            {
                DailyRewards.Instance.ShowNoInternetAlert();
                WorldTimeAPI.Instance.isStarted = true;
                return;
            }
        }

        WorldTimeAPI.Instance.isStarted = true;

        limitDateTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0);

        if (PlayerPrefs.GetString("DATE_REWARD") != "")
            DATE_REWARD = Convert.ToDateTime(PlayerPrefs.GetString("DATE_REWARD"));

        if (PlayerPrefs.GetInt("LAST_DAY_REWARD") == 0)
        {
            PlayerPrefs.SetInt("LAST_DAY_REWARD", 1);
            PlayerPrefs.SetInt("NEXT_DAY_REWARD", 1);
            PlayerPrefs.SetString("DATE_REWARD", currentDateTime.ToString("YYYY-MM-dd"));
            DATE_REWARD = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, 0, 0, 0);
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
                    PlayerPrefs.SetInt("PowerUp_BoosterFilling_AllBottles", PlayerPrefs.GetInt("PowerUp_BoosterFilling_AllBottles") + 1);
                    break;
                case 2:
                    PlayerPrefs.SetInt("PowerUp_FixInTime", PlayerPrefs.GetInt("PowerUp_FixInTime") + 1);
                    break;
                case 3:
                    PlayerPrefs.SetInt("PowerUp_BoosterFilling_OneBottle", PlayerPrefs.GetInt("PowerUp_BoosterFilling_OneBottle") + 1);
                    break;
                case 4:
                    PlayerPrefs.SetInt("PowerUp_NoBrokenBottles", PlayerPrefs.GetInt("PowerUp_NoBrokenBottles") + 1);
                    break;
                case 5:
                    PlayerPrefs.SetInt("PowerUp_BoosterFilling_Box", PlayerPrefs.GetInt("PowerUp_BoosterFilling_Box") + 1);
                    break;
                case 6:
                    PlayerPrefs.SetInt("PowerUp_DoubleCash", PlayerPrefs.GetInt("PowerUp_DoubleCash") + 1);
                    break;
                case 7:
                    PlayerPrefs.SetInt("PowerUp_DoubleCash", PlayerPrefs.GetInt("PowerUp_DoubleCash") + 1);
                    PlayerPrefs.SetInt("PowerUp_FixInTime", PlayerPrefs.GetInt("PowerUp_FixInTime") + 1);
                    PlayerPrefs.SetInt("PowerUp_BoosterFilling_OneBottle", PlayerPrefs.GetInt("PowerUp_BoosterFilling_OneBottle") + 1);
                    PlayerPrefs.SetInt("PowerUp_NoBrokenBottles", PlayerPrefs.GetInt("PowerUp_NoBrokenBottles") + 1);
                    PlayerPrefs.SetInt("PowerUp_BoosterFilling_Box", PlayerPrefs.GetInt("PowerUp_BoosterFilling_Box") + 1);
                    PlayerPrefs.SetInt("PowerUp_BoosterFilling_AllBottles", PlayerPrefs.GetInt("PowerUp_BoosterFilling_AllBottles") + 1);
                    break;
            }

            //print("REWARD DAY " + PlayerPrefs.GetInt("LAST_DAY_REWARD") + "!");
        }

        if (DailyRewardsDebugTexts != null)
        {
            SetDebugText(LAST_DAY_REWARD_debug, PlayerPrefs.GetInt("LAST_DAY_REWARD").ToString());
            SetDebugText(NEXT_DAY_REWARD_debug, PlayerPrefs.GetInt("NEXT_DAY_REWARD").ToString());
            SetDebugText(DATE_REWARD_debug, PlayerPrefs.GetString("DATE_REWARD"));
        }
    }

    public void SetDebugText(Text debugText, string text, bool increment = true)
    {
        if (DailyRewardsDebugTexts != null && debugText != null)
        {
            if (increment)
                debugText.text += text;
            else
                debugText.text = text;
        }
    }

    public void ShowNoInternetAlert()
    {
        uiController.SetLoading(false);
        uiController.NoInternetAlert.SetActive(true);
    }
}
