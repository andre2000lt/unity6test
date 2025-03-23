using GooglePlayGames.BasicApi;
using GooglePlayGames;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SocialPlatforms;



public class ScoresOutput : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreOutput;
    [SerializeField] private TMP_Text _recordOutput;
    [SerializeField] private TMP_Text _rankOutput;

    private int _score = 0;
    public static string Rank = " - ";


    public void Init()
    {
        InitScores();
        GPGS_Init.PlayerAuthenticated_Event.AddListener(GetPlayerRank);
        PlayerDataManager.DataChanged_Event.AddListener(UpdateScore);
        PlayerDataManager.RecordChanged_Event.AddListener(UpdateRecord);
    }


    private void InitScores()
    {
         _score = PlayerDataManager.GetScore();
        _scoreOutput.text = _score.ToString();

        UpdateRecord();
    }


    private void GetPlayerRank()
    {
        if (!PlayGamesPlatform.Instance.localUser.authenticated) return;

        PlayGamesPlatform.Instance.LoadScores(
            GPGS_Init.LEADERBOARD_ID,
            LeaderboardStart.PlayerCentered,
            1, // Количество загружаемых записей (1 - только текущий игрок)
            LeaderboardCollection.Public,
            LeaderboardTimeSpan.AllTime,
            (data) =>
            {
                if (data.Valid && data.Scores.Length > 0)
                {
                    IScore playerScore = data.PlayerScore;
                    int rank = playerScore.rank;
                    _rankOutput.text = "# " + rank;
                }

            });
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

        GetPlayerRank();
    }
}
