using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("----- Game Control -----")]
    public int[] nextExp = {10, 30, 50, 100, 200};
    public float maxGameTime = 2 * 10.0f;
    public float gameTime;
    public bool isLive;
    public int playerID;

    [Header("----- Player Info -----")]
    public int level;
    public int kill;
    public int exp;
    public float maxHealth;
    public float health;
    
    [Header("----- Game Object -----")]
    public static GameManager instance;
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject nuclear;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if(isLive)
        {
            gameTime += Time.deltaTime;

            if(gameTime > maxGameTime)
            {
                gameTime = maxGameTime;
                GameVictory();
            }
        }
    }

    public void GameStart(int id)
    {
        playerID = id;
        isLive = true;
        health = maxHealth;
        uiLevelUp.Select(playerID % 2); // 기본 무기를 지급
        player.gameObject.SetActive(true);
        
        AudioManager.instance.PlayBgm(isLive);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);

        Resume();
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    public void GetExp()
    {
        if(!isLive) return;

        exp++;

        if(exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            exp = 0;
            level++;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;

        AudioManager.instance.PlayBgm(isLive);
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;

        AudioManager.instance.PlayBgm(isLive);
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        nuclear.SetActive(true);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);

        yield return new WaitForSeconds(0.5f);
        nuclear.SetActive(false);
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);

        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();
    }
}
