using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Resources;

// #. Spawner 클래스 : 소환과 관련된 기능을 관리
public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public SpawnData[] spawnData;

    public List<SpawnData> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    public float levelTime;
    float timer;
    int level;
    int[] ranNum;

    void Awake()
    {
        spawnList = new List<SpawnData>();
        spawnPoint = GetComponentsInChildren<Transform>();
        levelTime = GameManager.instance.maxGameTime / spawnData.Length;

        ranNum = new int[] { 3, 6, 9, 12, 15 };
    }

    void Update()
    {
        if(GameManager.instance.isLive)
        {
            timer += Time.deltaTime;
            level = Mathf.Min(Mathf.FloorToInt(
                GameManager.instance.gameTime / levelTime), spawnData.Length - 1);

            if(!spawnEnd)
            {
                if(timer > spawnList[spawnIndex].spawnTime)
                {
                    SpecialSpawn();

                    timer = 0;
                }
            }

            if(timer > spawnData[level].spawnTime)
            {
                Spawn();

                timer = 0;
            }
        }
    }

    public void ReadSpawnFile(int playerLevel)
    {
        if(playerLevel % 3 != 0) return;
        if(playerLevel > 15)
        {
            int randomIndex = Random.Range(0, ranNum.Length);
            playerLevel = ranNum[randomIndex];
        }

        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        TextAsset textFile = Resources.Load("Level" + playerLevel) as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        while(stringReader != null)
        {
            string line = stringReader.ReadLine();

            if(line == null) break;

            SpawnData SPData = new SpawnData();
            SPData.spriteType = int.Parse(line.Split(',')[0]);
            SPData.spawnTime = float.Parse(line.Split(',')[1]);
            SPData.health = int.Parse(line.Split(',')[2]);
            SPData.speed = float.Parse(line.Split(',')[3]);
            SPData.spPoint = int.Parse(line.Split(',')[4]);
            spawnList.Add(SPData);
        }

        stringReader.Close();
    }

    void SpecialSpawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);

        enemy.GetComponent<Enemy>().Init(spawnList[spawnIndex]);
        enemy.transform.position = spawnPoint[spawnList[spawnIndex++].spPoint].position;

        if(spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            spawnIndex -= 1;
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
    public int spPoint;
}