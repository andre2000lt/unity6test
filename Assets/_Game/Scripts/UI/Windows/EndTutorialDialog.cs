
using UnityEngine;
using UnityEngine.UI;


public class EndTutorialDialog : PopupWindow
{
    [SerializeField] private Button _startButton;


    private void Awake()
    {
        _startButton.onClick.AddListener(StartGame);
    }


    private void StartGame()
    {
        PlayerDataManager.SetTutorialViewed();
        GameScenesController.LoadScene(GameScene.Main);
    }
}
