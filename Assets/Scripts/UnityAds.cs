using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using UnityEngine.Purchasing;

public class UnityAds : MonoBehaviour
{
    private UIController uiController;
    private string gameId = "4012557";
    private bool testMode = true;

    void Start()
    {
        uiController = FindObjectOfType<UIController>();

        if (PlayerPrefs.GetInt("removeAds") == 0)
        {
            Advertisement.Initialize(gameId, testMode);
            StartCoroutine(ShowBannerWhenInitialized());
        }
    }

    IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
        {
            //Give 3 seconds to process restore purchase
            yield return new WaitForSeconds(3f);
        }
        if (PlayerPrefs.GetInt("removeAds") == 0)
        {
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            Advertisement.Banner.Show("BottomBanner");
        }
    }

    public void ShowSkippableVideoAd(string buttonName)
    {
        // var options = new ShowOptions { resultCallback = AfterShowMenuButtonAd };
        ShowOptions options = new ShowOptions();// { resultCallback = AfterShowMenuButtonAd };

        if (buttonName == "Menu")
            options.resultCallback = AfterShowMenuButtonAd;
        else if (buttonName == "NextLevel")
            options.resultCallback = AfterShowNextLevelButtonAd;

        Advertisement.Show("video", options);
    }

    public void AfterShowMenuButtonAd(ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            //print("Menu Ad was finished and can Restart Game now");
            uiController.RestartGame();
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            //print("Menu Ad was skipped and can Restart Game now");
            uiController.RestartGame();
        }
        else if (showResult == ShowResult.Failed)
        {
            //Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void AfterShowNextLevelButtonAd(ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            //print("Next Level Ad was finished and can go to Next Level now");
            uiController.LoadNextLevel();
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
            //print("Next Level Ad was skipped and can go to Next Level now");
            uiController.LoadNextLevel();
        }
        else if (showResult == ShowResult.Failed)
        {
            //Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void ShowTimedRewardVideo()
    {
        //var options = new ShowOptions { resultCallback = OnUnityAdsDidFinish };
        ShowOptions options = new ShowOptions();

        options.resultCallback = AfterShowTimedRewardVideo;
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady("rewardedVideo"))
        {
            Advertisement.Show("rewardedVideo", options);
        }
        else
        {
            //Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    public void AfterShowTimedRewardVideo(ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            PlayerPrefs.SetInt("toolsCount", PlayerPrefs.GetInt("toolsCount") + 1);
            uiController.RefreshUIToolsAndMoney();
            TimedRewards.Instance.ResetTimer();
        }
        else if (showResult == ShowResult.Failed)
        {
            //Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    // // Implement IUnityAdsListener interface methods:
    // public void OnUnityAdsDidFinish(ShowResult showResult)
    // {
    //     // Define conditional logic for each ad completion status:
    //     if (showResult == ShowResult.Finished)
    //     {
    //         // Reward the user for watching the ad to completion.
    //         print("Reward the user for watching the ad to completion.");
    //         // gameController.RestartGame();
    //     }
    //     else if (showResult == ShowResult.Skipped)
    //     {
    //         // Do not reward the user for skipping the ad.
    //         print("Do not reward the user for skipping the ad");
    //     }
    //     else if (showResult == ShowResult.Failed)
    //     {
    //         Debug.LogWarning("The ad did not finish due to an error.");
    //     }
    // }

    public void RemoveAds()
    {
        // PlayerPrefs.SetInt("countToShowAd", 0);
        PlayerPrefs.SetInt("removeAds", 1);
        //StopCoroutine(ShowBannerWhenInitialized());
        Advertisement.Banner.Hide();
        //SceneManager.LoadScene(0);
    }

    public void RestoreProduct(Product product)
    {
        //Calls when user reinstall the app
        if (product.definition.id == "no_ads2")
        {
            RemoveAds();
        }
    }
}
