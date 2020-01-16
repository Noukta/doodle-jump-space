using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour {

    public Animator levelScreen;

    public void Resume()
    {
        Time.timeScale = 1;
        ScreenManager.instance.OpenPanel(levelScreen.GetComponent<Animator>());
    }
}
