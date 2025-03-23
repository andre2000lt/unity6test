
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class TargetAchievedDialog: PopupWindow
{
    public static UnityEvent RewardCanceled_Event;
    public static UnityEvent RewardAccepted_Event;


    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _acceptButton;

    [SerializeField] private TargetBlock _targetBlock;
    [SerializeField] private TMP_Text _rewardValueOutput;


    public static void InitStatics()
    {
        RewardCanceled_Event = new UnityEvent();
        RewardAccepted_Event = new UnityEvent();
    }


    private void Awake()
    {
        _closeButton.onClick.AddListener(CancelOnCloseWindow); 
        _acceptButton.onClick.AddListener(AcceptOnCloseWindow);

        Show();
    }


    protected override  void Show()
    {
        base.Show();
        SoundManager.PlaySound(SoundName.GoalCompleted);
        SetRewardParams();
    }


    private void SetRewardParams()
    {
        int targetIndex = PlayerDataManager.GetTargetBlockIndex() - 1;
        int reward = PlayerDataManager.GetReward(targetIndex);

        _targetBlock.SetIndex(targetIndex);
        _rewardValueOutput.text = reward.ToString();
    }


    // Listeners
    private void CancelOnCloseWindow()
    {
        /*        SoundManager.PlaySound(SoundName.CancelReward);
                RewardCanceled_Event?.Invoke();
                gameObject.SetActive(false);*/

        AdmobAds.Instance.ShowInterstitialAd(AcceptOnCloseWindow);
    }


    private void AcceptOnCloseWindow()
    {
        SoundManager.PlaySound(SoundName.AcceptReward);

        //AdmobAds.Instance.ShowRewarded(RewardedType.RewardCoins);
        UI_Manager.GetTargetReward();
        gameObject.SetActive(false);
    }


}
