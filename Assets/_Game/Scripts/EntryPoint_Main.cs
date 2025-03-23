using UnityEngine;


public class EntryPoint_Main : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private UI_Manager _uiManager;
    [SerializeField] private GameMatrix _matrix;

    [Header("UI Outputs")]
    [SerializeField] private TargetInfoOutput _targetInfoOutput;
    [SerializeField] private ScoresOutput _scoresOutput;
    [SerializeField] private LivesOutput _livesOutput;
    [SerializeField] private RollBackButton _rollBackButton;


    private void Awake()
    {
        Application.targetFrameRate = 60;

        GameMatrix.InitStatics();
        PopupWindow.InitEvents();
        Cell.InitStatics();
        TargetAchievedDialog.InitStatics();
        GameOverDialog.InitStatics();

        _uiManager.Init();


        Debug.Log("Lives "+PlayerDataManager.GetLiveCount());
        if (PlayerDataManager.GetScore() == 0 || PlayerDataManager.GetLiveCount() <= 0)
        {
            PlayerDataManager.ResetData();
            PlayerDataManager.ResetBackup();
            PlayerDataManager.SetTryCount(1);

            _levelManager.Init();
            _matrix.Init();
        } 
        else
        {
            _levelManager.Init(false);

            _matrix.Init(PlayerDataManager.GetMatrixMap());
        }
        

        _scoresOutput.Init();
        _livesOutput.Init();
        _targetInfoOutput.Init();
        _rollBackButton.Init();
    }
}
