
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static AdmobAds;

public class AdmobAds : MonoBehaviour
{
    public static AdmobAds Instance;

    [SerializeField] private TMP_Text _textOutput;
    [SerializeField] private AdBlackScreen _adBlackScreen;

    private string _rollBackRewaededID = "ca-app-pub-3156689890203578/8089279630";
    private RewardedAd _rollBackRewaededAD;

    private string _continueGameRewaededID = "ca-app-pub-3156689890203578/7270225823";
    private RewardedAd _continueGameRewaededAD;

    private string _interstitialId = "ca-app-pub-3156689890203578/7742312194";
    private InterstitialAd _interstitialAd;
    private UnityAction _interstitionCallback;




    private void Awake()
    {
        Instance = this;

        MobileAds.Initialize(initStatus =>
        {
            LoadRewarded(RewardedType.Rollback);
            LoadRewarded(RewardedType.ContinueGame);
            LoadInterstitialAd();
        });
    }


    public void LoadRewarded(RewardedType rewardedType)
    {
        RewardedAd rewardedAd = null;
        string rewardedId = "";

        switch (rewardedType)
        {
            case RewardedType.Rollback:
                rewardedId = _rollBackRewaededID;
                break;
            case RewardedType.ContinueGame:
                rewardedId = _continueGameRewaededID;
                break;
        }


        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }


        var adRequest = new AdRequest();

        RewardedAd.Load(rewardedId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());


                switch (rewardedType)
                {
                    case RewardedType.Rollback:
                        _rollBackRewaededAD = ad;

                        break;
                    case RewardedType.ContinueGame:
                        _continueGameRewaededAD = ad;

                        break;
                }

                RegisterReloadHandler(ad, rewardedType);
            });

        
    }

    public void ShowRewarded(RewardedType rewardedType)
    {
        RewardedAd rewardedAd = null;

        switch (rewardedType)
        {
            case RewardedType.Rollback:
                 rewardedAd = _rollBackRewaededAD;
                break;
            case RewardedType.ContinueGame:
                rewardedAd = _continueGameRewaededAD;
                break;
        }


        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd() && InAppData.IsAdsDisabled() == false)
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(string.Format(rewardMsg, reward.Type, reward.Amount));
                //_textOutput.text = reward.Amount.ToString();

                switch (rewardedType)
                {
                    case RewardedType.Rollback:
                        PlayerDataManager.RollDataBack();
                        break;
                    case RewardedType.ContinueGame:
                        UI_Manager.ContinuedAfterLoss();
                        break;
                }
            });
        }
        else
        {
            switch (rewardedType)
            {
                case RewardedType.Rollback:
                    PlayerDataManager.RollDataBack();
                    break;
                case RewardedType.ContinueGame:
                    UI_Manager.ContinuedAfterLoss();
                    break;
            }
        }

    }


    public void ShowTXT(string text)
    {
        _textOutput.text = text;
    }



    public void PreShowRewarded(RewardedType rewardedType)
    {
        StartCoroutine(PreShowRewardedRoutine(rewardedType));
    }


    private IEnumerator PreShowRewardedRoutine(RewardedType rewardedType)
    {
        int i = 3;
        _adBlackScreen.Show();

        while (i > 0)
        {
            Debug.Log(i);
            i--;

            yield return new WaitForSeconds(0.45f);
        }

        _adBlackScreen.Hide();
        ShowRewarded(rewardedType);
    } 



    private void RegisterReloadHandler(RewardedAd ad, RewardedType rewardedType)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadRewarded(rewardedType);
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadRewarded(rewardedType);
        };
    }




    // Interstitial

    public void LoadInterstitialAd()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(_interstitialId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;
            });

        RegisterReloadInterstitialHandler(_interstitialAd);
    }


    public void ShowInterstitialAd(UnityAction callBack)
    {



        _interstitionCallback = callBack;

        if (_interstitialAd != null && _interstitialAd.CanShowAd() && InAppData.IsAdsDisabled() == false)
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
            _interstitionCallback?.Invoke();
        }


    }


    private void RegisterReloadInterstitialHandler(InterstitialAd interstitialAd)
    {
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
            _interstitionCallback?.Invoke();
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            LoadInterstitialAd();
            _interstitionCallback?.Invoke();
        };
    }
}

    public enum RewardedType
{
    Rollback,
    ContinueGame
}
