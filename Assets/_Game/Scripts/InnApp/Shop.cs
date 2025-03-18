using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject _removeAdsWrapper;
    [SerializeField] private GameObject _bonusCodeWrapper;
    [SerializeField] private GameObject _adsDisabledWrapper;
    [SerializeField] private TMP_Text _priceOutput;


    private void OnEnable()
    {
        InAppData.AdsDisabled_Event.AddListener(ShowAdsDisabledContent);

        if (InAppData.IsAdsDisabled())
        {
            ShowAdsDisabledContent();
            //ShowRemoveAdsContent();
        }
        else
        {
            ShowRemoveAdsContent();
        }

        EffectsManager.Scale(gameObject, null);
    }


    private void ShowRemoveAdsContent()
    {
        _removeAdsWrapper.SetActive(true);
        _bonusCodeWrapper.SetActive(true);
        _adsDisabledWrapper.SetActive(false);

        _priceOutput.text = " " + IAP_Manager.NoAdsPrice;
    }
    
    
    private void ShowAdsDisabledContent()
    {
        _removeAdsWrapper.SetActive(false);
        _bonusCodeWrapper.SetActive(false);
        _adsDisabledWrapper.SetActive(true);
    }
}
