
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class GameOverDialog : PopupWindow
{
    public static UnityEvent LevelRestarted_Event;
    public static UnityEvent LevelContinued_Event;

    [SerializeField] private GameObject _dialogWindow;
    [SerializeField] private Button _restartButton;
    [SerializeField] private MyButton _continueLevelButton;

    [Header("StatsOutputs")]
    [SerializeField] private TMP_Text _scoreOutput;
    [SerializeField] private TMP_Text _bestScoreOutput;
    [SerializeField] private TMP_Text _rankOutput;



    public static void InitStatics()
    {
        LevelRestarted_Event = new UnityEvent();
        LevelContinued_Event = new UnityEvent();
    }


    private void Awake()
    {
        _restartButton.onClick.AddListener(RestartLevel);
        _continueLevelButton.onClick.AddListener(ContinueLevel);   
    }


    private void Start()
    {
        if(PlayerDataManager.GetTryCount() == 0)
        {
            _continueLevelButton.DisableButton();
        } 
        else
        {
            _continueLevelButton.EnableButton();
        }        
    }


    protected override void Show()
    {
        base.Show();
        SetOutputValues();
        SoundManager.PlaySound(SoundName.GameOver);
    }


    private void SetOutputValues()
    {
        _scoreOutput.text = PlayerDataManager.GetScore().ToString();
        _bestScoreOutput.text = PlayerDataManager.GetRecord().ToString();
        _rankOutput.text = "#" + ScoresOutput.Rank;
    }



    //Listeners
    private void RestartLevel()
    {
        SoundManager.PlaySound(SoundName.NeutralClick);
        AdmobAds.Instance.ShowInterstitialAd(OnRestartButtonClick);

    }


    private void ContinueLevel()
    {
        AdmobAds.Instance.ShowRewarded(RewardedType.ContinueGame);
    }


    private void OnRestartButtonClick()
    {
        LevelRestarted_Event?.Invoke();
    }
}
