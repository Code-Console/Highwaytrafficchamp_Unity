using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
//using GoogleMobileAds.Api.Mediation.UnityAds;

public class AdsManager 
{
    private static AdsManager sInstance = new AdsManager();
    InterstitialAd mInterstitial;
    //RewardedAd mRewardVideo;
    private RewardBasedVideoAd mRewardVideo;
    public bool isContinue =  false;
    bool shouldContinue    = false;
    public static AdsManager Instance
    {
        get { return sInstance; }
    }

    public void Init()
    {
        //Advertisement.Initialize("1257465");
          //UnityAds.SetGDPRConsentMetaData(true);
#if UNITY_ANDROID
            string appId = "ca-app-pub-7665074309496944~9825536910";
#elif UNITY_IPHONE
            string appId = "ca-app-pub-4149261229279364~1022264796";
#else
            string appId = "unexpected_platform";
#endif
        MobileAds.Initialize(appId);




#if UNITY_ANDROID
              string adUnitId = "ca-app-pub-7665074309496944/3779003313";
#elif UNITY_IPHONE
              string adUnitId = "ca-app-pub-4149261229279364/2480129770";
#else
              string adUnitId = "unexpected_platform";
#endif
        this.mInterstitial = new InterstitialAd(adUnitId);
        this.mInterstitial.OnAdClosed += HandleOnAdClosed;


        this.mRewardVideo = RewardBasedVideoAd.Instance;
        this.mRewardVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        this.mRewardVideo.OnAdClosed   += HandleRewardBasedVideoClosed;


        LoadInterstitial();
        LoadRewardVideo();
    }
    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        Debug.Log("!!!!!!!!!!!! HandleOnAdClosed!!!!!!!!!!");
        LoadInterstitial();
    }
    public void LoadInterstitial()
    {
        if (!mInterstitial.IsLoaded())
        {
            AdRequest request = new AdRequest.Builder().Build();
            mInterstitial.LoadAd(request);
        }
    }
    public void ShowInterstitial()
    {
        if (mInterstitial != null && mInterstitial.IsLoaded())
        {
            mInterstitial.Show();
        }
    }
    public void LoadRewardVideo()
    {
        
        isContinue = false;
        shouldContinue = false;

        if (!this.mRewardVideo.IsLoaded())
        {



#if UNITY_ANDROID
            string adRewardId = "ca-app-pub-7665074309496944/5255736515";//""ca-app-pub-3940256099942544/5224354917; 
#elif UNITY_IPHONE
                       string adRewardId = "ca-app-pub-3395412980708319/9503846983";
#else
                       string adRewardId   = "ca-app-pub-7665074309496944/5255736515";
#endif
            //this.mRewardVideo = new RewardedAd(adRewardId);
            AdRequest request = new AdRequest.Builder().Build();
            //this.mRewardVideo.LoadAd(request);
            this.mRewardVideo.LoadAd(request, adRewardId);
            Debug.Log("!!!!!!LoadRewardVideo!!!" + "     " + isContinue + "           " + shouldContinue);
        }
    }
    public bool isLoadReward()
    {
        if (this.mRewardVideo == null)
            return false;

        return this.mRewardVideo.IsLoaded();
        
    }
    public void ShowRewardVideo()
    {
        Debug.Log("!!!!!!ShowRewardVideo!!!" + "     "+isContinue+"      "+ mRewardVideo.IsLoaded());
        if (this.mRewardVideo.IsLoaded())
        {
            shouldContinue = false;
            this.mRewardVideo.Show();
        }
        else
        {
            if(isContinue)
            {
              // AskContinueUI.Instance.Skip();
               //GameObject.Find("AskContinue").GetComponent<AskContinueUI>().Skip();
            }
            LoadRewardVideo();
        }
    }
    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        //string type = args.Type;
        //double amount = args.Amount;
        
        if (!isContinue)
        {
            //UnityAdUI.Instance.DoReward();
            //GlobalValue.GemsAll += GlobalValue.rewardedVideoAd;
            //SoundManager.PlaySfx(UnityAdUI.Instance.sound);
            //UnityAdUI.Instance.ShowMessage();
            //SoundManager.PlaySfx(SoundManager.Instance.soundRewarded);
        }
        else
        {
            shouldContinue = true;
        }

        GameShop.IS.mTotalCash += 500;
        GameShop.IS.Save();
    
        Debug.Log("!!!!!!HandleRewardBasedVideoRewarded!!!" + "     " + isContinue+"           "+ shouldContinue);
    }
    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        Debug.Log("!!!!!!HandleRewardBasedVideoClosed!!!" + "     " + isContinue + "           " + shouldContinue);

        if (isContinue)
        {
            //Invoke("RevivePlayer", 0.5f);		
            //if (shouldContinue)
            //{
            //    GameObject.Find("AskContinue").GetComponent<AskContinueUI>().RewardWithRevive();
            //}
            //else
            //{
            //    GameObject.Find("AskContinue").GetComponent<AskContinueUI>().Skip();
            //}

        }
        //else
        {
            LoadRewardVideo();
        }
        
    }

}
