using UnityEngine;
using UnityEngine.Events;

public static class InAppData
{
    public static UnityEvent AdsDisabled_Event = new UnityEvent();

    private const string NO_ADS_KEY = "adsDisabled11";



    public static void DisableAds()
    {
        PlayerPrefs.SetInt(NO_ADS_KEY, 1);
        AdsDisabled_Event?.Invoke();
    }
    

    public static bool IsAdsDisabled()
    {
        return PlayerPrefs.GetInt(NO_ADS_KEY, 0) == 1;
    }
}
