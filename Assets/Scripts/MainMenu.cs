﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public static MainMenu instance;
	public Animator loadGamePanel;
    public Animator settingsPanel;

    public string selectedChapter = "";

    private void Awake()
    {
        instance = this;
    }
    void Start () {
        CloseLoadGamePanel();
        CloseSettingsPanel();
	}
	
	public void ClickLoadGame()
    {
        loadGamePanel.gameObject.SetActive(true);
        loadGamePanel.SetTrigger("activate");

        if (settingsPanel.gameObject.activeInHierarchy)
            settingsPanel.SetTrigger("desactivate");
    }

    public void ClickSettings()
    {
        settingsPanel.gameObject.SetActive(true);
        settingsPanel.SetTrigger("activate");

        if (loadGamePanel.gameObject.activeInHierarchy)
            loadGamePanel.SetTrigger("desactivate");
    }

    public void ExitOutOfLoadGamePanel()
    {
        loadGamePanel.SetTrigger("desactivate");
    }
    public void ExitOutOfSettingsPanel()
    {
        settingsPanel.SetTrigger("desactivate");
    }

    public void CloseLoadGamePanel()
    {
        loadGamePanel.gameObject.SetActive(false);

    }

    public void CloseSettingsPanel()
    {
        settingsPanel.gameObject.SetActive(false);

    }

    public void StartNewGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TestVNScene");
    }

    public void Continue()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(PlayerPrefs.GetInt("Last_Level"));
    }
}
