using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class MenuScreen : MonoBehaviour
{
    public LevelManager levelManager;
    public Animator levelScreen;

    private readonly string leaderboardId = GPGSIds.leaderboard_best_score;

    public void Start()
    {
        PlayGamesPlatform.Activate();
        Debug.Log("Activated");
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) => {});
        }
    }

    public void Play()
    {
        ScreenManager.instance.OpenPanel(levelScreen);
        levelScreen.GetComponent<LevelScreen>().SetScore(0);
        levelManager.Init();
    }

    public void Rate()
    {
        Application.OpenURL("market://details?id=stick.jump.space");
    }

    public void Leaderboard()
    {
        if (Social.localUser.authenticated)
        {
            //Update Score
            PlayGamesPlatform.Instance.LoadScores(leaderboardId, LeaderboardStart.PlayerCentered, 1, LeaderboardCollection.Public, LeaderboardTimeSpan.AllTime,
            (LeaderboardScoreData data) =>
                {
                    if(data.PlayerScore.value < DataManager.instance.Highscore)
                        Social.ReportScore(DataManager.instance.Highscore, leaderboardId,
                        (bool success) =>
                            {
                                // show leaderboard UI
                                if(success)
                                    //PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardId);
                                    Social.ShowLeaderboardUI();
                            });
                    else
                        // show leaderboard UI
                        //PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardId);
                        Social.ShowLeaderboardUI();
                }
            );
        }
        else
        {
            // authenticate user:
            Social.localUser.Authenticate((bool success) => {});
        }
    }
}
