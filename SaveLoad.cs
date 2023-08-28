using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public GameObject savePanel;
    public GameObject loadPanel;
    public GameObject createPanel;
    public GameObject checkPanel;

    public void OpenSavePanel()
    {
        savePanel.SetActive(true);
    }
    public void OpenLoadPanel()
    {
        loadPanel.SetActive(true);
    }
    public void OpenCreatePanel()
    {
        createPanel.SetActive(true);
    }
    public void OpenCheck()
    {
        checkPanel.SetActive(true);
    }
}
