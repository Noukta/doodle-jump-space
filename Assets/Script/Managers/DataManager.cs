using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    //Data variables
    public bool Sound { get; private set; }
    public int Highscore { get; private set; }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        if (!LoadData())
        {
            InitData();
        }

    }

    bool LoadData()
    {
        if (!PlayerPrefs.HasKey("Sound"))
            return false;

        Sound = IntToBool(PlayerPrefs.GetInt("Sound"));
        Highscore = PlayerPrefs.GetInt("Highscore");

        return true;
    }

    void InitData()
    {
        SetSound(true);
        SetHighscore(0);
    }

    public void SetSound(bool sound)
    {
        Sound = sound;
        PlayerPrefs.SetInt("Sound", BoolToInt(sound));
    }

    public void SetHighscore(int highscore)
    {
        this.Highscore = highscore;
        PlayerPrefs.SetInt("Highscore", highscore);
    }
    
    bool IntToBool(int value)
    {
        return value == 1 ? true : false;
    }

    int BoolToInt(bool value)
    {
        return value == true ? 1 : 0;
    }
}