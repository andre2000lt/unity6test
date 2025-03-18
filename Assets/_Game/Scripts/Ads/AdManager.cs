using System.Collections;
using UnityEngine;


public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    [SerializeField] private GameObject _showingAdText;


    private void Awake()
    {
        Instance = this;
    }


    public static void ShowRewarded(int id)
    {

    }
 
    private void OnRewardedShown(int rewardId)
    {
     switch (rewardId)
        {
            case 1:
                PlayerDataManager.RollDataBack();
                break;
            case 2:
                UI_Manager.GetTargetReward();
                break; 
            case 3:
                UI_Manager.ContinuedAfterLoss();
                break;
        }
    }
}
