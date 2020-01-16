using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour {

    public Text score, highscore;
    public Animator levelScreen, menuScreen;
    public AudioSource newHighscoreSound;

    private readonly string leaderboardId = "CgkIjqy7y6AREAIQAA";

    public void SetScore(int score)
    {
        this.score.text = score.ToString();
    }

    public void SetHighscore(int highscore)
    {
        this.highscore.text = highscore.ToString();
    }

    public void GameOver()
    {
        ScreenManager.instance.OpenPanel(GetComponent<Animator>());
        SetScore(LevelManager.instance.score);
        if (DataManager.instance.Highscore < LevelManager.instance.score)
        {
            SoundManager.instance.Play(newHighscoreSound);
            DataManager.instance.SetHighscore(LevelManager.instance.score);
            if (Social.localUser.authenticated)
            {
                Social.ReportScore(DataManager.instance.Highscore, leaderboardId, (bool success) => { });
            }
            else
            {
                // authenticate user:
                Social.localUser.Authenticate((bool success) =>
                {
                    if(success)
                        Social.ReportScore(DataManager.instance.Highscore, leaderboardId, (bool success2) => { });
                });
            }
        }
        SetHighscore(DataManager.instance.Highscore);
        StartCoroutine(AdsManager.instance.ShowAds());
    }

    public void Replay()
    {
        StartCoroutine(Replay_());
    }

    private IEnumerator Replay_()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(0);
        yield return new WaitUntil(() => op.isDone);
        ScreenManager.instance.OpenPanel(levelScreen);
        levelScreen.GetComponent<LevelScreen>().SetScore(0);
        LevelManager.instance.Init();
    }
}
