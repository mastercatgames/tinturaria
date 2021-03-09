using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedRewards : MonoBehaviour
{
    private DateTime currentDateTime, timedReward;
    private bool isWorldTimeAPIReadyToUse;
    public Text TimerText, TimerTextSecundary, WatchText;
    public GameObject ExclamationIcon;
    // Start is called before the first frame update
    public static TimedRewards Instance;

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

    void Start()
    {
        StartCoroutine(isWorldTimeAPIReady());
    }

    // Update is called once per frame
    void Update()
    {
        if (isWorldTimeAPIReadyToUse && WorldTimeAPI.Instance.IsTimeLodaed)
        {
            GetTimedRewardDiff();
        }
    }

    private void GetTimedRewardDiff()
    {
        TimeSpan diff = timedReward - WorldTimeAPI.Instance.GetCurrentDateTime();
        TimeSpan timeZero = new TimeSpan(0, 0, 0, 0);

        if (diff <= timeZero)
        {
            diff = timeZero;
        }

        TimerText.text = String.Format("{0:0}m:{1:0}s", diff.Minutes, diff.Seconds);

        if (TimerTextSecundary != null)
            TimerTextSecundary.text = TimerText.text;

        if (diff > timeZero)
            SetActiveWatchButton(false);
        else
            SetActiveWatchButton(true);
    }

    private void SetActiveWatchButton(bool TrueOrFalse)
    {
        WatchText.transform.parent.GetComponent<Button>().interactable = TrueOrFalse;
        TimerText.gameObject.SetActive(!TrueOrFalse);
        WatchText.gameObject.SetActive(TrueOrFalse);
        ExclamationIcon.SetActive(TrueOrFalse);
        ExclamationIcon.transform.parent.parent.Find("SunRay").gameObject.SetActive(TrueOrFalse);

        // if (TimerTextSecundary != null)
        //     TimerTextSecundary.gameObject.SetActive(!TrueOrFalse);
    }

    IEnumerator isWorldTimeAPIReady()
    {
        //print("Verify isStarted...");
        yield return new WaitUntil(() => WorldTimeAPI.Instance.isStarted);
        //print("IS STARTED!");

        if (PlayerPrefs.GetString("timedReward") == "")
        {
            ResetTimer();
        }
        else
        {
            timedReward = DateTime.Parse(PlayerPrefs.GetString("timedReward"));
        }

        //timedReward = currentDateTime.AddMinutes(20);
        isWorldTimeAPIReadyToUse = true;
    }

    public void ResetTimer()
    {
        currentDateTime = WorldTimeAPI.Instance.GetCurrentDateTime();
        timedReward = currentDateTime.AddMinutes(30);
        PlayerPrefs.SetString("timedReward", timedReward.ToString());
    }
}
