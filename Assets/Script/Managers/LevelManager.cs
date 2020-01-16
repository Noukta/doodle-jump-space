using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [HideInInspector]
    public int score = 0;
    public float difficulty = .001f;
    public int noPlatformMax = 2;
    public float initY = -8f;
    
    public float[] initGeneralProbabilities = { .98f, .99f };
    public float[] initPlatformProbabilities = { .98f, .99f };
    public float[] initMonsterProbabilities = { .9f, .95f, 1f };
    public float[] initPowerupProbabilities = { .04f, .07f, .09f, .1f };
    
    public GameObject[] platforms;
    public GameObject[] powerups;
    public GameObject[] monsters;

    public LevelScreen levelScreen;
    public GameOverScreen gameOverScreen;
    //public AudioSource fallSound;

    private float[] generalProbabilities;
    private float[] platformProbabilities;
    private float[] monsterProbabilities;
    private float[] powerupProbabilities;

    private float platformDelta;
    private float movingPlatformDelta;
    private float monsterDelta;
    private float powerupDelta = .5f;
    private int noPlatform = 0;
    private float currentY;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        float edge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        platformDelta = edge - platforms[0].GetComponent<SpriteRenderer>().bounds.size.x;
        movingPlatformDelta = edge - platforms[1].GetComponent<SpriteRenderer>().bounds.size.x - 2f;
        monsterDelta = edge - monsters[2].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    public void Init()
    {
        Time.timeScale = 1;
        score = 0;
        generalProbabilities = (float[]) initGeneralProbabilities.Clone();
        platformProbabilities = (float[]) initPlatformProbabilities.Clone();
        monsterProbabilities = (float[]) initMonsterProbabilities.Clone();
        powerupProbabilities = (float[]) initPowerupProbabilities.Clone();
        currentY = initY;
        //Instantiate the starting platforms
        for (int i = 0; i < Camera.main.orthographicSize; i++)
        {
            currentY += 2;
            float currentX = Random.Range(-platformDelta, platformDelta);
            Vector3 platformPosition = new Vector3(currentX, currentY, 0f);
            Instantiate(platforms[0], platformPosition, Quaternion.identity);
        }
        noPlatform = noPlatformMax;
        Player.SetActive(true);
    }

    private void Update()
    {
        if (Player.Active && Camera.main.transform.position.y + 12 > currentY)
        {
            score++;
            levelScreen.SetScore(score);
            #region Instantiate platform, monster, powerup ...
            currentY += 2;
            //choose platform, monster or space
            float random = Random.value;

            if (noPlatform == 0)
                random = 0;

            if (random <= generalProbabilities[0])
            {
                random = Random.value;
                if (noPlatform == 0)
                    random = Mathf.Min(random, platformProbabilities[1]);

                Vector3 platformPosition = new Vector3(0, currentY, 0f);
                if (random <= platformProbabilities[0])
                {
                    platformPosition.x = Random.Range(-platformDelta, platformDelta);
                    GameObject platform = Instantiate(platforms[0], platformPosition, Quaternion.identity);
                    InstantiatePowerup(platform);
                    noPlatform = noPlatformMax;
                }
                else if (random <= platformProbabilities[1])
                {
                    platformPosition.x = Random.Range(-movingPlatformDelta, movingPlatformDelta);
                    GameObject platform = Instantiate(platforms[1], platformPosition, Quaternion.identity);
                    InstantiatePowerup(platform);
                    noPlatform = noPlatformMax;
                }
                else
                {
                    platformPosition.x = Random.Range(-platformDelta, platformDelta);
                    Instantiate(platforms[2], platformPosition, Quaternion.identity);
                }
            }
            else if (random <= generalProbabilities[1])
            {
                random = Random.value;
                Vector3 monsterPosition = new Vector3(0f, currentY, 0f);
                monsterPosition.x = Random.Range(-monsterDelta, monsterDelta);
                for (int i = 0; i < monsterProbabilities.Length; i++)
                {
                    if (random <= monsterProbabilities[i])
                    {
                        Instantiate(monsters[i], monsterPosition, Quaternion.identity);
                        break;
                    }
                }
            }
            noPlatform--;
            #endregion
            #region Make it more Difficult
            //less platform, more monsters and space
            generalProbabilities[0] = Mathf.Lerp(generalProbabilities[0], 0.1f, difficulty);
            generalProbabilities[1] = Mathf.Lerp(generalProbabilities[1], 0.5f, difficulty);
            //more moving and broken platforms
            platformProbabilities[0] = Mathf.Lerp(platformProbabilities[0], 0.1f, difficulty);
            platformProbabilities[1] = Mathf.Lerp(platformProbabilities[1], .5f, difficulty);
            //harder monsters
            monsterProbabilities[0] = Mathf.Lerp(monsterProbabilities[0], 0.1f, difficulty);
            monsterProbabilities[1] = Mathf.Lerp(monsterProbabilities[1], .5f, difficulty);
            //better powerups
            powerupProbabilities[0] = Mathf.Lerp(powerupProbabilities[0], 0.001f, difficulty);
            powerupProbabilities[1] = Mathf.Lerp(powerupProbabilities[1], .005f, difficulty);
            powerupProbabilities[2] = Mathf.Lerp(powerupProbabilities[2], .01f, difficulty);
            powerupProbabilities[3] = Mathf.Lerp(powerupProbabilities[3], .02f, difficulty);
            #endregion


        }
    }

    private void InstantiatePowerup(GameObject parent)
    {
        float random = Random.value;
        float powerupX = Random.Range(-powerupDelta, powerupDelta);
        GameObject powerup = null;

        for (int i = 0; i < powerupProbabilities.Length; i++)
        {
            if (random < powerupProbabilities[i])
            {
                powerup = Instantiate(powerups[i], parent.transform);
                break;
            }
        }

        if (powerup != null)
        {
            Vector3 powerupPosition = powerup.transform.localPosition;
            powerupPosition.x = powerupX;
            powerup.transform.localPosition = powerupPosition;
        }
    }

    public void GameOver()
    {
        gameOverScreen.GameOver();
    }
}
