using UnityEngine;

public class OpenShopButton : MonoBehaviour
{
    void Awake()
    {
        InAppData.AdsDisabled_Event.AddListener(Deactivate);

        if (InAppData.IsAdsDisabled())
        {
            Deactivate();
        }
    }


    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
