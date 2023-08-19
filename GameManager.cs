using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

// #. GameManager 클래스 : 플레이어 + 풀 클래스를 다른 클래스들에게 공유하고 게임을 조정
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("----- Game Control -----")]
    public int[] nextExp = {10, 30, 50, 100, 200};
    public float maxGameTime = 2 * 10.0f;
    public float gameTime;
    public bool isLive;
    public int playerID;
    float weightSpeed = 1.3f;

    [Header("----- Player Info -----")]
    public int level;
    public int kill;
    public int exp;
    public float maxHealth;
    public float health;
    
    [Header("----- Game Object -----")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject nuclear;
    public Transform uiJoy;
    public Volume volume;
    public Spawner spawner;

    void Awake()
    {
// #. 게임 매니저 자신을 스태틱 변수로 만든다.
        instance = this;

        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if(isLive)
        {
            gameTime += Time.deltaTime;

            volume.weight = Mathf.Clamp01(gameTime / maxGameTime * weightSpeed);

            if(gameTime > maxGameTime)
            {
                gameTime = maxGameTime;
                GameVictory();
            }
        }
    }

    public void GameStart(int id)
    {
// #. 사용자가 캐릭터 버튼을 클릭하면 해당 버튼에 등록되어 있는 캐릭터ID를 받는다.
//      => 캐릭터ID를 현재 플레이어ID로 저장
//      => Player 클래스에서 플레이어ID를 받아 외형을 지정
//      => Character 클래스에서 플레이어ID를 받아 특수 능력을 지정
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
// #. 플레이어가 죽으면 Player 클래스에서 호출하여 게임 오버를 실행한다.
        StartCoroutine(GameOverRoutine());
    }

    public void GetExp()
    {
// #. 몬스터가 죽으면 Enemy 클래스에서 호출하여 경험치를 증가 시키고 스킬 레벨업 UI를 출력
        if(!isLive) return;

        exp++;

        if(exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            exp = 0;
            level++;
            uiLevelUp.Show();
            spawner.ReadSpawnFile(level);
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
        uiJoy.localScale = Vector3.zero;

        AudioManager.instance.PlayBgm(isLive);
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
        uiJoy.localScale = Vector3.one;

        AudioManager.instance.PlayBgm(isLive);
    }

    public void Exit()
    {
        Application.Quit();
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
