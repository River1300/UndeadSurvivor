using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    
    public SaveData data = new SaveData();
    public SaveLoad open;
    public AchiveManager achive;

    public int slotNum;
    public string path;

    void Awake()
    {
        if(instance == null) instance = this;
        else if(instance != this) Destroy(instance.gameObject);
        DontDestroyOnLoad(this.gameObject);

        path = Application.persistentDataPath + "/save";
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path + slotNum.ToString(), json);
    }
    public void LoadData()
    {
        string json = File.ReadAllText(path + slotNum.ToString());
        data = JsonUtility.FromJson<SaveData>(json);
    }
}

public class SaveData
{
    public string username;
    public string saveTime;
    public AchiveData achiveData;
}