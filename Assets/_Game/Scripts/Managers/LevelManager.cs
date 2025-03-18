using UnityEngine;



/// <summary>
/// —ледит за матрицей и реагирую на ее событи€ - измен€ет статистику игрока. » сохран€ет ее.
/// ћен€ет режим игры между генерацией матрицы, обработки действтвий игрока и паузой про запуске окон. 
/// </summary>
public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    [SerializeField] private GameObject _blackScreen;
    [SerializeField] protected FlyingText _flyingTextPrefab;

    private bool _isTargetCompleted = false;


    public void Init(bool isGenerateMatrixMode = true)
    {
        _instance = this;
        
        if(isGenerateMatrixMode)
        {
            _blackScreen.SetActive(true);
            Time.timeScale = 100f;
        } 
        else
        {
            TurnOffGenerateMatrixMode(null);
        }


        GameMatrix.BlocksMerged_Event.AddListener(IncreaseLives);
        GameMatrix.BlocksMerged_Event.AddListener(IncreaseScore);
        GameMatrix.AllBlocksMerged_Event.AddListener(CheckIsGameOver);
        GameMatrix.AllBlocksMerged_Event.AddListener(TurnOffGenerateMatrixMode);
        GameMatrix.PlayerMadeMove_Event.AddListener(OnPlayerMove);

        PopupWindow.WindowOpened_Event.AddListener(TurnOnPauseMode);
        PopupWindow.WindowClosed_Event.AddListener(TurnOnPlayerMode);
    }



    //Listeners
    private void TurnOffGenerateMatrixMode(Cell[,] cell)
    {
        _blackScreen.SetActive(false);
        Time.timeScale = 1f;

        GameMatrix.AllBlocksMerged_Event.RemoveListener(TurnOffGenerateMatrixMode);
    }


    public static void TurnOnPauseMode()
    {
        Debug.Log("Pause Mode On");
        GameMatrix.MatrixMode = MatrixMode.Pause;
    }


    public static void TurnOnPlayerMode()
    {
        Debug.Log("Pause Mode Off");
        GameMatrix.MatrixMode = MatrixMode.PlayerMoves;
    }


    private void OnPlayerMove()
    {
        PlayerDataManager.SaveBackup();
        DecreaseLives();
     
       if(Saver.SavesData.IsFirstMoveMade == false)
       {
            UI_Manager.ShowTutorialWindow();
            PlayerDataManager.MarkFirstMove();
       }
        
    }


    private void IncreaseLives(int blockCount, Cell cell)
    {
        if (GameMatrix.MatrixMode != MatrixMode.PlayerMoves) return;
        int liveCount = PlayerDataManager.GetLiveCount();

        if (liveCount >= 5) return;

        PlayerDataManager.SetLiveCount(liveCount + 1); 
    }


    public static void DecreaseLives()
    {
        int liveCount = PlayerDataManager.GetLiveCount();

        PlayerDataManager.SetLiveCount(liveCount - 1);
    }


    private void IncreaseScore(int blockCount, Cell cell)
    {
        if (GameMatrix.MatrixMode != MatrixMode.PlayerMoves) return;

        SoundManager.PlaySound(SoundName.BlocksMerge);

        int[] scoreByBlockCount = new int[10]
        {
            0, 0, 0, 9, 20, 35, 60, 130, 250, 480 
        };

        PlayerDataManager.IncreaseScore(scoreByBlockCount[blockCount]);

        var pointsText = Instantiate(_flyingTextPrefab, cell.transform.position, Quaternion.identity);
        pointsText.Init(scoreByBlockCount[blockCount].ToString(), FlyingTextType.Points);
        

        if (cell.Block.Index + 1 == PlayerDataManager.GetTargetBlockIndex())
        {
            _isTargetCompleted = true;
        }    
    }


    private void CheckIsGameOver(Cell[,] cells)
    {
        if (GameMatrix.MatrixMode != MatrixMode.PlayerMoves) return;

        if (PlayerDataManager.GetScore() != 0)
        {
            PlayerDataManager.SetMatrixMap(cells);
            PlayerDataManager.SaveData();
        } 

        if(_isTargetCompleted)
        {
            UI_Manager.ShowTargetAchievedWindow();
            _isTargetCompleted = false;
        }
        
        if(PlayerDataManager.GetLiveCount() <= 0)
        {
            UI_Manager.GameOver();
        }
    }
}




