
using UnityEngine;
using UnityEngine.UI;


public class RollBackButton : MonoBehaviour
{
    private Button _rollBackButton;
    private CanvasGroup _canvasGroup;


    public void Init()
    {
        GameMatrix.PlayerMadeMove_Event.AddListener(DecreaseMovesToActivateButtone);

        _rollBackButton = GetComponent<Button>();
        _canvasGroup = GetComponent<CanvasGroup>();

        _rollBackButton.onClick.AddListener(RollBackData);
    }





    private void Update()
    {
        int score = PlayerDataManager.GetBackupScore();
        SetActive(score > 0);
    }


    private void SetActive(bool isActive)
    {
        var movesToActivateButton = PlayerDataManager.GetMovesToActivateRollback();

        if (isActive && !GameMatrix.IsBisy && movesToActivateButton == 0)
        {
            _rollBackButton.enabled = true;
            _canvasGroup.alpha = 1;
        } 
        else
        {
            _rollBackButton.enabled = false;
            _canvasGroup.alpha = 0.3f;
        }
    }



    // Listeners
    private void RollBackData()
    {
        AdmobAds.Instance.PreShowRewarded(RewardedType.Rollback);
    }


    private void DecreaseMovesToActivateButtone()
    {
        int moves = PlayerDataManager.GetMovesToActivateRollback();
        if(moves > 0)
        {
            PlayerDataManager.SetMovesToActivateRollback(moves - 1);
        }
    }
}
