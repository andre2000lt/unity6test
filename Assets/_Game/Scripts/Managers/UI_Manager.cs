using UnityEngine;



/// <summary>
/// Обрабатывает все UI элементы в игре
/// </summary>
public class UI_Manager : MonoBehaviour
{
    public static UI_Manager _instance;

    [SerializeField] private FlyingText _flyingTextPrefab;
    [SerializeField] private Transform _matrixTransform;


    [Header("Windows")]
    [SerializeField] private GameObject _gameOverWindow;
    [SerializeField] private GameObject _targetAchievedWindow;
    [SerializeField] private GameObject _tutorialInfoWindow;


    public void Init()
    {
        _instance = this;

        TargetAchievedDialog.RewardCanceled_Event.AddListener(OnRewardCancel);
        TargetAchievedDialog.RewardAccepted_Event.AddListener(GetTargetReward);

        GameOverDialog.LevelRestarted_Event.AddListener(OnLevelRestarted);
        GameOverDialog.LevelContinued_Event.AddListener(ContinuedAfterLoss);
    }


    public static void GameOver()
    {
        _instance._gameOverWindow.SetActive(true);
    }

    public static void ShowTargetAchievedWindow()
    {
        _instance._targetAchievedWindow.SetActive(true);
    }
    
    
    public static void ShowTutorialWindow()
    {
        _instance._tutorialInfoWindow.SetActive(true);
    }



    // Listeners
    private void OnRewardCancel()
    {
        PlayerDataManager.IncreaseTargetBlockIndex();
    }


    public static void GetTargetReward()
    {
        int targetIndex = PlayerDataManager.GetTargetBlockIndex() - 1;
        int reward = PlayerDataManager.GetReward(targetIndex);

        var pointsTextPosition = new Vector3(_instance._matrixTransform.position.x,
                                             _instance._matrixTransform.position.y,
                                             -1);
        var pointsText = Instantiate(_instance._flyingTextPrefab, pointsTextPosition, Quaternion.identity);
        pointsText.Init(reward.ToString(), FlyingTextType.TargetReward);


        PlayerDataManager.IncreaseScore(reward);

       


        //_instance._targetAchievedWindow.SetActive(false);
    }


    private void OnLevelRestarted()
    {
        PlayerDataManager.ResetData();
        PlayerDataManager.ResetBackup();

        PlayerDataManager.SetTryCount(1);

        GameScenesController.LoadScene(GameScene.Main);
    }


    public static void ContinuedAfterLoss()
    {
        PlayerDataManager.ContinueAfterLoss();
    }
}
