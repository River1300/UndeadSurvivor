using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("----- Game Control -----")]
    public int[] nextExp = {10, 30, 50, 100, 200};
    public float maxGameTime = 2 * 10.0f;
    public float gameTime;
    public bool isLive;

    [Header("----- Player Info -----")]
    public int level;
    public int kill;
    public int exp;
    public int maxHealth;
    public int health;
    
    [Header("----- Game Object -----")]
    public static GameManager instance;
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        health = maxHealth;
        uiLevelUp.Select(0);
    }

    void Update()
    {
        if(isLive)
        {
            gameTime += Time.deltaTime;

            if(gameTime > maxGameTime)
            {
                gameTime = maxGameTime;
            }
        }
    }

    public void GetExp()
    {
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
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
