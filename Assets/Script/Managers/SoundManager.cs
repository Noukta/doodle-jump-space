using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    
    public Toggle soundToggle;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        soundToggle.isOn = DataManager.instance.Sound;
    }

    public void Play(AudioSource sound)
    {
        if (!soundToggle.isOn)
            return;
        sound.Play();
    }

    public void ActiveSound()
    {
        DataManager.instance.SetSound(soundToggle.isOn);
    }
}
