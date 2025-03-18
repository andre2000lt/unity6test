using TMPro;
using UnityEngine;



public class ScoresOutput : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreOutput;
    [SerializeField] private TMP_Text _recordOutput;
    [SerializeField] private TMP_Text _placeOutput;

    private int _score = 0;
    public static string Rank = " - ";


    public void Init()
    {
        InitScores();
        PlayerDataManager.DataChanged_Event.AddListener(UpdateScore);
        PlayerDataManager.RecordChanged_Event.AddListener(UpdateRecord);
    }


    private void OnDisable()
    {

    }


    private void InitScores()
    {
         _score = PlayerDataManager.GetScore();
        _scoreOutput.text = _score.ToString();

        UpdateRecord();
    }



    //Listeners
    private void UpdateScore()
    {
        if(PlayerDataManager.GetScore() != _score)
        {
            _score = PlayerDataManager.GetScore();
            _scoreOutput.text = _score.ToString();
        }
    }


    private void UpdateRecord()
    {
        var record = PlayerDataManager.GetRecord();
        _recordOutput.text = record.ToString();
    }
}
