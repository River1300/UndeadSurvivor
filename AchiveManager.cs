using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;

// #. AchiveManager 클래스 : 업적을 관리하고 캐릭터를 해금하는 역할
public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;

    public AchiveData achiveData = new AchiveData();
    WaitForSecondsRealtime wait;

    void Awake()
    {
        wait = new WaitForSecondsRealtime(5);
    }

    void Start()
    {
        UnlockCharacter();
    }

    void LateUpdate()
    {
        for(int i = 0; i < achiveData.checkAchive.Length; i++)
        {
            string achiveStr = "";

            switch(i)
            {
            case 0:
                achiveStr = "UnlockPuple";
                break;
            case 1:
                achiveStr = "UnlockOrange";
                break;
            default:
                break;
            }

            CheckAchive(i, achiveStr);
        }
    }

    void CheckAchive(int index, string achiveStr)
    {
        bool isAchive = false;

        switch(achiveStr)
        {
        case "UnlockPuple":
            isAchive = GameManager.instance.kill >= 100;
            break;

        case "UnlockOrange":
            isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
            break;
        }

        if(isAchive && !achiveData.checkAchive[index])
        {
            achiveData.checkAchive[index] = isAchive;

            for(int i = 0; i < uiNotice.transform.childCount; i++)
            {
                bool isActive = achiveData.checkAchive[i];
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    public AchiveData GetAchiveData()
    {
        return achiveData;
    }
    public void SetAchiveData(AchiveData newData)
    {
        achiveData = newData;
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        for(int i = 0; i < lockCharacter.Length; i++)
        {
            bool isUnlock = achiveData.checkAchive[i];

            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;

        uiNotice.SetActive(false);
    }
}

[System.Serializable]
public class AchiveData
{
    public int checkAchiveCount;
    public bool[] checkAchive;
    public bool unlockPuple;
    public bool unlockOrange;

    public AchiveData()
    {
        checkAchiveCount = 2;
        checkAchive = new bool[checkAchiveCount];
        checkAchive[0] = unlockPuple;
        checkAchive[1] = unlockOrange;
    }
}