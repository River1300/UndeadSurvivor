using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// #. Spawner 클래스 : 소환과 관련된 기능을 관리
public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    public float levelTime;
    float timer;
    int level;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;
    }

    void Update()
    {
        if(GameManager.instance.isLive)
        {
            timer += Time.deltaTime;
            level = Mathf.Min(Mathf.FloorToInt(
                GameManager.instance.gameTime / levelTime), spawnData.Length - 1);

            if(timer > spawnData[level].spawnTime)
            {
                Spawn();

                timer = 0;
            }
        }
    }

    void Spawn()
    {
// #. PoolManager 클래스로 부터 몬스터 인스턴스를 받는다.
//      => 랜덤한 몬스터를 지정하고 해당 몬스터의 정보를 Enemy 클래스 초기화 함수로 전달
//      => 랜덤한 위치에 소환
        GameObject enemy = GameManager.instance.pool.Get(0);

        int enemyLevel = GetLevel(level);

        enemy.GetComponent<Enemy>().Init(spawnData[enemyLevel]);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }

    int GetLevel(int level)
    {
        if(level == 0) return 0;

        int ran = Random.Range(0, level * 2 + 1);

        switch(ran)
        {
        case 0:
        case 1:
            ran = 0;
            break;

        case 2:
        case 3:
            ran = 1;
            break;

        case 4:
        case 5:
            ran = 2;
            break;

        case 6:
        case 7:
            ran = 3;
            break;

        default:
            ran = 4;
            break;
        }

        return ran;
    }
}

// #. SpawnData 클래스 : 몬스터의 정보
[System.Serializable]
public class SpawnData
{
    public int spriteType;
    public float spawnTime;
    public int health;
    public float speed;
}