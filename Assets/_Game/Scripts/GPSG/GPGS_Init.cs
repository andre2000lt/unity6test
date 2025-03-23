using GooglePlayGames.BasicApi;
using GooglePlayGames;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Events;

public class GPGS_Init : MonoBehaviour
{
	public static GPGS_Init Instance;
	public const string LEADERBOARD_ID = "CgkI3NCV-5EKEAIQBg";

	public static UnityEvent PlayerAuthenticated_Event = new UnityEvent();

    [SerializeField] private TMP_Text _lastAchievementOutput;
	[SerializeField] private Button _achievementsButton;
	[SerializeField] private Button _leaderboardButton;



    private void Awake()
    {
        Instance = this;
    }


    public void Start()
	{
		_achievementsButton.onClick.AddListener(AchievementsButtonClickHandler);
        _leaderboardButton.onClick.AddListener(LeaderboardButtonClickHandler);
		PlayGamesPlatform.Activate();
		PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
	}






    internal void ProcessAuthentication(SignInStatus status)
	{
		if (status == SignInStatus.Success)
		{
            _lastAchievementOutput.text = "";
            PlayerAuthenticated_Event?.Invoke();

        }
		else
		{
            _lastAchievementOutput.text = "!";
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
	}


	// Listeners
	private void AchievementsButtonClickHandler()
	{
		if (PlayGamesPlatform.Instance.localUser.authenticated)
		{
			Social.ShowAchievementsUI();
		}
		else
		{
            PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
        }
	}


    private void LeaderboardButtonClickHandler()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
        else
        {
            PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
        }
    }
}
