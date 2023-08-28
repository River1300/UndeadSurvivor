using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class OpenData : MonoBehaviour
{
    public Text[] slotText;
    public Text newPlayerName;

    bool[] savefile = new bool[4];

    void Start()
    {
        PaintSlot();
    }

    void PaintSlot()
    {
        for(int i = 0; i < 4; i++)
        {
            if(File.Exists(DataManager.instance.path + $"{i}"))
            {
                savefile[i] = true;
                DataManager.instance.slotNum = i;
                DataManager.instance.LoadData();
                slotText[i].text = string.Format(
                    "NAME: " + DataManager.instance.data.username + 
                    "\nSAVETIME: " + DataManager.instance.data.saveTime);
            }
        }
    }

    public void SelectSaveSlot(int slotNum)
    {
        DataManager.instance.slotNum = slotNum;

        if(savefile[slotNum])
        {
            DataManager.instance.open.OpenCheck();
        }
        else
        {
            DataManager.instance.open.OpenCreatePanel();
        }
    }
    public void SelectLoadSlot(int slotNum)
    {
        DataManager.instance.slotNum = slotNum;
        DataManager.instance.LoadData();
        LoadGame();
    }

    public void SaveGame()
    {
        DateTime now = DateTime.Now;

        DataManager.instance.data.username = newPlayerName.text;
        DataManager.instance.data.saveTime = now.ToString("H:m");
        DataManager.instance.data.achiveData = DataManager.instance.achive.GetAchiveData();

        DataManager.instance.SaveData();

        PaintSlot();
    }
    public void LoadGame()
    {
        DataManager.instance.achive.SetAchiveData(DataManager.instance.data.achiveData);
    }
}
