using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("----- Game Control -----")]
    public int[] nextExp = {10, 30, 50, 100, 200};
    public float maxGameTime = 2 * 10.0f;
    public float gameTime;

    [Header("----- Player Info -----")]
    public int level;
    public int kill;
    public int exp;
    
    [Header("----- Game Object -----")]
    public static GameManager instance;
    public PoolManager pool;
    public Player player;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        gameTime += Time.deltaTime;

        if(gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
        }
    }

    public void GetExp()
    {
        exp++;

        if(exp >= nextExp[level])
        {
            exp = 0;
            level++;
        }
    }
}
