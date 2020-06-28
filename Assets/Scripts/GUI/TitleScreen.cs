using System;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : GUIWindow
{
    public Text highScoreText;
    public Text goldCoinText;
    public Text silverCoinText;
    public Button rewardedAdvertisementButton;
    public Text timeUntilNextAdText;
    public static bool loadShopOnStart = false;

    public void Start() {
        if(!PlayerBoxController.currentPlayer.playerSuggestedNameAlready && SystemInfo.deviceUniqueIdentifier == "aafc27d674ed92d3de90e0434bac2972")
        {
            GUIManagement.LoadGUISetupSingular("WonNamingPrize");
            PlayerBoxController.currentPlayer.silverCoins += 400;
            PlayerBoxController.currentPlayer.playerSuggestedNameAlready = true;
        }
        if(loadShopOnStart)
        {
            GUIManagement.LoadGUISetupSingular("ShopPremium");
            loadShopOnStart = false;
            return;
        }
        highScoreText.text = "High Score: " + PlayerBoxController.highestScore;
        goldCoinText.text = "x " + PlayerBoxController.currentPlayer.goldCoins;
        silverCoinText.text = "x " + PlayerBoxController.currentPlayer.silverCoins;
        rewardedAdvertisementButton.onClick.AddListener(() => { AdvertisementManager.instance.ShowRewardedVideo(); });
        if(Leaderboard.currentUserStatus == Leaderboard.UserAuthenticationStatus.Authenticated)
        {
            Social.ReportScore(PlayerBoxController.highestScore, GooglePlayIDs.leaderboard_high_scores, succ => { Debug.Log("Posting user's highest score successful: " + succ); });
        }
    }

    public void Update()
    {
#if !UNITY_EDITOR
        if (AdvertisementManager.instance.ReadyVideo() && PlayerBoxController.currentPlayer.timeUntilNextAd < DateTime.Now)
        {
            if (!rewardedAdvertisementButton.gameObject.activeSelf)
            {
                rewardedAdvertisementButton.gameObject.SetActive(true);
            }
        }
        else
        {
#endif
        if (rewardedAdvertisementButton.gameObject.activeSelf)
            {
                rewardedAdvertisementButton.gameObject.SetActive(false);
            }
            timeUntilNextAdText.text = "Next ad in:\n" + Math.Max(0, (int)((PlayerBoxController.currentPlayer.timeUntilNextAd - DateTime.Now).TotalMinutes)) + "m";
#if !UNITY_EDITOR
        }
#endif
    }
}