using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    public int maxPeriod = 2;

    private int period = 0;
    //Google Play gameID
    private string gameId = "2582701";

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
        if (Advertisement.isSupported)
            Advertisement.Initialize(gameId);
    }


    public IEnumerator ShowAds()
    {
        period++;
        if (period == maxPeriod)
        {
            if (Advertisement.IsReady())
            {
                Time.timeScale = 0;
                Advertisement.Show();
                yield return new WaitWhile(() => Advertisement.isShowing);
                Time.timeScale = 1;
            }
            period = 0;
        }
    }
}
