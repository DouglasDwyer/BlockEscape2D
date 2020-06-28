using GoogleMobileAds.Api;
using System;
using System.Linq;
using UnityEngine;

public class AdvertisementManager : MonoBehaviour {

    public static AdvertisementManager instance;

    private static bool initialized = false;
    private static BannerView advertBanner;
    private static InterstitialAd advertInterstitial;
    private static RewardBasedVideoAd advertVideo;

    public string androidAppID = "ca-app-pub-3940256099942544~3347511713";
    public string appleAppID = "ca-app-pub-3940256099942544~1458002511";
    public string advertisementAndroidBannerID = "ca-app-pub-3940256099942544/6300978111";
    public string advertisementIosBannerID = "ca-app-pub-3940256099942544/2934735716";
    public string advertisementAndroidInterstitialID = "ca-app-pub-3940256099942544/1033173712";
    public string advertisementIosInterstitialID = "ca-app-pub-3940256099942544/4411468910";
    public string advertisementAndroidVideoID = "ca-app-pub-3940256099942544/5224354917";
    public string advertisementIosVideoID = "ca-app-pub-3940256099942544/1712485313";
    public string[] permanentTestDevices;

    public void Start() {
        instance = this;
        if (initialized) {
            if (!PlayerBoxController.currentPlayer.noAdvertisements)
            {
                advertBanner.LoadAd(GetNextRequest());
            }
            return;
        }
        initialized = true;
#if UNITY_ANDROID
        string appId = androidAppID;
#elif UNITY_IPHONE
            string appId = appleAppID;
#else
            string appId = "unknown-platform";
#endif
        MobileAds.Initialize(appId);
        advertBanner = GetBannerView();
        if (!PlayerBoxController.currentPlayer.noAdvertisements) { advertBanner.LoadAd(GetNextRequest()); }
        advertInterstitial = GetInterstitialAd();
        advertInterstitial.OnAdClosed += OnInterstitialUnload;
        OnInterstitialUnload(null, null);
        advertVideo = GetVideoAd();
        advertVideo.OnAdClosed += OnVideoUnload;
        advertVideo.OnAdRewarded += OnAdRewarded;
        OnVideoUnload(null, null);
    }

    public void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (initialized)
        {
            advertBanner.SetPosition(AdPosition.Bottom);
        }
#endif
    }

    public void ShowInterstitial()
    {
        if(!initialized || !advertInterstitial.IsLoaded() || PlayerBoxController.currentPlayer.noAdvertisements) { return; }
        advertInterstitial.Show();
    }

    public void ShowRewardedVideo()
    {
        if (!ReadyVideo()) { return; }
        advertVideo.Show();
    }

    public bool ReadyVideo()
    {
        if(!initialized)
        {
            return false;
        }
        else if(!advertVideo.IsLoaded())
        {
            OnVideoUnload(null, null);
            return false;
        }
        return true;
    }

    public AdRequest GetNextRequest() {
        if (permanentTestDevices.Contains(SystemInfo.deviceUniqueIdentifier)) {
            return new AdRequest.Builder()
                .AddTestDevice(SystemInfo.deviceUniqueIdentifier)
                .AddTestDevice(AdRequest.TestDeviceSimulator)
                .Build();
        }
        else
        {
            return new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)
            .Build();
        }
    }

    private BannerView GetBannerView()
    {
#if UNITY_ANDROID
        return new BannerView(advertisementAndroidBannerID, AdSize.Banner, AdPosition.Bottom);
#elif UNITY_IPHONE
        return new BannerView(advertisementIosBannerID, AdSize.Banner, AdPosition.Bottom);
#else
        return new BannerView("unknown-platform", AdSize.Banner, AdPosition.Bottom);
#endif
    }

    private InterstitialAd GetInterstitialAd()
    {
#if UNITY_ANDROID
        return new InterstitialAd(advertisementAndroidInterstitialID);
#elif UNITY_IPHONE
        return new InterstitialAd(advertisementIosInterstitialID);
#else
        return new InterstitialAd("unknown-platform");
#endif
    }

    private RewardBasedVideoAd GetVideoAd()
    {
        return RewardBasedVideoAd.Instance;
    }

    private string GetVideoAdID()
    {
#if UNITY_ANDROID
        return advertisementAndroidVideoID;
#elif UNITY_IPHONE
        return advertisementIosVideoID;
#else
        return "unknown-platform";
#endif
    }

    private void OnInterstitialUnload(object sender, EventArgs useless)
    {
        advertInterstitial.LoadAd(GetNextRequest());
    }

    private void OnVideoUnload(object sender, EventArgs useless)
    {
        advertVideo.LoadAd(GetNextRequest(), GetVideoAdID());
    }

    public void OnAdRewarded(object sender, Reward args)
    {
        PlayerBoxController.currentPlayer.timeUntilNextAd = DateTime.Now.AddMinutes(60);
        PlayerBoxController.currentPlayer.goldCoins += 50;
        PlayerBoxController.currentPlayer.silverCoins += 5;
        GUIManagement.LoadGUISetupSingular("VideoRewarded");
        OnVideoUnload(null, null);
    }

    public void HideBanner()
    {
        advertBanner.Hide();
    }
}