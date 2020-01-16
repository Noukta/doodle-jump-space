using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScreen : MonoBehaviour {

    public Text score;
    public Animator pauseScreen;

    public void Pause()
    {
        Time.timeScale = 0;
        ScreenManager.instance.OpenPanel(pauseScreen.GetComponent<Animator>());
    }

    public void SetScore(int score)
    {
        this.score.text = score.ToString();
    }
}
