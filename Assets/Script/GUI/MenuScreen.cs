using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.UI;

public class MenuScreen : MonoBehaviour
{
    public LevelManager levelManager;
    public Animator levelScreen;
    public Image title;

    private readonly string leaderboardId = GPGSIds.leaderboard_best_score;

    public void Start()
    {
        PlayGamesPlatform.Activate();
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
            Social.ReportScore(DataManager.instance.Highscore, leaderboardId,(bool success) =>
            {
                // show leaderboard UI
                if(success)
                    PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderboardId);
            });
        }
        else
        {
            // authenticate user:
            Social.localUser.Authenticate((bool success) => {});
        }
    }
}
